using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all characters in game, including Player and Enemies
public abstract class Character : MonoBehaviour
{

  
    [SerializeField]
    protected Vector2 movementDirection;

    [SerializeField]
    public float movementSpeed;

    private Animator animator;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update() {
        Animate(movementDirection);
    }

    protected virtual void FixedUpdate() {
        Move();
    }



    public void Move() {
        rb.velocity = movementDirection * movementSpeed;
        
        
    }



    public void Animate(Vector2 direction) {
        animator.SetFloat("x", movementDirection.x);
        animator.SetFloat("y", movementDirection.y);

       

    }

}
