using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCtrl : MonoBehaviour
{
    private Animator anim;
    private new Transform transform;
    private Vector3 moveDir;
    private float moveSpeed = 4.0f;

    private PlayerInput playerInput;
    private InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAction;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();

        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");

        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");

        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            anim.SetFloat("Movement", dir.magnitude);
        };
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            anim.SetFloat("Movement", 0.0f);
        };

        attackAction.performed += ctx =>
        {
            Debug.Log("Attack by c# event");
            anim.SetTrigger("Attack");
        };
    }
    private void Update()
    {
        if (moveDir != Vector3.zero)
        {
            //진행 방향으로 회전
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전 후 전진 방향으로 이동.
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }
#region SEND_MESSEGE
    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        //2차원 좌표를 3차원 좌표로 변환.
        moveDir = new Vector3(dir.x, 0, dir.y);
        //Warrior_Run에니매이션 실행.
        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = ({dir.x}, {dir.y})");
    }
    private void OnAttack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }
#endregion SEND_MESSEGE


#region UNITY_EVENTS
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        //2차원 좌표를 3차원 좌표로 변환.
        moveDir = new Vector3(dir.x, 0, dir.y);
        //Warrior_Run에니매이션 실행.
        anim.SetFloat("Movement", dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase={ctx.phase}");

        if(ctx.performed)//키 입력받고 실행이 완료되었을 때 performed
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");
        }
    }
#endregion UNITY_EVENTS
}
