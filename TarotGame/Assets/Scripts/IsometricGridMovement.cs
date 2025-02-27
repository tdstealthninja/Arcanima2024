using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricGridMovement : MonoBehaviour
{
   [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 5.0f;
    //[SerializeField] float rotationSpeed = 360;
    [SerializeField] PlayerInput input;
    [SerializeField] float inputAngle = 45f;
    private Vector3 playerInput;
    private IsometricPlayerController controller;

    private void Awake()
    {
        controller = new IsometricPlayerController();
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    private void Update()
    {
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void GatherInput(/*InputAction.CallbackContext context*/)
    {
        //playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Vector3 rawInput = context.ReadValue<Vector3>();
        //playerInput = new Vector3(rawInput.x, 0, rawInput.y);
        Vector2 vector3 = controller.Player.Move.ReadValue<Vector2>();
        playerInput = new Vector3(vector3.x, 0, vector3.y);
    }

    void Look()
    {
        if (playerInput != Vector3.zero)
        {
            //var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
            //var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, inputAngle, 0));

            var skewedInput = matrix.MultiplyPoint3x4(playerInput);

            var relative = (transform.position + skewedInput) - transform.position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.LookRotation(relative, Vector3.up); /*Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);*/
        }

    }

    

    public void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * playerInput.magnitude) * speed * Time.deltaTime);
    }
}
