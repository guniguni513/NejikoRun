using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejkoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(controller.isGrounded){
            if(Input.GetAxis("Vertical") > 0f){
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }else{
                moveDirection.z =0;
            }

            transform.Rotate(0,Input.GetAxis("Horizontal") * 3,0);

            if(Input.GetButton("Jump")){
                moveDirection.y=speedJump;
                animator.SetTrigger("jump");
            }

        }
        moveDirection.y -= gravity * Time.deltaTime;
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection*Time.deltaTime);

        if(controller.isGrounded) moveDirection.y = 0;
        animator.SetBool("run",moveDirection.z > 0f);
        
    }
}
