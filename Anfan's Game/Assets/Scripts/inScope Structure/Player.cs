using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    [SerializeField]
    private Stat hunger;

    
    private float initHunger = 200;

   
    [SerializeField]
    private Block[] blocks;


    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    private SpellBook spellBook;

    [SerializeField]
    private GameObject arrow;

    

    public Transform MyTarget { get; set; }

    private Vector3 min, max;












    private GameObject currentObject = null;

   
    // references the things he can interact with e.g. enemies, chests, trees
    private IInteractable interactable;
    
 
    
    protected override void Start() {

        spellBook = GetComponent<SpellBook>();

 
        hunger.Initialize(initHunger, initHunger);

        base.Start();
    }

    protected override void Update() {
        

        ProcessInputs();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
                                         Mathf.Clamp(transform.position.y, min.y, max.y), 
                                         transform.position.z);


        base.Update();
        

    }
    
    // code relating to adjusting physics is here
    protected override void FixedUpdate() {

        base.FixedUpdate();

    }

    private void ProcessInputs() {

        MovementDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            exitIndex = 0;
            MovementDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)) {
            exitIndex = 3;
            MovementDirection += Vector2.left;

        }
        if (Input.GetKey(KeyCode.S)) {
            exitIndex = 2;
            MovementDirection += Vector2.down;

        }
        if (Input.GetKey(KeyCode.D)) {
            exitIndex = 1;
            MovementDirection += Vector2.right;

        }






        if (Input.GetKeyDown(KeyCode.Space)) {

            if (!IsAttacking && !IsMoving) {

                attackRoutine = StartCoroutine(Attack());

            }
            
        }

        

        if (Input.GetKeyDown(KeyCode.B)) {

            Block();

            if (MyTarget!= null && !isShooting && !IsMoving && InLineOfSight()) {

                bowRoutine = StartCoroutine(Shoot());

            }

        }

        // Cancel spells if player moves
        if (IsMoving) {
            StopAttack();
            StopSpell();
            StopShoot();
        }
        





        //Debugging HP bar

        if (Input.GetKeyDown(KeyCode.O)) {
            health.MyCurrentValue -= 10;
            hunger.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            health.MyCurrentValue += 10;
            hunger.MyCurrentValue += 10;

        }
             
        
    }

    public void SetLimits(Vector3 min, Vector3 max) {

        this.min = min;
        this.max = max;
        
    }





    private IEnumerator Attack() {

        
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking);
        
        yield return new WaitForSeconds(1);

        StopAttack();

    }

    private IEnumerator Spellcast(int spellIndex) {

        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.FindSpell(spellIndex);

        isSpellcasting = true; // Changes our state to spellcasting

        MyAnimator.SetBool("spellcast", isSpellcasting); // Starts spellcast animation
        

        yield return new WaitForSeconds(newSpell.MyCastTime);

        if (currentTarget != null && InLineOfSight()) {

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, newSpell.MyDamage);

        }

              

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
        MyAnimator.SetBool("bow", isShooting);

        yield return new WaitForSeconds(1);

        Instantiate(arrow, transform.position, Quaternion.identity);

        StopShoot();

    }


    

    // Checks if target is line of sight of player model
    private bool InLineOfSight() {

        // check if we have a target
        if (MyTarget != null) {

            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            // throw a raycast in the direction of the target
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);


            if (hit.collider == null) {
                return true;
            }

        }

        
        // if we hit a line of sight block we are not allowed to cast spell
        return false;
    }

    private void Block() {
        foreach (Block b in blocks) {
            b.Deactivate();

        }

        blocks[exitIndex].Activate();
    }

    

    public void StopAttack() {

        if (attackRoutine != null) {
            StopCoroutine(attackRoutine);
            IsAttacking = false;
            MyAnimator.SetBool("attack", IsAttacking);

        }

    }

    public void StopSpell() {

        spellBook.StopCasting();

        if (spellRoutine != null) {
            StopCoroutine(spellRoutine);
            isSpellcasting = false;
            MyAnimator.SetBool("spellcast", isSpellcasting);
        }

    }

    public void StopShoot() {

        if (bowRoutine != null) {
            StopCoroutine(bowRoutine);
            isShooting = false;
            MyAnimator.SetBool("bow", isShooting);
        }

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
