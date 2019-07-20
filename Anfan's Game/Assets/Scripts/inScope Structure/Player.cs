using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat hunger;

    private float initHealth = 100;
    private float initHunger = 200;

    [SerializeField]
    private GameObject[] spellPrefab;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;


    private GameObject currentObject = null;

   
    // references the things he can interact with e.g. enemies, chests, trees
    private IInteractable interactable;
    
 
    
    protected override void Start() {

        health.Initialize(initHealth, initHealth);
        hunger.Initialize(initHunger, initHunger);
        
        base.Start();
    }

    protected override void Update() {
        

        ProcessInputs();


        base.Update();
        

    }
    
    // code relating to adjusting physics is here
    protected override void FixedUpdate() {

        base.FixedUpdate();

    }

    private void ProcessInputs() {

        movementDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            exitIndex = 0;
            movementDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)) {
            exitIndex = 3;
            movementDirection += Vector2.left;

        }
        if (Input.GetKey(KeyCode.S)) {
            exitIndex = 2;
            movementDirection += Vector2.down;

        }
        if (Input.GetKey(KeyCode.D)) {
            exitIndex = 1;
            movementDirection += Vector2.right;

        }







        //float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //float moveVertical = Input.GetAxisRaw("Vertical");

        //movementDirection =  new Vector2(moveHorizontal, moveVertical);
        
        
        //movementDirection.Normalize();



        if (Input.GetKeyDown(KeyCode.Space)) {

            if (!isAttacking && !IsMoving) {

                attackRoutine = StartCoroutine(Attack());

            }
            
        }

        if (Input.GetKeyDown(KeyCode.M)) {

            if (!isSpellcasting && !IsMoving) {

                spellRoutine = StartCoroutine(Spellcast());

            }
           
        }

        if (Input.GetKeyDown(KeyCode.B)) {

            if (!isShooting && !IsMoving) {

                bowRoutine = StartCoroutine(Shoot());

            }

        }


        //Debugging

        if (Input.GetKeyDown(KeyCode.O)) {
            health.MyCurrentValue -= 10;
            hunger.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            health.MyCurrentValue += 10;
            hunger.MyCurrentValue += 10;

        }

        


    }


    private IEnumerator Attack() {

        
        isAttacking = true;
        animator.SetBool("attack", isAttacking);
        
        yield return new WaitForSeconds(1);

        StopAttack();




    }

    private IEnumerator Spellcast() {

        isSpellcasting = true;
        animator.SetBool("spellcast", isSpellcasting);
        

        yield return new WaitForSeconds(1);
        CastSpell();

        StopSpell();

               
    }

    public void CastSpell() {

        Instantiate(spellPrefab[1], exitPoints[exitIndex].position, Quaternion.identity);

    }

    public void ShootArrow() {

        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);

    }


    private IEnumerator Shoot() {


        isShooting = true;
        animator.SetBool("bow", isShooting);

        yield return new WaitForSeconds(1);
        ShootArrow();

        StopShoot();




    }




















    public void Interact() {
        if(interactable != null) {
            interactable.Interact();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            if (interactable!= null) {
                interactable.StopInteract();
                interactable = null;
            }
            
        }
    }





}
