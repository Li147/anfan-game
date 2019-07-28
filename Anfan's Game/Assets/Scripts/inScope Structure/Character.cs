using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all characters in game, including Player and Enemies

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{

  
    [SerializeField]
    protected Vector2 movementDirection;

    [SerializeField]
    public float movementSpeed;

    protected Animator animator;
    //public Animator childAnimator;

    private Rigidbody2D rb;
    private int length;

    protected bool isAttacking = false;
    protected bool isSpellcasting = false;
    protected bool isShooting = false;

    protected Coroutine attackRoutine;
    protected Coroutine spellRoutine;
    protected Coroutine bowRoutine;

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Stat MyHealth {
        get { return health; }
    }



    [SerializeField]
    private float initHealth = 100;




    public bool IsMoving {
        get {
            return movementDirection.x != 0 || movementDirection.y != 0;
        }
    }




    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);


        animator = GetComponent<Animator>();
        //childAnimator = GetComponentsInChildren<Animator>()[1];
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
            ActivateLayer(animator, "WalkLayer");
            //ActivateLayer(childAnimator, "WalkLayer");

            // sets animation parameter  to ensure player faces correct direction
            //childAnimator.SetFloat("x", movementDirection.x);
            //childAnimator.SetFloat("y", movementDirection.y);


            // sets animation parameter  to ensure player faces correct direction
            animator.SetFloat("x", movementDirection.x);
            animator.SetFloat("y", movementDirection.y);

            StopAttack();
            StopSpell();
            StopShoot();



        } else if (isAttacking) {

            ActivateLayer(animator, "AttackLayer");
            //ActivateLayer(childAnimator, "AttackLayer");



        } else if (isSpellcasting) {

            ActivateLayer(animator, "SpellcastLayer");
            //ActivateLayer(childAnimator, "SpellcastLayer");


        } else if (isShooting) {

            ActivateLayer(animator, "BowLayer");
           // ActivateLayer(childAnimator, "BowLayer");

        } else {

        
            ActivateLayer(animator, "IdleLayer");
            //ActivateLayer(childAnimator, "IdleLayer");
        }
    }



    public void ActivateLayer(Animator animator, string layerName) {
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

    public virtual void StopSpell() {

        if (spellRoutine != null) {
            StopCoroutine(spellRoutine);
            isSpellcasting = false;
            animator.SetBool("spellcast", isSpellcasting);
        }
        
    }

    public void StopShoot() {

        if (bowRoutine != null) {
            StopCoroutine(bowRoutine);
            isShooting = false;
            animator.SetBool("bow", isShooting);
        }

    }

    public virtual void TakeDamage(float damage) {

        health.MyCurrentValue -= damage;

       
        if (health.MyCurrentValue <= 0) {

            animator.SetTrigger("die");
        }

    }



}
