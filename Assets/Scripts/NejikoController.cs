using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;

    public int Life()
    {
        return life;
    }

    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown("left")) MoveToLeft();
        if(Input.GetKeyDown("right")) MoveToRight();
        if(Input.GetKeyDown("space")) Jump();

        if(IsStun())
        {
            //動きを止めて気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            float acceleratedZ=moveDirection.z +(accelerationZ*Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ,0,speedZ);

            float ratioX=(targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }

        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //ネジコの向きを考慮したベクトルに変換
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection*Time.deltaTime);

        if(controller.isGrounded) moveDirection.y = 0;
        animator.SetBool("run",moveDirection.z > 0f);
        
    }
    public void MoveToLeft(){
        if(IsStun()) return;
        if(controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    public void MoveToRight(){
        if(IsStun()) return;
        if(controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump(){
        if(IsStun()) return;
        if(controller.isGrounded){
            moveDirection.y = speedJump;
            animator.SetTrigger("jump");
        }
    }

    //CharacterControllerに衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(IsStun()) return;

        if(hit.gameObject.tag == "Robo")
        {
            //ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガーを設定
            animator.SetTrigger("damage");

            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }
}