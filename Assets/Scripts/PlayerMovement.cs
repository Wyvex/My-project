using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 6f;
    
    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [Header("Custom UI Joystick")]
    [SerializeField] private RectTransform joystickBG;
    [SerializeField] private RectTransform joystickKnob;
    [SerializeField] private RectTransform joystickJump; 

    private bool uiJumpPressed = false;

    private float velocityThreshold = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetupUIJumpDetection();
    }

    void SetupUIJumpDetection()
    {
        if (joystickJump != null)
        {
            EventTrigger trigger = joystickJump.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) trigger = joystickJump.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnUIJumpDown(); });
            trigger.triggers.Add(entry);
        }
    }

    void OnUIJumpDown()
    {
        uiJumpPressed = true;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (joystickBG != null && joystickKnob != null)
        {
            Vector2 knobPosition = joystickKnob.anchoredPosition;
            float maxRadius = joystickBG.rect.width / 2f;

            if (knobPosition.magnitude > 0.01f)
            {
                Vector2 joystickInput = knobPosition / maxRadius;
                
                horizontalInput = Mathf.Clamp(joystickInput.x, -1f, 1f);
                verticalInput = Mathf.Clamp(joystickInput.y, -1f, 1f);
            }
        }

        rb.linearVelocity = new Vector3(horizontalInput * moveSpeed, rb.linearVelocity.y, verticalInput * moveSpeed);

        if ((Input.GetButtonDown("Jump") || uiJumpPressed) && IsGrounded()) {
            TriggerJump();
        }

        uiJumpPressed = false;
    }

    bool hasJumped = false;

    bool IsGrounded() {
    if (rb.linearVelocity.y < -velocityThreshold) {
        hasJumped = false;
        return false; 
    }

    if (rb.linearVelocity.y > velocityThreshold) {
        return false;
    }

    return !hasJumped; 
    }

    public void TriggerJump() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, moveSpeed, rb.linearVelocity.z);
        hasJumped = true; // Mark that we are in the air
    }
}