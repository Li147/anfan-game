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

    protected Animator animator;
    private Rigidbody2D rb;
    private int length;

    protected bool isAttacking = false;
    protected bool isSpellcasting = false;

    protected Coroutine attackRoutine;


    public bool IsMoving {
        get {
            return movementDirection.x != 0 || movementDirection.y != 0;
        }
    }




    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update() {
        HandleLayers();
    }

    protected virtual void FixedUpdate() {
        Move();
    }



    public void Move() {
        rb.velocity = movementDirection * movementSpeed;
           
    }


    public void HandleLayers() {

        if (IsMoving) {
            ActivateLayer("WalkLayer");


            // sets animation parameter  to ensure player faces correct direction
            animator.SetFloat("x", movementDirection.x);
            animator.SetFloat("y", movementDirection.y);



        } else if (isAttacking) {

            ActivateLayer("AttackLayer");

                       

        } else if (isSpellcasting) {

            ActivateLayer("SpellcastLayer");


        } else {
            ActivateLayer("IdleLayer");
        }
    }



    public void ActivateLayer(string layerName) {
        for (int i = 0; i < animator.layerCount; i++) {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
        
    }

    public void StopAttack() {

        if (attackRoutine != null) {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            animator.SetBool("attack", isAttacking);

        }
        
        
    }

}
