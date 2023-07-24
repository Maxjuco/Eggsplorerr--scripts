using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class S_PlayerController : MonoBehaviour
{
    [Header("Running")]
    public float moveSpeed, sensitivity, maxForce;
    public GameObject camHolder;
    private Rigidbody rb;
    private Vector2 move, look;
    private float lookRotation;

    [Header("Jumping")]
    public float jumpForce;
    public bool grounded;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Crouching")]
    public float crouchYScale;
    public float crouchSpeed;
    public float crouchMoveSpeed;
    private float startYScale;
    private float currentMoveSpeed;
    public bool crouched;

    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private CapsuleCollider collider;
    private float playerHeight;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
            Jump();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        Crouch();
    }

    private void FixedUpdate()
    {
        Move();
        //better jump
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }
    private void Update()
    {

    }
    private void LateUpdate()
    {
        Look();
    }

    void Jump()
    {
        if (grounded)
        {
            Debug.Log("Jump");
            Vector3 jumpForces = Vector3.up * jumpForce;
            rb.AddForce(jumpForces, ForceMode.Impulse);         
        }
        
    }


    void Move()
    {
        //mouvements
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= currentMoveSpeed;

        //Alligner direction du joueur 
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calculer forces
        Vector3 velocityChange = targetVelocity - currentVelocity;
        //Ramettre la gravité
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        //Limiter force
        Vector3.ClampMagnitude(velocityChange, maxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Look()
    {
        // tourner sur un axe
        transform.Rotate(Vector3.up * look.x * sensitivity);

        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    void Crouch()
    {
        if (crouched) //se relever
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            currentMoveSpeed = moveSpeed;
            crouched = false;
        }
        else //crouch
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            currentMoveSpeed = crouchMoveSpeed;
            crouched = true; 
        }

    }
  

    private void Start()
    {
        currentMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        startYScale = transform.localScale.y;
        crouched = false;
        Cursor.lockState = CursorLockMode.Locked;
        collider = GetComponent<CapsuleCollider>();
        playerHeight = collider.bounds.size.y;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit,playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
