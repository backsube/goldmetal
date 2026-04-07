
using System.Dynamic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{   
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();

        Invoke("Think",5);
    }

    
    void FixedUpdate()
    {
        rigid.linearVelocity = new Vector2(nextMove*1f ,rigid.linearVelocity.y); 

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1,  LayerMask.GetMask("platform"));

        if(rayHit.collider == null)
        {   
            Turn();
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed",nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
        

        float nextThinkTime = Random.Range(2f,5f);
        Invoke("Think",  nextThinkTime);

    }
    void Turn()
    {
        nextMove = nextMove * -1;
        CancelInvoke();
        Invoke("Think",2);
        spriteRenderer.flipX = nextMove == 1;
    }

}

