using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask blockLayer;
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
    Animator animator;

    //SE
    [SerializeField] AudioClip getItemSE;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip stampSE;
    AudioSource audioSource;


    bool isDead = false;

    float jumpPower = 400;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); //playerについているrigidbodyを取得
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isDead)
        {
            return;
        }
        //横方向の入力を取得
        float x = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0)
        {
            //止まっている
            direction = DIRECTION_TYPE.STOP;
        }
        else if(x < 0)
        {
            //左に
            direction = DIRECTION_TYPE.LEFT;
        }
        else if(x > 0)
        {
            //右に
            direction = DIRECTION_TYPE.RIGHT;
        }

        //スペースキーが推されたらJumpさせる。
        if( IsGround())
        {
            if (Input.GetKeyDown("space"))
            { 
                Jump();
            }
            else
            {
                animator.SetBool("isJumping", false);
            }

        }
       
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
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

    void Jump()
    {
        //上に力を加える
        audioSource.PlayOneShot(jumpSE);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        Debug.Log("Jump");
        animator.SetBool("isJumping", true);
    }

    bool IsGround()
    {
        //ベクトルの始点と終点を作成
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        //ベクトルの可視化
        Debug.DrawLine(leftStartPoint,endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint,endPoint,blockLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }
   
    
    //物体の衝突を感知する関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }
        if (collision.gameObject.tag == "Trap")
        {
            PlayerDeath();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("Clear");
            gameManager.GameClear();
        }

        if (collision.gameObject.tag == "Item")
        {
            //アイテム取得
            audioSource.PlayOneShot(getItemSE);
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            //上から踏んだ場合,敵を削除
            if (this.transform.position.y + 0.3 > enemy.transform.position.y)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                audioSource.PlayOneShot(stampSE);
                enemy.DestroyEnemy();
            }
            //横からぶつかった場合,Gameover
            else 
            {
                PlayerDeath();
            }
        }
    }
    void PlayerDeath()
    {
        isDead = true;
        rigidbody2D.velocity = new Vector2(0, 0); //速度の初期化
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.Play("PlayerDeathAnimation");
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        gameManager.GameOver();
    }
}
