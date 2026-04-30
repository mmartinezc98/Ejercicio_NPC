using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 15f;
    public float runSpeed = 25f;
    private float currentSpeed;
    private Animator animator;
    private CharacterController controller;
    [SerializeField] private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isSprinting = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Movement.performed += OnMove;
        inputActions.Player.Movement.canceled += OnMoveCanceled;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;
    }

    void OnDisable()
    {
        inputActions.Player.Movement.performed -= OnMove;
        inputActions.Player.Movement.canceled -= OnMoveCanceled;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        inputActions.Player.Disable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = true;
        currentSpeed = runSpeed;
    }

    void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        Vector3 direction = new Vector3(-moveInput.y, 0, moveInput.x).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            controller.Move(direction * currentSpeed * Time.deltaTime);
            animator.SetFloat("Velocity", isSprinting ? 4 : 2);
        }
        else
        {
            animator.SetFloat("Velocity", 0);
        }

        controller.Move(Vector3.down * 9.8f * Time.deltaTime);
    }
}
