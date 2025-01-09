using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    [SerializeField] private Sound[] footstepsSounds;
    
    public static AudioManager Instance;

    private AudioSource _footstepsAudioSource;
    
    private void Awake()
    {
        Instance = this;
        
        // Add an AudioSource and set it up for each sound
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
            if (s.playOnAwake)
                Play(s.name);
        }

        // Add just one AudioSource for all the footsteps sounds
        _footstepsAudioSource = gameObject.AddComponent<AudioSource>();
        _footstepsAudioSource.volume = 1f;
        _footstepsAudioSource.pitch = 1f;
    }

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
}
