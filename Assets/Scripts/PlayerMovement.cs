using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;

    private float _horizontalMovement;
    private bool _jump;
    
    private CharacterController2D controller;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            _jump = true;
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMovement * Time.fixedDeltaTime, _jump);
        _jump = false;
    }
}
