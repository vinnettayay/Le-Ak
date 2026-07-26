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
    [SerializeField] private float maxSight = 70f;

    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isOnGround;

    [Header("Audio")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float runStepInterval = 0.3f;
    private float footstepTimer;
    
    


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
        xRotation = Mathf.Clamp(xRotation, -maxSight, maxSight);

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

        HandleFootsteps(moveHorizontal, moveVertical);
    }
    private void HandleFootsteps(float horizontal, float vertical)
    {
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || MathF.Abs(vertical) > 0.1f;

        if (isOnGround && isMoving)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            AudioClip targetClip = isRunning ? runClip : walkClip;

            if (footstepSource.clip != targetClip)
            {
                footstepSource.Stop();
                footstepSource.clip = targetClip;            
            }

            if (!footstepSource.isPlaying)
            {
                footstepSource.loop = true;
                footstepSource.Play();
            }   
        }
        if (!isMoving)
        {
            footstepSource.Stop();
        }
        if (!isOnGround)
        {
            footstepSource.Stop();
        }
    }
}