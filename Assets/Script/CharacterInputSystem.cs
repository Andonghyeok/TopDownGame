using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterInputSystem : MonoBehaviour
{

    [SerializeField]public float MoveSpeed = 4.0f;
    private Rigidbody2D rigid;
    private Vector2 curMovementInput;
    private Animator animator;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        ApplyMove();

    }
    public void OnMove(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)    //InputActionPhase.Started : 키를누르는 순간만작동, Performed : 키가 눌린상태에서도 값을 받아옴
        {
            animator.SetBool("isWalking", true);
            curMovementInput = context.ReadValue<Vector2>();
            animator.SetFloat("InputX", curMovementInput.x);
            animator.SetFloat("InputY", curMovementInput.y);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("isWalking", false);
            curMovementInput = Vector2.zero;
        }

    }
    public void ApplyMove()
    {

        if (rigid != null)
        {
            rigid.linearVelocity = curMovementInput * MoveSpeed;
        }


    }
}
