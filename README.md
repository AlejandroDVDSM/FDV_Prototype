# Introducción al prototipo

El prototipo desarrollado consiste en un juego de plataformas de scroll lateral en el que el personaje deberá superar obstáculos y llegar hasta el final del mapa.

https://github.com/user-attachments/assets/d533c7d1-1e36-4a5b-9385-52c7e74d81fa

Enlace al vídeo: [https://youtu.be/OZX2B9JNGIE](https://youtu.be/OZX2B9JNGIE).

# Jugador

El personaje es capaz de moverse a los lados y saltar. Para ello, se cuenta con los scripts `CharacterController2D.cs` y `PlayerMovement.cs`. Además, el personaje controlado por el jugador posee a su vez el script `Player` que gestiona las colisiones con el fin de aumentar la puntuación, reducir la salud, etc.

![image](https://github.com/user-attachments/assets/5dfb76be-37f1-41bb-8f29-a5f662b59361)

![image](https://github.com/user-attachments/assets/e20e20d7-5cc9-48d7-b474-502ba3cc1490)

## Movimiento

`CharacterController2D.cs` verifica continuamente si el jugador se encuentra en el suelo y de modificar la propiedad `Velocity` de su `Rigidbody2D` para mover al jugador. Además, se encarga de invocar eventos cuando el jugador se mueva, salte, aterrize al suelo o empiece a caer tras un salto.

```c#
private void FixedUpdate()
{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

public void Move(float move, bool jump)
{
  	// Only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity,
				m_MovementSmoothing);

			// Flip the player
			if (move > 0)
				Flip(false);
			else if (move < 0)
				Flip(true);

			if (move != 0 && m_Grounded)
				OnGroundMoveEvent.Invoke();
		}

		// If the character is not grounded, invoke events depending on if he is jumping or falling
		if (!m_Grounded)
		{
			if (m_Rigidbody2D.velocity.y < -0.01)
				OnFallEvent.Invoke();
			else if (m_Rigidbody2D.velocity.y > 0.00)
				OnJumpEvent.Invoke();
		}
		
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
}
```

Por otra parte, `PlayerMovement.cs` únicamente se encargará de llamar a `CharacterController2D.Move(...)`.

```c#
    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
        _animator.SetFloat("Speed", Mathf.Abs(_horizontalMovement));

        if (Input.GetKeyDown(KeyCode.Space))
            _jump = true;
    }

    private void FixedUpdate()
    {
        _controller.Move(_horizontalMovement * Time.fixedDeltaTime, _jump);
        _jump = false;
    }
```

## Otras características del jugador

Como se mencionó anteriormente, el jugador cuenta con `Player.cs` para controlar los aspectos de su salud, puntuación y colisiones gracias a las funciones `TakeDamage()`, `IncreaseScore()`, `OnCollisionEnter2D()` y `OnTriggerEnter2D()`.

# Recolección de objetos

En el nivel se encuentran diamantes repartidos que incrementan el puntaje del jugador. Estos diamantes tienen la etiqueta `Collectable` y cuentan con un `BoxCollider2D` con la propiedad `IsTrigger` activada. Por tanto, en `Player.cs`, cuando el jugador entra en el trigger del diamante, se dispara el evento `OnTriggerEnter2D()` y se incrementa la puntuación del jugador además de destruir al propio diamante recién recolectado.

![Recolectar](https://github.com/user-attachments/assets/22fe3b8c-4d52-4742-a4d5-919e3afffa59)

```c#
private void IncreaseScore(int score)
{
    _score += score;
    UIManager.Instance.UpdateScoreTxt(_score);
    AudioManager.Instance.Play("Coin");
}

private void OnTriggerEnter2D(Collider2D other)
{
    // If the player hits a collectable increase the score and destroy it
    if (other.CompareTag("Collectable"))
    {
        IncreaseScore(1);
        Destroy(other.gameObject);
    }

    // ...
}
```

# Animaciones

La animación del jugador cuenta con cinco estados distintos:
* `Player_Idle`: animación a ejecutar cuando el jugador se encuentra parado.
* `Player_Run`: animación de correr que se activa cuando el parámetro `Speed` es superior a `0.01`. Si `Speed` llega a 0, se transicionará a `Player_Idle`.
* `Player_Hit`: animación que se activa cuando el trigger `Hit`. Cualquier estado puede transicionar a esta animación gracias al nodo `Any State`.
* `Player_Jump`: animación de salto activada cuando el parámetro `IsJumping` es `true`. 
* `Player_Fall`: animación de caída activada cuando el parámetro `IsFalling` es `true`. Al igual que `Player_Hit`, se puede transicionar desde cualquier estado a esta animación gracias a `Any State`. Por otra parte, este estado es capaz de transicionar a `Player_Idle` o `Player_Run` dependiendo del valor del parámetro `Speed`.

![image](https://github.com/user-attachments/assets/7d8b6cc6-be7c-4683-8a94-530a1294d971)

`Player_Idle`:

![Player_Idle](https://github.com/user-attachments/assets/dcff5d62-e4ab-4473-b567-84497139de63)

`Player_Run`:

![Player_run](https://github.com/user-attachments/assets/dd941913-f60f-4a52-a9f1-2ff64a9f351e)

`Player_Hit`:

![Player_Hit](https://github.com/user-attachments/assets/b89f09bb-4d3a-4025-8be6-98e9bb0674ec)

`Player_Jump` y `Player_Fall`:

![Player_Jump y Fall](https://github.com/user-attachments/assets/f0907b11-2104-4aab-8e36-816655f530c5)

# Activación de sonidos

Para incorporar audio al juego se ha creado `AudioManager.cs`, que contiene una colección de todos los sonidos empleados en el juego.

![image](https://github.com/user-attachments/assets/84589b03-f8a7-4408-aeb8-f35f60a3a106)

`AudioManager.cs` cuenta con dos vectores que almacena objetos de tipo `Sound`, uno de ellos es `Sound[] sounds`, que contiene cualquier audio a emplear. Por otro lado se encuentra `Sound[] footstepsSounds`, que contiene únicamente los audios de pisadas.
`Sound` es un objeto que contiene la configuración de cada audio empleado en el juego.

```c#
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop;
    public bool playOnAwake;
    
    [HideInInspector]
    public AudioSource source;
}
```
Cuando comienza el juego, `AudioManager` crea un `AudioSource` por cada `Sound` asignado a los dos arrays mencionados anteriormente. Así, cuando se desee reproducir un audio, basta simplemente con llamar a la función `Play()` de `AudioManager.cs`. Por ejemplo, cuando el jugador recolecta un diamante se reproduce el audio de la siguiente manera: `AudioManager.Instance.Play("Coin");`

```c#
/// <summary>
/// Play an audio
/// </summary>
/// <param name="name">Name of the audio to be played</param>
public void Play(string name)
{
    Sound s = Array.Find(sounds, sound => sound.name.Equals(name));

    if (s == null)
    {
        Debug.LogWarning("No sound found with name: " + name);
        return;
    }
            
    s.source.Play();
}
```

En cambio, para reproducir las pisadas habrá que llamar a `PlayFootsteps()`, dado que los audios de las pisadas se encuentra en un array de `Sound` distinto.

```c#
/// <summary>
/// Play a random footsteps audio
/// </summary>
public void PlayFootsteps()
{
     if (!_footstepsAudioSource.isPlaying)
    {
        Sound s = footstepsSounds[Random.Range(0, footstepsSounds.Length)];
            
        if (s == null)
        {
            Debug.LogWarning("No footstep sound found");
            return;
        }
        
        _footstepsAudioSource.clip = s.clip;
        _footstepsAudioSource.volume = s.volume;
        _footstepsAudioSource.pitch = s.pitch;
        _footstepsAudioSource.Play();
    }
}
```

# Pooling de objetos

El juego cuenta con un pool de objetos para los proyectiles donde se puede configurar el número de proyectiles a instanciar, con cuanta frecuencia y la dirección que debe seguir su movimiento. Este pool es `BulletSpawner.cs`.

![image](https://github.com/user-attachments/assets/ea7b49db-1c43-49e0-b53b-1470686db7da)

El pool instanciará objetos de tipo `Bullet`, que su única función es la de moverse en la dirección indicada a la velocidad establecida en la propiedad `MoveSpeed` en `Bullet.cs`.

![image](https://github.com/user-attachments/assets/36a7dbde-8cad-4060-9dce-01ef38d234f3)

![Pool](https://github.com/user-attachments/assets/55016881-16f1-4c2c-a20c-2c350a531cd9)

# Scrolling de fondo

El fondo cuenta con tres capas distintas que al combinarse forman un cielo nuboso y un mar.

![image](https://github.com/user-attachments/assets/54ff9272-87cf-4838-8f08-f5132abe4f63)

![image](https://github.com/user-attachments/assets/8fb6da8b-55be-4812-aaf8-8e06d6369d29)

Luego, se aplica parallax con la técnica aprendida en clase desde el script `Parallax.cs`.

```c#
private void Update()
{
    // Update background position
    newPos.x = mainCam.transform.position.x;
    transform.position = newPos;
        
    // Add a parallax effect
    _offset.x = _scrollSpeedX * Time.deltaTime;
        
    for (int i = 0; i < _parallaxLayers.Length; i++)
    {
        _parallaxLayers[i].SetTextureOffset(MainTex, 
            _parallaxLayers[i].GetTextureOffset(MainTex) + _offset / (i + 2.0f));
    }
}
```

![Parallax](https://github.com/user-attachments/assets/b64716e8-a30c-41fd-9f2c-2d3f3dae84e2)

# Tilemap

El tilemap que conforma el nivel cuenta con dos capas:
* `Tilemap_Ground`: capa que emplea los componentes `TilemapCollider2D`, `Rigidbody2D` y `CompositeCollider2D` para añadir colisiones y que así el jugador no caiga al vacío.
* `Tilemap_Decoration`: capa que simplemente sirve para añadir decoración al mapa, como carteles, setas, plantas, etc.

<img width="1366" alt="image" src="https://github.com/user-attachments/assets/cb3eb3eb-be9f-4844-916f-48f3b9bda4a0" />


Componentes para las colisiones de `Tilemap_Ground`:

![image](https://github.com/user-attachments/assets/f8a7b754-20b6-485f-b31c-746660b0a236)

## UI

La interfaz se compone de un panel que contiene los textos que muestran la salud y el puntaje del jugador. Además, se cuenta con un 2º panel que será mostrado cuando el jugador alcanza el final del mapa.

<img width="1366" alt="image" src="https://github.com/user-attachments/assets/15b62688-6119-4e45-83ca-155f4a71c8d1" />

<img width="1369" alt="image" src="https://github.com/user-attachments/assets/b66fe100-f3b9-4a2e-90b3-4461b1a674db" />

La salud se actualiza en la UI cada vez que se recibe daño y el puntaje se actualiza cuando se recolecta un nuevo diamante. Para ello, se llama a las correspondientes funciones de `UIManager.cs`.

`UIManager.Instance.UpdateHpTxt(_currentHealth);`
`UIManager.Instance.UpdateScoreTxt(_score);`

```c#
public void UpdateHpTxt(int hp)
{
    hpText.text = $"x{hp}";
        
    // Add a tween to the HP image to make it shake
    hpImg.rectTransform.DOShakePosition(0.5f, 1f);
    hpImg.rectTransform.DOShakeRotation(0.5f, 1f);
    hpImg.rectTransform.DOShakeScale(0.5f, 1f);

}

public void UpdateScoreTxt(int score)
{
    scoreText.text = $"x{score}";

    // Add a tween to the score image to rotate it
    scoreImg.rectTransform.DORotate(new Vector3(0, 180, 0), 1f)
        .SetEase(Ease.InOutExpo)
        .SetLoops(2, LoopType.Yoyo);
}
```

Para mostrar el panel final del juego, simplemente se llama a `UIManager.Instance.DisplayEndPanel();`:

```c#
/// <summary>
/// Fade the end panel in
/// </summary>
public void DisplayEndPanel()
{
    endPanel.DOFade(1.0f, 1.25f)
        .SetEase(Ease.OutSine);
        
    endText.DOFade(1.0f, 1.25f)
        .SetEase(Ease.OutSine);
}
```

Cabe destacar que el panel final siempre está activo pero no aparece en pantalla debido a que su alfa está a 0, dejando este panel totalmente transparente. Cuando se llama a `DisplayEndPanel()` se modifica su alfa para que sea visible.

# Cámara

El juego, que emplea [Cinemachine](https://unity.com/es/features/cinemachine), cuenta con dos cámaras virtuales distintas:
* `Player VCam`: cámara que sigue al jugador constantemente y que está confinada por el componente `CinemachineConfiner`.
* `Target Group VCam`: cámara que enfocará al jugador y a la puerta que se encuentra al final del nivel.

`Player Vcam`:
<img width="1917" alt="image" src="https://github.com/user-attachments/assets/ca143892-7b41-4fd7-bd1a-8caf4a849987" />

`Camera Confiner`:
![image](https://github.com/user-attachments/assets/0257e0bb-733c-4b50-9957-79c41102b858)


`Target Group VCam`:

![image](https://github.com/user-attachments/assets/96c7cda1-5e23-4bf3-b1ef-8ae578bc5295)

![TargetGroupVCam](https://github.com/user-attachments/assets/c890fdb3-98a5-4ea6-af46-8bf93c7c0bf2)

Además de emplear dos cámaras distintas, se emplea el componente `Cinemachine Impulse Listener` para hacer temblar `Player VCam` cuando el jugador recibe un golpe. El componente `Cinemachine Impulse Source` se encuentra vinculado al personaje controlado por el jugador, dado que será quien haga temblar la cámara.

![image](https://github.com/user-attachments/assets/b8dab9ad-92ca-4925-8bfe-65bfe217684e)

`_impulseSource.GenerateImpulse(); // Make the camera shake`.

![ShakeCam](https://github.com/user-attachments/assets/99223fbe-88d1-4464-b472-efd937dc1b3e)

# Otros aspectos del juego

## Límite del mapa

El mapa cuenta con un rectángulo rojo fuera de la vista del jugador que actúa como trigger para devolver al jugador al comienzo del mapa en caso de que se caiga del mapa.

<img width="1916" alt="image" src="https://github.com/user-attachments/assets/68a14e52-9594-48cf-831d-7aa2cd51884a" />

![OffLimit](https://github.com/user-attachments/assets/eb0e3f65-9f55-403f-96ed-04a64fbc7618)

## Tweens

Para darle más vida al juego se ha importado el paquete [DOTween](https://dotween.demigiant.com/) que facilita la implementación de los tweens. Los tween añadidos han sido:

1. Tween para dar la sensación de que el diamante flota en el aire:

`FloatingObject.cs`
```c#
private void MakeObjectFloat()
{
    transform.DOMoveY(transform.position.y + .4f, 2).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
}
```
![Diamante Tween](https://github.com/user-attachments/assets/faf2a5fd-d6b3-42c8-8803-65ca78791cfe)

2. Tween para dar retroalimentación visual cuando la puntuación incrementa:

`UIManager.cs`
```c#
public void UpdateScoreTxt(int score)
{
    scoreText.text = $"x{score}";

    // Add a tween to the score image to rotate it
    scoreImg.rectTransform.DORotate(new Vector3(0, 180, 0), 1f)
        .SetEase(Ease.InOutExpo)
        .SetLoops(2, LoopType.Yoyo);
    }
 ```
![DIamanteUI](https://github.com/user-attachments/assets/b6eef266-cac5-4ecf-b8e8-cdcdef7da17d)

3. Tween para dar retroalimentación visual cuando la salud del personaje se reduce:

`UIManager.cs`
```c#
public void UpdateHpTxt(int hp)
{
    hpText.text = $"x{hp}";
        
    // Add a tween to the HP image to make it shake
    hpImg.rectTransform.DOShakePosition(0.5f, 1f);
    hpImg.rectTransform.DOShakeRotation(0.5f, 1f);
    hpImg.rectTransform.DOShakeScale(0.5f, 1f);

}
```
   
![CorazónUI](https://github.com/user-attachments/assets/bed7f44b-e3b3-4d48-bd02-3cefbc39c46a)

4. Tween para aumentar el alfa del panel final del juego:

`UIManager.cs`
```c#
public void DisplayEndPanel()
{
    endPanel.DOFade(1.0f, 1.25f)
        .SetEase(Ease.OutSine);
        
    endText.DOFade(1.0f, 1.25f)
        .SetEase(Ease.OutSine);
}
```

![FadeIn](https://github.com/user-attachments/assets/78927443-9cf7-4f57-a530-7813de947aea)

## Area Effector 2D

Al final del nivel se se encuentra situado un muelle que eleva por los aires al jugador gracias al componente `AreaEffector2D`.

![image](https://github.com/user-attachments/assets/5c918ef2-4fca-430b-b71a-af45325ea448)

![AreaEffector](https://github.com/user-attachments/assets/ba2f9437-0207-4bfa-9191-67311ee37642)

# Créditos

## Sprites
* Tiles por "Kenney": [https://kenney.nl/assets/pixel-platformer](https://kenney.nl/assets/pixel-platformer).
* Background por "Free Game Assets (GUI, Sprite, Tilesets)": [https://free-game-assets.itch.io/ocean-and-clouds-free-pixel-art-backgrounds](https://free-game-assets.itch.io/ocean-and-clouds-free-pixel-art-backgrounds).
* Rana Ninja por "Pixel Frog": [https://pixelfrog-assets.itch.io/pixel-adventure-1](https://pixelfrog-assets.itch.io/pixel-adventure-1).

## Audio:
* Sonidos de pisadas por "Pelatho": [https://thowsenmedia.itch.io/video-game-footstep-sound-pack](https://thowsenmedia.itch.io/video-game-footstep-sound-pack).
* Música por "Brackeys" y "Sofia Thirslund": [https://brackeysgames.itch.io/brackeys-platformer-bundle](https://brackeysgames.itch.io/brackeys-platformer-bundle).
* Sonido a la hora de recolectar un diamante por "Brackeys" y "Asbjørn Thirslund": [https://brackeysgames.itch.io/brackeys-platformer-bundle](https://brackeysgames.itch.io/brackeys-platformer-bundle).
* Sonido a la hora de recibir daño por "Mixkit": [https://mixkit.co/free-sound-effects/hurt/](https://mixkit.co/free-sound-effects/hurt/)
