using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    [SerializeField]
    private Stats health;

    private float initHealth = 100;
 
    // references the things he can interact with e.g. enemies, chests, trees
    private IInteractable interactable;


    private bool playerMoving;
    private Vector2 lastMove;



    protected override void Start() {

        health.Initialize(initHealth, initHealth);
        base.Start();
    }

    protected override void Update() {
        playerMoving = false;

        ProcessInputs();
        

        base.Update();

        
    }

    private void ProcessInputs() {
       

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

       

        movementDirection =  new Vector2(moveHorizontal, moveVertical);
        
        
        movementDirection.Normalize();

        //Debugging

        if (Input.GetKeyDown(KeyCode.O)) {
            health.MyCurrentValue += 10;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            health.MyCurrentValue -= 10;
        }


    }

    

    



    // code relating to adjusting physics is here
    void FixedUpdate() {

       


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
