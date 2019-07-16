using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat hunger;

    private float initHealth = 100;
    private float initHunger = 200;

    private GameObject currentObject = null;

   
    // references the things he can interact with e.g. enemies, chests, trees
    private IInteractable interactable;
    
 
    private Vector2 lastMove;

    


    protected override void Start() {

        health.Initialize(initHealth, initHealth);
        hunger.Initialize(initHunger, initHunger);
        
        base.Start();
    }

    protected override void Update() {
        

        ProcessInputs();


        base.Update();
        

    }
    
    // code relating to adjusting physics is here
    protected override void FixedUpdate() {

        base.FixedUpdate();

    }

    private void ProcessInputs() {
       

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movementDirection =  new Vector2(moveHorizontal, moveVertical);
        
        
        movementDirection.Normalize();

        //Debugging

        if (Input.GetKeyDown(KeyCode.O)) {
            health.MyCurrentValue -= 10;
            hunger.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            health.MyCurrentValue += 10;
            hunger.MyCurrentValue += 10;

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
