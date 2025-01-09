using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;
    
    private float _horizontalMovement;
    private bool _jump;
    
    private CharacterController2D _controller;
    private Animator _animator;
    
    private void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
    }

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
    
    /// <summary>
    /// Actions executed when the character is jumping
    /// </summary>
    public void OnJumping()
    {
        _animator.SetBool("IsJumping", true);
    }
    
    /// <summary>
    /// Actions executed when the character is falling
    /// </summary>
    public void OnFalling()
    {
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsFalling", true);
    }
    
    /// <summary>
    /// Actions executed when the character has landed
    /// </summary>
    public void OnLanding()
    {
        // _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsFalling", false);
    }
}
