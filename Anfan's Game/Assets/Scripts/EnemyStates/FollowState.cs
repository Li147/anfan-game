using UnityEngine;
using System.Collections;

public class FollowState : IState {

    private Enemy parent;

    public void Enter(Enemy parent) {

        Player.MyInstance.AddAttacker(parent);
        this.parent = parent;

    }

    public void Exit() {

        parent.MovementDirection = Vector2.zero;
        
    }

    public void Update() {

        // If parent has a target
        if (parent.MyTarget != null) {

            parent.MovementDirection = (parent.MyTarget.transform.position - parent.transform.position).normalized;

            parent.transform.position = 
            Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.MyBaseMovementSpeed * Time.deltaTime);

            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            if (distance <= parent.MyAttackRange) {

                parent.ChangeState(new AttackState());

            }
        }

        // If parent is no longer in range of target
        if (!parent.InRange) {

            parent.ChangeState(new EvadeState());


        }     

    }
}
