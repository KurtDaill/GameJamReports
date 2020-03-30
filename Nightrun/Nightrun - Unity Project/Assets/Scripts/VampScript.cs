using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VampScript : MonoBehaviour
{
    public float moveSpeed, jumpSpeed, groundRush, dashSpeed, dashCooldown, wallJumpX;
    float dashCooldownCounter;
    bool grounded, wallhugging, wallJumpReady, isWallRight;
    int dashesAvailable;
    public AudioSource dash, jump, daywalker, shrine;
    GameObject mainCamera;
    RaycastHit2D[] castResult;
    Rigidbody2D rb;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        dashCooldownCounter = 0;
        castResult = new RaycastHit2D[3];
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.y < 0.1F) rb.AddForce(Vector2.down * 35);

        if (Input.GetAxis("Horizontal") != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (Mathf.Abs(rb.velocity.x) < moveSpeed) rb.velocity = new Vector2((Input.GetAxis("Horizontal") * moveSpeed), rb.velocity.y);
        if (rb.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (rb.velocity.x > 0)
        {
            this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownCounter == 0)
        {
            rb.AddForce(Vector3.left * -Input.GetAxis("Horizontal") * dashSpeed);
            anim.SetBool("isDashing", true);
            dashCooldownCounter = dashCooldown;
            dash.Play();
        }

        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            anim.SetBool("isDashing", true);
        } else
        {
            anim.SetBool("isDashing", false);
        }

        if (grounded || wallhugging)
        {
            if (grounded)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
                wallJumpReady = true;
                if (Input.GetKeyDown(KeyCode.W))
                {
                    rb.AddForce(Vector2.up * jumpSpeed);
                    grounded = false;
                    jump.Play();
                }
            }
            else if (wallhugging)
            {
                anim.SetBool("isWallHugging", true);
                if (wallJumpReady && Input.GetKeyDown(KeyCode.W))
                {
                        rb.AddForce(Vector2.up * jumpSpeed);
                    if (isWallRight)
                    {
                        rb.AddForce(Vector2.left * wallJumpX);
                    } else if (!isWallRight)
                    {
                        rb.AddForce(Vector2.right * wallJumpX);
                    }
                    //wallJumpReady = false;
                    jump.Play();
                }
            }
        }
        else
        {
            anim.SetBool("isWallHugging", false);
            if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", true);
                grounded = false;
                anim.SetBool("isGrounded", false);
            }
            anim.SetBool("isJumping", true);
        }

        if (dashCooldownCounter > 0){
            dashCooldownCounter -= Time.deltaTime;
        }
        else
        {
            dashCooldownCounter = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ground")
        {
            grounded = true;
            anim.SetBool("isGrounded", true);
        }
        else if (collision.collider.gameObject.tag == "Wall")
        {
            wallhugging = true;
            anim.SetBool("isJumping", true);
            
                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 center = collision.collider.bounds.center;

                isWallRight = contactPoint.x < center.x;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DayWalker")
        {
            if (other.gameObject.GetComponent<DayWalkerBehaviour>().KillMe())
            {
                other.gameObject.GetComponent<Animator>().SetBool("isDying", true);
                daywalker.Play();
                mainCamera.GetComponent<GameStateManager>().DayWalkerDown();
            }
        }
        else if (other.gameObject.tag == "Shrine")
        {
            other.gameObject.GetComponent<Animator>().SetBool("isDesecrated", true);
            if (other.gameObject.GetComponent<shrineCheatBuster>().Desecrate())
            {
                mainCamera.GetComponent<GameStateManager>().ShrineDesecrated();
                shrine.Play();
            }
        }
        else if(other.gameObject.tag == "TransitionTrigger")
        {
            mainCamera.transform.position = new Vector3(other.gameObject.transform.parent.transform.position.x, 0, -10);
            mainCamera.GetComponent<GameStateManager>().spawnNextLevel();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
        if (collision.collider.gameObject.tag == "Ground")
        {
            grounded = false;
            anim.SetBool("isGrounded", false);
        }
        */
        wallhugging = false;
        anim.SetBool("isWallHugging", false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "DayWalker") GameObject.Destroy(other.gameObject);
    }
}
