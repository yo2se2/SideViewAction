using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameObject deathEffect;
    //列挙型で方向を定義
    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2D;
    float speed;
    
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); //playerについているrigidbodyを取得
        direction = DIRECTION_TYPE.LEFT;
    }
    private void Update()
    {
        if (!IsGround())
        {
            ChangeDirection();
        }
    }

    private void FixedUpdate()
    {
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                transform.localScale = new Vector3(1, 1, 1); //右を向く
                speed = 3;
                break;
            case DIRECTION_TYPE.LEFT:
                transform.localScale = new Vector3(-1, 1, 1); //左を向く
                speed = -3;
                break;
        }
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    //敵キャラが崖で反転する。
    bool IsGround()
    {
        //ベクトルの始点と終点を作成
        //transform.position:キャラクターの中心
        Vector3 startVec = transform.position + transform.right * 0.5f * transform.localScale.x;
        Vector3 endVec = startVec - transform.up * 0.5f;
        Debug.DrawLine(startVec, endVec);
        return Physics2D.Linecast(startVec, endVec, blockLayer);
    }

    void ChangeDirection()
    {
        if (direction == DIRECTION_TYPE.RIGHT)
        {
            direction = DIRECTION_TYPE.LEFT;
        }
        else if(direction == DIRECTION_TYPE.LEFT)
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
    }

    public void DestroyEnemy()
    {
        //Instantiate　はプレハブを発生させた時に使用
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }



}