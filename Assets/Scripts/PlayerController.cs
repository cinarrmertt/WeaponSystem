using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _playerTransform;
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    private float horizontal;
    private float vertical;

    [Header("Gravity")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 _gravityVector;

    private void Update()
    {
        Movement();
        Gravity();
        Jump();
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = _playerTransform.right * horizontal + _playerTransform.forward * vertical;
        
        _characterController.Move(movement * TotalSpeed() * Time.deltaTime);
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(_groundCheck.position, 0.4f, groundLayer);

        if (!isGrounded)
        {
            _gravityVector.y += gravity * Mathf.Pow(Time.deltaTime, 2); //Pow değerin üssünü alır.
        }
        else if(_gravityVector.y < 0)
        {
            _gravityVector.y = -0.15f;
        }
        
        _characterController.Move(_gravityVector);
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _gravityVector.y = Mathf.Sqrt(jumpPower * -1 * gravity / 5000);
        }
    }
    float TotalSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            return runSpeed;
        else
            return walkSpeed;
    }
}
