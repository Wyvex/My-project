using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    
    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [Header("Custom UI Joystick")]
    [SerializeField] private RectTransform joystickBG;
    [SerializeField] private RectTransform joystickKnob;
    [SerializeField] private RectTransform joystickJump; 

    private bool uiJumpPressed = false;

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
            entry.callback.AddListener((data) => {uiJumpPressed = true;});
            trigger.triggers.Add(entry);
        }
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
            uiJumpPressed = false;
        }
    }

    bool hasJumped = false;

    bool IsGrounded() {
        if (rb.linearVelocity.y < -.01f || rb.linearVelocity.y > .01f) {
            hasJumped = false;
            return false; 
        }
        else
        {
            return true;    
        }
    }

    public void TriggerJump() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, moveSpeed, rb.linearVelocity.z);
        hasJumped = true;
    }
}
