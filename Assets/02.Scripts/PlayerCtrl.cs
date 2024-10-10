using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCtrl : MonoBehaviour
{
    private Animator anim;
    private new Transform transform;
    private Vector3 moveDir;
    private float moveSpeed = 4.0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }
    private void Update()
    {
        if(moveDir != Vector3.zero)
        {
            //진행 방향으로 회전
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전 후 전진 방향으로 이동.
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }
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
}
