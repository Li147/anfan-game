using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private CanvasGroup healthGroup;

    private IState currentState;


    // test
    private bool flashActive;
    [SerializeField]
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer enemySprite;
    //




    [SerializeField]
    private LootTable lootTable;

    private bool looted = false;

    [SerializeField]
    private string enemyName;

    public float MyAttackRange { get; set; }
    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }


    public float MyAggroRange { get; set; }

    [SerializeField]
    private float initAggroRange;

    public bool InRange {
        get {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    public string MyName { get => enemyName; set => enemyName = value; }

    protected void Awake() {

        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 0.4f;
        ChangeState(new IdleState());

        

    }

    protected override void Start()
    {
        base.Start();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    protected override void Update() {

        if (IsAlive) {

            if (!IsAttacking) {

                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();
                        
        }

        Flash();

        base.Update();


    }


    public Transform Select() {

        healthGroup.alpha = 1;

        return hitBox;
    }

    public void DeSelect() {

        healthGroup.alpha = 0;
        // Good practice to always include unsubscribe code in your thing

        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);

    }

    

    public override void TakeDamage(float damage, Transform source) {

        if (!(currentState is EvadeState) && IsAlive) {

            SetTarget(source);

            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);
            flashActive = true;
            flashCounter = flashLength;

        }
      
    }

    

    public void ChangeState(IState newState) {

        if (currentState != null) {

            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);

    }

    public void SetTarget(Transform target) {

        if (MyTarget == null && !(currentState is EvadeState)) {

            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;

        }


    }


    public void Reset() {

        // resets enemy target
        this.MyTarget = null;

        // resets aggro range
        this.MyAggroRange = initAggroRange;

        // resets enemies health stat
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;

        // resets the UI enemy health bar
        OnHealthChanged(health.MyCurrentValue);

    }

    public void Interact() {

        // loot enemy

        if (!IsAlive && !looted) {

            lootTable.RollLoot();
            looted = true;
            Destroy(this.gameObject);

        }
    }

    public void StopInteract() {



    }


    public void OnHealthChanged(float health)
    {

        if (healthChanged != null)
        {

            healthChanged(health);

        }

    }

    public void OnCharacterRemoved()
    {

        if (characterRemoved != null)
        {
            characterRemoved();
        }
        Destroy(gameObject);

    }

    public void Flash()
    {
        if (flashActive)
        {
            if (flashCounter > flashLength * .99f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .82f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .66f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .49f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCounter > flashLength * .33f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCounter > flashLength * .16f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCounter > 0f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
                flashActive = false;
            }
            flashCounter -= Time.deltaTime;
        }
    }

}
