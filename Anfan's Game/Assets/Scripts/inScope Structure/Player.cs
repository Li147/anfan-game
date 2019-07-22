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
    private Block[] blocks;


    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    

    public Transform MyTarget { get; set; }




    private GameObject currentObject = null;

   
    // references the things he can interact with e.g. enemies, chests, trees
    private IInteractable interactable;
    
 
    
    protected override void Start() {

        health.Initialize(initHealth, initHealth);
        hunger.Initialize(initHunger, initHunger);

        MyTarget = GameObject.Find("Target").transform;

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






        if (Input.GetKeyDown(KeyCode.Space)) {

            if (!isAttacking && !IsMoving) {

                attackRoutine = StartCoroutine(Attack());

            }
            
        }

        

        if (Input.GetKeyDown(KeyCode.B)) {

            Block();

            if (MyTarget!= null && !isShooting && !IsMoving && InLineOfSight()) {

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

    private IEnumerator Spellcast(int spellIndex) {

        isSpellcasting = true;
        animator.SetBool("spellcast", isSpellcasting);
        

        yield return new WaitForSeconds(1);
        
        Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity);

        StopSpell();

               
    }

    public void CastSpell(int spellIndex) {

        Block();

        if (MyTarget != null && !isSpellcasting && !IsMoving && InLineOfSight()) {

            spellRoutine = StartCoroutine(Spellcast(spellIndex));

        }

        

    }

    private IEnumerator Shoot() {


        isShooting = true;
        animator.SetBool("bow", isShooting);

        yield return new WaitForSeconds(1);

        Instantiate(spellPrefab[0], transform.position, Quaternion.identity);

        StopShoot();

    }


    


    private bool InLineOfSight() {

        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);


        if (hit.collider == null) {
            return true;
        }

        return false;
    }

    private void Block() {
        foreach (Block b in blocks) {
            b.Deactivate();

        }

        blocks[exitIndex].Activate();
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
