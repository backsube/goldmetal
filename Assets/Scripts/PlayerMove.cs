using System.Drawing;
using System.Dynamic;
using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {   

        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump",true);
        }
            

        if(Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.5f, rigid.linearVelocity.y);
        
        }
        if(Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if(Mathf.Abs(rigid.linearVelocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < -maxSpeed)
            rigid.linearVelocity = new Vector2(-maxSpeed, rigid.linearVelocity.y);
        if(rigid.linearVelocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1,  LayerMask.GetMask("platform"));

            if(rayHit.collider != null)
            {   
                if(rayHit.distance < 0.5f)
                    anim.SetBool("isJump",false);
                
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gmaeObject.tag == "eneny")
        {
            OnDamaged(collision.transform.position);
        }
    }
    void OnDamageda(Vector2 targetPos)
    {
        gameObject.layer = 9;

        spriteRenderer.color = new Color(1,1,1,0.4f);
        int dirc = TransformBlock.position.x-targetPos.X > 0?1:-1;
        rigid.AddForce(new Vector2(dirc,1)*7, ForceMode2D.Impulse);

        anim.SetTrigger("hit");

        Invoke("offDamaged",3);
    }

    void offDamaged()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1,1,1,1);
    }
}