using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    private bool looted = false;

    

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




    protected void Awake() {

        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 0.3f;
        ChangeState(new IdleState());
    }

    protected override void Update() {

        if (IsAlive) {

            if (!IsAttacking) {

                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();
                        
        }

        base.Update();


    }


    public override Transform Select() {

        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect() {

        healthGroup.alpha = 0;

        base.DeSelect();

    }

    public override void TakeDamage(float damage, Transform source) {

        if (!(currentState is EvadeState)) {

            SetTarget(source);

            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);

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

    public override void Interact() {

        // loot enemy

        if (!IsAlive && !looted) {

            lootTable.RollLoot();
            looted = true;

        }
        
    }

}
