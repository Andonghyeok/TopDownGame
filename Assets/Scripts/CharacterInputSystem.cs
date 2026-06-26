using JetBrains.Annotations;
using System.Runtime.InteropServices.WindowsRuntime;
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

        if (context.phase == InputActionPhase.Performed)    // InputActionPhase.Started : Ű�������� �������۵�, Performed : Ű�� �������¿����� ���� �޾ƿ�
        { 
            curMovementInput = context.ReadValue<Vector2>();
            animator.SetFloat("InputX", curMovementInput.x);
            animator.SetFloat("InputY", curMovementInput.y);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", curMovementInput.x);
            animator.SetFloat("LastInputY", curMovementInput.y);
            curMovementInput = Vector2.zero;
        }

    }
    public void ApplyMove()
    {
        if(PauseController.IsGamePaused)
        {
            rigid.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }
        if (rigid != null)
        {
            rigid.linearVelocity = curMovementInput * MoveSpeed;
            animator.SetBool("isWalking", rigid.linearVelocity.magnitude > 0);
        }


    }
}
