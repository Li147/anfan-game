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

            StopAttack();
            StopSpell();
            StopShoot();



        } else if (isAttacking) {

            ActivateLayer("AttackLayer");



        } else if (isSpellcasting) {

            ActivateLayer("SpellcastLayer");


        } else if (isShooting) {

            ActivateLayer("BowLayer");
            
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
