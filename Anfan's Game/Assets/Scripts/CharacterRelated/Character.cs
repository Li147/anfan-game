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

    [SerializeField]
    private int level;


    public Animator MyAnimator { get; set; }

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
    public int MyLevel { get => level; set => level = value; }




    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);


        MyAnimator = GetComponent<Animator>();
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


    public virtual void HandleLayers() {

        // If the character is ALIVE
        if (IsAlive) {

            if (IsMoving) {
                ActivateLayer(MyAnimator, "WalkLayer");
                
                // sets animation parameter  to ensure player faces correct direction
                MyAnimator.SetFloat("x", MovementDirection.x);
                MyAnimator.SetFloat("y", MovementDirection.y);




            } else if (IsAttacking) {

                ActivateLayer(MyAnimator, "AttackLayer");
           
            } else if (isSpellcasting) {

                ActivateLayer(MyAnimator, "SpellcastLayer");
                                
            } else if (isShooting) {

                ActivateLayer(MyAnimator, "BowLayer");

            } else {

                ActivateLayer(MyAnimator, "IdleLayer");
       
            }



        } else { //if the character is DEAD

            ActivateLayer(MyAnimator, "DeathLayer");

        }

        
    }


    // activate a specific animation layer using the string name of the layer
    public virtual void ActivateLayer(Animator animator, string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }

    

    public virtual void TakeDamage(float damage, Transform source) {

        // update health value
        health.MyCurrentValue -= damage;

        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);

        // if death, velocity is set to zero so character can't move
        if (health.MyCurrentValue <= 0) {

            rb.velocity = Vector2.zero;

            MyAnimator.SetTrigger("die");

            if (this is Enemy)
            {
                Player.MyInstance.GainXP(EXPManager.CalculateXP((this as Enemy)));
                
            }
        }

    }

    // use this function any time you want to increase character's health
    public void GainHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTTYPE.HEAL, true);
    }



}
