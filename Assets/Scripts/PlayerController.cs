using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    private bool isGrounded;
    private bool isJumping;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float moveInput;
    public float jumpVelocity;
    public float speed;
    public float constantSpeed;

    private float jumpTimeCounter;
    public float jumpTime;
    
    void Awake()
    {
        
    }

    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        //set frame rate
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        //constant run
        rigidbody2d.velocity = new Vector2(constantSpeed, rigidbody2d.velocity.y);
        //movement code
        moveInput = Input.GetAxisRaw("Horizontal");
        rigidbody2d.velocity = new Vector2(moveInput * speed, rigidbody2d.velocity.y);
    }

    void Update()
    {
        

        //jump code
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if ( (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isGrounded == true)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidbody2d.velocity = Vector2.up * jumpVelocity;
        }
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rigidbody2d.velocity = Vector2.up * jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }

    }

}
