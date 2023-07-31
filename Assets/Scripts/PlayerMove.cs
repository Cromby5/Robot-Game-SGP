using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Vector3 moveDirection;
    Vector3 lookDirection;
    
    private Rigidbody rigidPlayer;

    [Header("Speed")]
    private float speed; // The current speed the player is set at
    public float walkSpeed; // The default speed of the player
    public float runSpeed; // The Run speed of the player
    public float rotateSpeed; // The rotation speed of the player 


    private void OnEnable()
    {
        InputManager.inputActions.Gameplay.Move.performed += ctx => MoveInput(ctx.ReadValue<Vector2>());
        InputManager.inputActions.Gameplay.Move.canceled += ctx => MoveInput(Vector2.zero);

    }
    private void OnDisable()
    {
        InputManager.inputActions.Gameplay.Move.performed -= ctx => MoveInput(ctx.ReadValue<Vector2>());
        InputManager.inputActions.Gameplay.Move.canceled -= ctx => MoveInput(Vector2.zero);

    }

    void Start()
    {
        rigidPlayer = GetComponent<Rigidbody>();
        speed = walkSpeed;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0) return; // This stops the player from moving when the game is paused
        Move();
        Look();
    }

    public void MoveInput(Vector2 direction)
    {
        moveDirection = new Vector3(direction.x, 0, direction.y); // This determines our move direction in the x and z planes (blue/red in inspector) 
    }
    public void LookInput(Vector2 direction)
    {
        lookDirection = new Vector3(direction.x, 0, direction.y); 
    }
    
    private void Move()
    {
        rigidPlayer.AddForce(moveDirection.normalized * speed, ForceMode.Impulse); // Add the force to move the player
        /*
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed); // Start to rotate in the direction of movement at the speed of rotation 
        }
        */
    }

    private void Look()
    {
        // Look in mouse direction / right stick direction
        Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
        Vector3 cursorPostition = rayFromCameraToCursor.GetPoint(distanceFromCamera);

        Vector3 LookatPosition = cursorPostition;

        transform.LookAt(LookatPosition);
    }

}
