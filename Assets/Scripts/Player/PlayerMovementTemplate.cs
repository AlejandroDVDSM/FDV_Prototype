using System;
using TMPro;
using UnityEngine;

public class PlayerMovementTemplate : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _jumpForce = 7.5f;
    
    [Range(0.1f, 1.0f)]
    [SerializeField] private float _jumpLength;

    /* [SerializeField] private TMP_Text _txtCollectedClouds;
    [SerializeField] private TMP_Text _txtPoints; */

    // [SerializeField] private AudioSource _treeAudioSource;
    
    private float _horizontalMovement;
    private Vector3 _direction;

    private Vector2 _jumpDirection = new Vector2(0, 1);
    private bool _isJumping;
    private bool _isGrounded;

    /* private int _collectedClouds;
    private int _points; */
    
    private SpriteRenderer _spriteRenderer;
    // private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    // private static readonly int IsMoving = Animator.StringToHash("isMoving");
    // private static readonly int IsJumping = Animator.StringToHash("isJumping");
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");

        // Flip player's sprite and change animation
        if (_horizontalMovement != 0 && _isGrounded)
        {
            // _animator.SetBool(IsMoving, true);
            _spriteRenderer.flipX = _horizontalMovement > 0;
        } else // Stop the moving animation
            // _animator.SetBool(IsMoving, false);

        // If the player is not jumping and has pressed the spacebar set the 'isJumping' flag to true
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            // _animator.SetBool(IsJumping, true);
        }

    }

    private void FixedUpdate()
    {
        // Moves the player if it is in the ground
        if (_horizontalMovement != 0 && _isGrounded)
        {
            _direction = new Vector3(_horizontalMovement, 0, 0).normalized;
            _rigidbody2D.MovePosition(transform.position + _direction * (_moveSpeed * Time.fixedDeltaTime));
        }

        // If the Spacebar has been pressed in the Update() and the player is in the ground, jump
        if (_isJumping && _isGrounded)
        {
            _isGrounded = false;
            _jumpDirection.x = _horizontalMovement >= 0 ? _jumpLength : -_jumpLength;
            _rigidbody2D.AddForce(_jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"<{gameObject.name}> has collided with <{other.gameObject.name}>");

        // Reset flags if the player collides with the ground or with an obstacle 
        if (other.gameObject.CompareTag("Ground") /*|| other.gameObject.CompareTag("Obstacle") */)
        {
            _isJumping = false;
            _isGrounded = true;
            // _animator.SetBool(IsJumping, false);
            _rigidbody2D.velocity = Vector2.zero; // Cancel any remaining speed
        }

        // Collect the cloud
        /* if (other.gameObject.CompareTag("Cloud"))
        {
            _collectedClouds++;
            _txtCollectedClouds.text = $"Clouds: {_collectedClouds}";
        } */

        // Add points
        /* if (other.gameObject.CompareTag("Mushroom"))
        {
            _points++;
            _txtPoints.text = $"Points: {_points}";
            Destroy(other.gameObject);
        } */
    }

    /* private void OnTriggerEnter2D(Collider2D other)
    {       
        // Reproduce a special sound when we hit a tree sprite
        if (other.gameObject.CompareTag("TreeTrigger"))
        {
            if (!_treeAudioSource.isPlaying)
                _treeAudioSource.Play();
        }
        
        // Add two points when we hit a grass sprite
        if (other.gameObject.CompareTag("GrassTrigger"))
        {
            _points += 2;
            _txtPoints.text = $"Points: {_points}";
        }
    } */
}
