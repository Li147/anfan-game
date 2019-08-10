﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    // This makes the player a singleton
    private static Player instance;

    public static Player MyInstance {

        get {
            if (instance == null) {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
                
    }

    private List<Enemy> attackers = new List<Enemy>();

    public Stat MyExp { get => exp; set => exp = value; }
    public Stat MyMana { get => mana; set => mana = value; }
    public Stat MyHunger { get => hunger; set => hunger = value; }
    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }
    public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }

    // player's hunger stat
    [SerializeField]
    private Stat hunger;

    private float initHunger = 200;

    // player's mana stat
    [SerializeField]
    private Stat mana;

    private float initMana = 100;

    [SerializeField]
    private Stat exp;

    [SerializeField]
    private Text levelText;


    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    [SerializeField]
    private Transform[] hitBoxes;

    private int hitBoxIndex = 2;
        
    [SerializeField]
    private GameObject arrow;
 

    private Vector3 min, max;

    [SerializeField]
    private GearSocket[] gearSockets;

    [SerializeField]
    private ParticleSystem particleSystem;
    
      
    private GameObject currentObject = null;

   
    // references the things he can interact with e.g. enemies, chests, trees
    private List<IInteractable> interactables = new List<IInteractable>();
    
 
    
   

    protected override void Update()
    {
        ProcessInputs();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
                                         Mathf.Clamp(transform.position.y, min.y, max.y), 
                                         transform.position.z);

        base.Update();
    }
    
    // code relating to adjusting physics is here
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    public void SetDefaultValues()
    {
        health.Initialize(initHealth, initHealth);
        MyHunger.Initialize(initHunger, initHunger);
        MyMana.Initialize(initMana, initMana);
        MyExp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = "Level" + MyLevel.ToString();
    }



    private void ProcessInputs() {

        MovementDirection = Vector2.zero;

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"])) {  // move up
            exitIndex = 0;
            hitBoxIndex = 0;
            MovementDirection += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"])) {
            exitIndex = 3;
            hitBoxIndex = 3;
            MovementDirection += Vector2.left;

        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"])) {
            exitIndex = 2;
            hitBoxIndex = 2;
            MovementDirection += Vector2.down;

        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"])) {
            exitIndex = 1;
            hitBoxIndex = 1;
            MovementDirection += Vector2.right;

        }

                
        
        if (Input.GetKeyDown(KeyCode.Space)) {

            if (!IsAttacking && !IsMoving) {

                actionRoutine = StartCoroutine(Attack());
                

            }

        }

        

        if (Input.GetKeyDown(KeyCode.B)) {

            

            if (MyTarget!= null && !isShooting && !IsMoving) {

                bowRoutine = StartCoroutine(Shoot());

            }

        }

        // Cancel spells if player moves
        if (IsMoving) {
            StopAttack();
            StopSpell();
            StopShoot();
        }


        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys) {

            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action])) {

                UIManager.MyInstance.ClickActionButton(action);

            }

        }
        


        

        //Debugging stats bars

        if (Input.GetKeyDown(KeyCode.O)) {
            health.MyCurrentValue -= 10;
            MyHunger.MyCurrentValue -= 10;
            MyMana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            health.MyCurrentValue += 10;
            MyHunger.MyCurrentValue += 10;
            MyMana.MyCurrentValue += 10;

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(100);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {


            particleSystem.Play();

        }


    }


    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }



    
    public float attackRange;
    public LayerMask whatIsEnemies;

    private IEnumerator Attack() {

        
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking);

        foreach (GearSocket g in gearSockets)
        {

            g.MyAnimator.SetBool("attack", IsAttacking);

        }

        //experimental code

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(hitBoxes[hitBoxIndex].position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].tag == "enemy")
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(10, transform);
            }

        }

        yield return new WaitForSeconds(0.5f);
        StopAttack();

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBoxes[0].position, attackRange);
        Gizmos.DrawWireSphere(hitBoxes[1].position, attackRange);
        Gizmos.DrawWireSphere(hitBoxes[2].position, attackRange);
        Gizmos.DrawWireSphere(hitBoxes[3].position, attackRange);

    }




    private IEnumerator Spellcast(string spellName) {

        Transform currentTarget = MyTarget;

        Spell newSpell = SpellBook.MyInstance.FindSpell(spellName);

        isSpellcasting = true; // Changes our state to spellcasting

        MyAnimator.SetBool("spellcast", isSpellcasting); // Starts spellcast animation

        foreach (GearSocket g in gearSockets) {

            g.MyAnimator.SetBool("spellcast", isSpellcasting);

        }
        

        yield return new WaitForSeconds(newSpell.MyCastTime);

        if (currentTarget != null) {

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, transform, newSpell.MyDamage);

        }

              

        StopSpell();

               
    }

    public void CastSpell(string spellName) {

        //Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isSpellcasting && !IsMoving) {

            spellRoutine = StartCoroutine(Spellcast(spellName));

        }

    }

    public void Gather(string skillName, Loot[] possibleLoot, int[] quantity)
    {
        if (!IsAttacking)
        {
            spellRoutine = StartCoroutine(GatherRoutine(skillName, possibleLoot, quantity));
        }
    }

    private IEnumerator GatherRoutine(string skillName, Loot[] possibleLoot, int[] quantity)
    {
        Transform currentTarget = MyTarget;

        Spell newSpell = SpellBook.MyInstance.FindSpell(skillName);

        isSpellcasting = true; // Changes our state to spellcasting

        MyAnimator.SetBool("spellcast", isSpellcasting); // Starts spellcast animation

        foreach (GearSocket g in gearSockets)
        {

            g.MyAnimator.SetBool("spellcast", isSpellcasting);

        }

        yield return new WaitForSeconds(newSpell.MyCastTime);

        StopSpell();

        for (int i = 0; i < possibleLoot.Length; i++)
        {
            int itemIndex = possibleLoot[i].MyItem.MyItemIndex;
            int itemQuantity = quantity[i];
            ItemSpawnManager.MyInstance.SpawnEntities(itemIndex, itemQuantity);
        }



        
    }



    // =====UNIFNISHED
    private IEnumerator Shoot() {


        isShooting = true;
        MyAnimator.SetBool("bow", isShooting);

        yield return new WaitForSeconds(1);

        Instantiate(arrow, transform.position, Quaternion.identity);

        StopShoot();

    }
    //======= UNFINISHED


    

    //// Checks if target is line of sight of player model
    //private bool InLineOfSight() {

    //    // check if we have a target
    //    if (MyTarget != null) {

    //        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

    //        // throw a raycast in the direction of the target
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);


    //        if (hit.collider == null) {
    //            return true;
    //        }

    //    }

        
    //    // if we hit a line of sight block we are not allowed to cast spell
    //    return false;
    //}

    //private void Block() {
    //    foreach (Block b in blocks) {
    //        b.Deactivate();

    //    }

    //    blocks[exitIndex].Activate();
    //}


    public void StopAttack()
    {

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            IsAttacking = false;
            MyAnimator.SetBool("attack", IsAttacking);

            foreach (GearSocket g in gearSockets)
            {

                g.MyAnimator.SetBool("attack", IsAttacking);

            }

        }

    }


    public void StopSpell() {

        SpellBook.MyInstance.StopCasting();

        if (spellRoutine != null) {
            StopCoroutine(spellRoutine);
            isSpellcasting = false;
            MyAnimator.SetBool("spellcast", isSpellcasting);

            foreach (GearSocket g in gearSockets) {

                g.MyAnimator.SetBool("spellcast", isSpellcasting);

            }


        }

    }

    public void StopShoot() {

        if (bowRoutine != null) {
            StopCoroutine(bowRoutine);
            isShooting = false;
            MyAnimator.SetBool("bow", isShooting);
        }

    }



    // Used for setting parameters to change animations of specific peices of gear
    public override void HandleLayers() {
        base.HandleLayers();

        if (IsMoving) {

            foreach (GearSocket g in gearSockets) {

                g.SetXAndY(MovementDirection.x, MovementDirection.y);

            }

        }

    }

    public override void ActivateLayer(Animator animator, string layerName) {

        base.ActivateLayer(animator, layerName);

        foreach (GearSocket g in gearSockets) {

            g.ActivateLayer(layerName);

        }
    }



    public void GainXP(int xp)
    {
        MyExp.MyCurrentValue += xp;
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), SCTTYPE.EXP, false);
        if (MyExp.MyCurrentValue >= MyExp.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    private IEnumerator LevelUp()
    {
        while(!MyExp.isFull){
            yield return null;
        }

        MyLevel++;
        particleSystem.Play();
        levelText.text = "Level" + MyLevel.ToString();
        MyExp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyExp.MyMaxValue = Mathf.Floor(MyExp.MyMaxValue);
        MyExp.MyCurrentValue = MyExp.MyOverflow;
        MyExp.Reset();

        if (MyExp.MyCurrentValue >= MyExp.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }

    }

    public void UpdateLevel()
    {
        levelText.text = "Level" + MyLevel.ToString();
    }



    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "enemy" || collision.tag == "interactable") 
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();

            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
            
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "enemy" || collision.tag == "interactable")
        {
            // if we have something in our interactables array
            if (MyInteractables.Count > 0)
            {
                // check if we have the interactable we just stopped our collision with
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if (interactable != null)
                {
                    interactable.StopInteract();
                }

                MyInteractables.Remove(interactable);
            }

        }
    }

    // adds enemy to list of enemies that are attacking the player
    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

   





}
