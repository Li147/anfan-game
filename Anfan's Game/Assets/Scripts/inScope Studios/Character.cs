using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all characters in game, including Player and Enemies
public abstract class Character : MonoBehaviour
{

  
    [SerializeField]
    protected Vector2 movementDirection;

    [SerializeField]
    private float movementSpeed;

    [Header("References:")]
    public Animator animator;
    public Rigidbody2D rb;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    void Move() {
        rb.velocity = movementDirection * movementSpeed * Time.deltaTime;
        Animate(movementDirection);
        
    }

    public void Animate(Vector2 direction) {
        animator.SetFloat("x", movementDirection.x);
        animator.SetFloat("y", movementDirection.y);

       

    }

}
