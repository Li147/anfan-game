using UnityEngine;
using System.Collections;

public class FollowState : IState {

    private Enemy parent;

    public void Enter(Enemy parent) {

        this.parent = parent;

    }

    public void Exit() {

        parent.MovementDirection = Vector2.zero;
        
    }

    public void Update() {

        // If parent has a target
        if (parent.Target != null) {

            parent.MovementDirection = (parent.Target.transform.position - parent.transform.position).normalized;

            parent.transform.position = 
            Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.MovementSpeed * Time.deltaTime);

            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);

            if (distance <= parent.MyAttackRange) {

                parent.ChangeState(new AttackState());

            }


        } else { // otherwise change to idle state

            parent.ChangeState(new IdleState());

        }


        

    }
}
