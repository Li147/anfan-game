using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all characters in game, including Player and Enemies

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;

    private Vector2 movementDirection;


    public Animator MyAnimator { get; set; }


    //public Animator childAnimator;

    private Rigidbody2D rb;
    private int length;

  
    public bool IsAttacking { get; set; }

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


    public Transform MyTarget { get; set; }









    public bool IsMoving {
        get {
            return MovementDirection.x != 0 || MovementDirection.y != 0;
        }
    }


    public bool IsAlive {
        get {
            return health.MyCurrentValue > 0;
        }
    }


    public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }




    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);


        MyAnimator = GetComponent<Animator>();
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

        if (IsAlive) {
            rb.velocity = MovementDirection * MovementSpeed;
        }
        
           
    }


    public void HandleLayers() {

        // If the character is ALIVE
        if (IsAlive) {

            if (IsMoving) {
                ActivateLayer(MyAnimator, "WalkLayer");
                //ActivateLayer(childAnimator, "WalkLayer");

                // sets animation parameter  to ensure player faces correct direction
                //childAnimator.SetFloat("x", movementDirection.x);
                //childAnimator.SetFloat("y", movementDirection.y);


                // sets animation parameter  to ensure player faces correct direction
                MyAnimator.SetFloat("x", MovementDirection.x);
                MyAnimator.SetFloat("y", MovementDirection.y);




            } else if (IsAttacking) {

                ActivateLayer(MyAnimator, "AttackLayer");
                //ActivateLayer(childAnimator, "AttackLayer");



            } else if (isSpellcasting) {

                ActivateLayer(MyAnimator, "SpellcastLayer");
                //ActivateLayer(childAnimator, "SpellcastLayer");


            } else if (isShooting) {

                ActivateLayer(MyAnimator, "BowLayer");
                // ActivateLayer(childAnimator, "BowLayer");

            } else {


                ActivateLayer(MyAnimator, "IdleLayer");
                //ActivateLayer(childAnimator, "IdleLayer");
            }



        } else { //if the character is DEAD

            ActivateLayer(MyAnimator, "DeathLayer");

        }

        
    }



    public void ActivateLayer(Animator animator, string layerName) {
        for (int i = 0; i < animator.layerCount; i++) {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
        
    }

    

    public virtual void TakeDamage(float damage, Transform source) {

        // update health value
        health.MyCurrentValue -= damage;
                       
        // if death, velocity is set to zero so character can't move
        if (health.MyCurrentValue <= 0) {

            rb.velocity = Vector2.zero;

            MyAnimator.SetTrigger("die");
        }

    }



}
