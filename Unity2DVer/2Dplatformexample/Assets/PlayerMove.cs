using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float jumpHeight;
    public BoxCollider2D groundChecker;

    private Rigidbody2D rigid;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        } else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        if(Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpHeight);
        }

        if(groundChecker.IsTouchingLayers())
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
    }
}
