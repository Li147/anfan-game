using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState {

    private Enemy parent;
    private float attackCooldown = 1;

    private float extraRange = 0.1f;

    public void Enter(Enemy parent) {

        this.parent = parent;
       
    }

    public void Exit() {
       
    }

    public void Update() {

        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking) {

            parent.MyAttackTime = 0;
            parent.StartCoroutine(Attack());

        }

        if (parent.Target != null) {

            //We need to check range and attack
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking) {

                parent.ChangeState(new FollowState());

            }

        } else {

            parent.ChangeState(new IdleState());

        }
        
    }

    public IEnumerator Attack() {

        parent.IsAttacking = true;
        parent.MyAnimator.SetTrigger("attack");

        // Waits for however long the animation time is
        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;

    }

}
