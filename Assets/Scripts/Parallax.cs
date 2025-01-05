using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _scrollSpeedX;
    [SerializeField] private PlayerMovement playerMovement;
    
    private Renderer _renderer;
    private Material[] _parallaxLayers;
    
    private Vector2 _offset = Vector2.right;

    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _parallaxLayers = _renderer.materials;
    }

    private void Update()
    {
        /*if (playerMovement.HorizontalMovement == 0)
            _offset.x = 0;
        else if (playerMovement.HorizontalMovement > 0)
            _offset.x = _scrollSpeedX * Time.deltaTime;
        else if (playerMovement.HorizontalMovement < 0)
            _offset.x = -_scrollSpeedX * Time.deltaTime;*/
            
        _offset.x = _scrollSpeedX * Time.deltaTime;
        
        for (int i = 0; i < _parallaxLayers.Length; i++)
            _parallaxLayers[i].SetTextureOffset(MainTex, 
                _parallaxLayers[i].GetTextureOffset(MainTex) + _offset / (i + 2.0f));
    }
}