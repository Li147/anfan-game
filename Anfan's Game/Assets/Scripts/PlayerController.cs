using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [Header("Character attributes:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;



    [Header("Character statistics:")]
    public Vector2 movementDirection;
    public float movementSpeed;

    [Header("References:")]
    public Animator animator;
    public Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();

    }

    void Update() {
        ProcessInputs();
        Move();
        Animate();
    }

    void ProcessInputs() {
       

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movementDirection =  new Vector2(moveHorizontal, moveVertical);
        
        movementDirection.Normalize();

    }

    private void Move() {
        rb.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
    }

    void Animate() {
        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Vertical", movementDirection.y);
    }



    // code relating to adjusting physics is here
    void FixedUpdate() {

       


    }
}
