using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    private Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;


    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = moveAction.ReadValue<Vector2>().x * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (jumpAction.triggered)
        {
            AudioManager.Instance.Play("Jump");
            jump = true;
        }
        animator.SetBool("Jump", jump);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }


}

