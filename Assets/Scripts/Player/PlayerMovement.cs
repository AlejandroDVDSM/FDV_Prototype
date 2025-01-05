using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;
    
    private float _horizontalMovement;
    private bool _jump;
    
    private CharacterController2D _controller;
    private Animator _animator;

    public float HorizontalMovement => _horizontalMovement;
    
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
        {
            _jump = true;
            _animator.SetBool("IsJumping", true);
        }
    }

    private void FixedUpdate()
    {
        _controller.Move(_horizontalMovement * Time.fixedDeltaTime, _jump);
        //AudioManager.Instance.PlayFootsteps();
        _jump = false;
    }

    public void OnLanding()
    {
        _animator.SetBool("IsJumping", false);
    }
}
