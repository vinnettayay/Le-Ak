using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation;

    public CharacterController playerController;
    public float speed = 10f;
    [SerializeField] private float runSpeed = 5f;
    public float jumpHeight = 3f;

    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isOnGround;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //playerSight
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        //playerMovement
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerController.Move(movement * runSpeed * Time.deltaTime);
        }
        else
        {
            playerController.Move(movement * speed * Time.deltaTime);
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            velocity.y = (float)Math.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}