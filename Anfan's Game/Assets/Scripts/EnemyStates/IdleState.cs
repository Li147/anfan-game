using UnityEngine;
using UnityEditor;

class IdleState : IState {

    private Enemy parent;

    public void Enter(Enemy parent) {

        this.parent = parent;
        this.parent.Reset();
     
    }

    public void Exit() {
     
    }

    public void Update() {

        // If we have a target...
        if (parent.MyTarget != null) {

            // Change state to follow state
            parent.ChangeState(new FollowState());

        }
    }
}