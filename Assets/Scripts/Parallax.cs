using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _scrollSpeedX = 0.1f;
    
    [SerializeField] private Camera mainCam;
    
    private Renderer _renderer;
    private Material[] _parallaxLayers;
    
    private Vector2 _offset = Vector2.right;

    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private Vector3 newPos;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _parallaxLayers = _renderer.materials;
        newPos = new Vector3(0, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        // Update background position
        newPos.x = mainCam.transform.position.x;
        transform.position = newPos;
        
        // Add a parallax effect
        _offset.x = _scrollSpeedX * Time.deltaTime;
        
        for (int i = 0; i < _parallaxLayers.Length; i++)
            _parallaxLayers[i].SetTextureOffset(MainTex, 
                _parallaxLayers[i].GetTextureOffset(MainTex) + _offset / (i + 2.0f));
    }
}