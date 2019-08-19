using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
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

    
    //===============PLAYER STATISTICS====================//
    [SerializeField]
    private Stat hunger;
    private float initHunger = 200;

    [SerializeField]
    private Stat mana;
    private float initMana = 100;

    [SerializeField]
    private float manaRegenRate;

    [SerializeField]
    private Stat exp;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private int baseDamage;

    [SerializeField]
    private int baseDefence;

    private int bonusDamage = 0;

    private int bonusDefence = 0;

    private float bonusSpeed = 0;


    //===================================================//




    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    [SerializeField]
    private Transform[] hitBoxes;

    private int hitBoxIndex = 2;
        
    private Vector3 min, max;

    [SerializeField]
    private GearSocket[] gearSockets;

    [SerializeField]
    private GameObject bow;

    private Coroutine initRoutine;

    [SerializeField]
    private ParticleSystem particleSystem;

    [SerializeField]
    private Transform minimapIndicator;

    [SerializeField]
    private Crafting crafting;
    
      
    private GameObject currentObject = null;

    [SerializeField]
    public Tilemap tileMap;

    [SerializeField]
    private CanvasGroup damageEffect;

   
    // references the things he can interact with e.g. enemies, chests, trees
    private List<IInteractable> interactables = new List<IInteractable>();

    #region PATHFINDING

    [SerializeField]
    private AStarAlgorithm astar;

    private Stack<Vector3> path;
    private Vector3 destination;
    private Vector3 goal;
    private Vector3 current;

    #endregion


    private List<Enemy> attackers = new List<Enemy>();

    public Stat MyExp { get => exp; set => exp = value; }
    public Stat MyMana { get => mana; set => mana = value; }
    public Stat MyHunger { get => hunger; set => hunger = value; }
    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }
    public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }
    public Coroutine MyInitRoutine { get => initRoutine; set => initRoutine = value; }
    public GameObject MyBow { get => bow; set => bow = value; }
    public float MyManaRegenRate { get => manaRegenRate; set => manaRegenRate = value; }
    public int MyBonusDamage { get => bonusDamage; set => bonusDamage = value; }
    public int MyBonusDefence { get => bonusDefence; set => bonusDefence = value; }
    public float MyBonusSpeed { get => bonusSpeed; set => bonusSpeed = value; }
    public int MyBaseDamage { get => baseDamage; set => baseDamage = value; }
    public int MyBaseDefence { get => baseDefence; set => baseDefence = value; }

    protected override void Update()
    {
        ProcessInputs();
        ClickToMove();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
                                         Mathf.Clamp(transform.position.y, min.y, max.y), 
                                         transform.position.z);

        // sets bow active to true or false
        if (CharacterPanel.MyInstance.MyMain.MyEquippedArmour != null)
        {
            if (CharacterPanel.MyInstance.MyMain.MyEquippedArmour.MyName == "Bow")
            {
                MyBow.SetActive(true);

            }
            else
            {
                MyBow.SetActive(false);
            }
        }
        


        base.Update();
    }

    //code relating to adjusting physics is here
    protected void FixedUpdate()
    {
        Move();
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

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"]))
        {
            exitIndex = 0;
            hitBoxIndex = 0;
            minimapIndicator.eulerAngles = new Vector3(0, 0, 0);
            MovementDirection += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            hitBoxIndex = 3;
            
            MovementDirection += Vector2.left;

            if (MovementDirection.y == 0)
            {
                minimapIndicator.eulerAngles = new Vector3(0, 0, 90);
            }

        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            hitBoxIndex = 2;
            MovementDirection += Vector2.down;

            minimapIndicator.eulerAngles = new Vector3(0, 0, 180);

        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"]))
        {
            exitIndex = 1;
            hitBoxIndex = 1;
            MovementDirection += Vector2.right;

            if (MovementDirection.y == 0)
            {
                minimapIndicator.eulerAngles = new Vector3(0, 0, 270);
            }

        }

                
        if (Input.GetKeyDown(KeyCode.Space)) {

            if (!IsAttacking && !IsMoving) {

                actionRoutine = StartCoroutine(Attack());
                

            }

        }

        // Cancel spells if player moves
        if (IsMoving) {
            StopAttack();
            StopAction();
            StopInit();
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

      

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(hitBoxes[hitBoxIndex].position, attackRange, whatIsEnemies);

        Vector2 direction = new Vector2(attackRange, 0);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].tag == "enemy")
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(MyBaseDamage + MyBonusDamage, transform);
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




    private IEnumerator SpellRoutine(ICastable castable) {

        Transform currentTarget = MyTarget;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if (currentTarget != null) {

            Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, transform, newSpell.MyDamage, newSpell.MySpeed);
            Player.MyInstance.MyMana.MyCurrentValue -= newSpell.MyManaCost;

        }

        StopAction();

    }



   
    private IEnumerator GatherRoutine(ICastable castable, GatheringLootTable lootTable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        Loot[] possibleLoot = lootTable.possibleLoot;
        int[] quantity = lootTable.quantity;


        yield return new WaitForSeconds(castable.MyCastTime);

        for (int i = 0; i < possibleLoot.Length; i++)
        {
            int itemIndex = possibleLoot[i].MyItem.MyItemIndex;
            int itemQuantity = quantity[i];
            ItemSpawnManager.MyInstance.SpawnEntities(itemIndex, itemQuantity);
        }

        lootTable.isEmpty = true;



        StopAction();
        
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        crafting.AddItemsToInventory();
    }



    public void CastSpell(ICastable castable)
    {
        //Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isSpellcasting && !IsMoving)
        {

            MyInitRoutine = StartCoroutine(SpellRoutine(castable));
        }

    }


    
    public void Gather(ICastable castable, GatheringLootTable lootTable)
    {
        if (!IsAttacking && !isSpellcasting)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, lootTable));
        }
    }




    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.MyInstance.Cast(castable);

        isSpellcasting = true; // Changes our state to spellcasting

        MyAnimator.SetBool("spellcast", isSpellcasting); // Starts spellcast animation

        foreach (GearSocket g in gearSockets)
        {

            g.MyAnimator.SetBool("spellcast", isSpellcasting);

        }

        yield return new WaitForSeconds(castable.MyCastTime);

        StopAction();


    }



    

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


    public void StopAction() {

        SpellBook.MyInstance.StopCasting();

        isSpellcasting = false;

        MyAnimator.SetBool("spellcast", isSpellcasting);

        foreach (GearSocket g in gearSockets)
        {

            g.MyAnimator.SetBool("spellcast", isSpellcasting);

        }

        if (actionRoutine != null) {
            StopCoroutine(actionRoutine);
            
        }

    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
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

        // add a little extra offset for EXP SCT
        Vector3 pos = transform.position;
        pos.y += 0.7f;

        CombatTextManager.MyInstance.CreateText(pos, xp.ToString(), SCTTYPE.EXP, false);
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
        MyHealth.MyMaxValue += 1;
        MyHunger.MyMaxValue += 2;
        MyMana.MyMaxValue += 2;
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

    public void GetPath(Vector3 goal)
    {
        path = astar.Algorithm(transform.position, goal);
        current = path.Pop();
        destination = path.Pop();
        this.goal = goal;
    }

    private void ClickToMove()
    {
        if (path != null)
        {
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, MyBaseMovementSpeed * Time.deltaTime);

            Vector3Int dest = astar.MyTilemap.WorldToCell(destination);
            Vector3Int cur = astar.MyTilemap.WorldToCell(current);

            Debug.Log("ClickToMove: destination Vector3: " + destination.ToString());

            float distance = Vector2.Distance(destination, transform.parent.position);

            if(cur.y > dest.y)
            {
                MovementDirection = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                MovementDirection = Vector2.up;
            }
            if (cur.y == dest.y)
            {
                if (cur.x > dest.x)
                {
                    MovementDirection = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    MovementDirection = Vector2.right;
                }
            }



            if (distance <= 0f)
            {
                if(path.Count > 0)
                {
                    current = destination;
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                }
            }
        }
    }




    public void Move()
    {
        if (path == null)
        {
            if (IsAlive)
            {
                MyRigidBody.velocity = MovementDirection * (MyBaseMovementSpeed + bonusSpeed);
            }
        }
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


    public void GainHunger(int hunger)
    {
        MyHunger.MyCurrentValue += hunger;
        
        CombatTextManager.MyInstance.CreateText(transform.position, hunger.ToString(), SCTTYPE.HUNGER, true);
    }

    public override void TakeDamage(float damage, Transform source)
    {
        // update health value
        health.MyCurrentValue -= damage;

        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);

        // if death, velocity is set to zero so character can't move
        if (health.MyCurrentValue <= 0)
        {

            MyRigidBody.velocity = Vector2.zero;

            MyAnimator.SetTrigger("die");

            foreach (GearSocket g in gearSockets)
            {
                g.MyAnimator.SetTrigger("die");
            }


        }

        StartCoroutine(ScreenRedFlash());

        if (!IsAlive)
        {
            UIManager.MyInstance.ShowGameOver();
        }

    }



    private IEnumerator ScreenRedFlash()
    {
        damageEffect.alpha = 1;
        yield return new WaitForSeconds(0.2f);
        damageEffect.alpha = 0;
    }



}
