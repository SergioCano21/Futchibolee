using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    private float rotationSpeed = 100f;
    private float targetRotationSpeed = 100f;
    private float rotationVelocity = 0f;
    private float smoothTime = 0.1f;
    private bool isGrounded = false;
    public float jumpForce;
    public Rigidbody2D rb;
    private float zRotation;

    //Move Right = 1; Move Left = -1
    public float i = 1;
    private Vector2 startPos;
    private Quaternion startRot;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rb = gameObject.GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
        if (gm.play)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Jump();
            }
            rotationSpeed = Mathf.SmoothDamp(rotationSpeed, targetRotationSpeed, ref rotationVelocity, smoothTime);
        }
    }
    private void Rotate() {
        if (isGrounded)
        {
            ChangeRotationSpeed();
            zRotation = transform.eulerAngles.z;
            //Going right, change to left
            if (zRotation < 310 && zRotation >= 180 && i == -1)
            {
                i = 1;
            }
            // Going left, change to right
            if (zRotation > 50 && zRotation <= 180 && i == 1)
            {
                i = -1;
            }
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime * i));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, (rotationSpeed / 2) * Time.deltaTime * i));
        }
    }
    private void ChangeRotationSpeed()
    {
        if (transform.eulerAngles.z <= 20 || transform.eulerAngles.z >= 340) {
            targetRotationSpeed = 80f;
        }
        else if ((transform.eulerAngles.z <= 40 && transform.eulerAngles.z > 20) || (transform.eulerAngles.z < 340 && transform.eulerAngles.z >= 320)) {
            targetRotationSpeed = 120f;
        }
        else {
            targetRotationSpeed = 200f;
        }
    }
    private void Jump()
    {
        if (isGrounded == true)
        {
            rb.velocity = Vector2.zero;
            //Normal jump to the direction the head of the player is pointing
            if (transform.eulerAngles.z < 40 || transform.eulerAngles.z > 320) {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            else if ((transform.eulerAngles.z > 40 && transform.eulerAngles.z < 65) || (transform.eulerAngles.z < 320 && transform.eulerAngles.z > 295)) {
                Vector2 jumpDirection = (Vector2.up + new Vector2(transform.up.x, transform.up.y) * 0.7f).normalized;
                rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            }
            else {
                Vector2 jumpDirection = (Vector2.up + new Vector2(transform.up.x, transform.up.y) * 0.3f).normalized;
                rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            }
            isGrounded = false;
        }
    }
    public void ResetPosition()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        i = 1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
