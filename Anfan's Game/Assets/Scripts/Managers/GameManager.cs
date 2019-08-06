using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This class is responsible for selecting a player's target
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private NPC currentTarget;
    

    // Update is called once per frame
    void Update()
    {
        ClickTarget();

        //Debug.Log(LayerMask.GetMask("Clickable"));
    }

    private void ClickTarget() {

        // deals with clicks using the LEFT MOUSE BUTTON
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
                        
            if (hit.collider != null) { // if my mouse clicks something

                if (currentTarget!= null) {

                    currentTarget.DeSelect();

                }

                currentTarget = hit.collider.GetComponent<NPC>();

                // BUG - clicking the mini hitbox causes an error -> fix!!!
                player.MyTarget = currentTarget.Select();




                UIManager.MyInstance.ShowTargetFrame(currentTarget);


                
              
            } else { // if my mouse clicks nothing

                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null) {

                    currentTarget.DeSelect();
                }

                currentTarget = null;
                player.MyTarget = null;
            }
        }
        // deals with clicks on the RIGHT MOUSE BUTTON
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            
            // if our mouse clicks on an enemy...
            if (hit.collider != null && (hit.collider.tag == "enemy" || hit.collider.tag == "interactable")) {

                player.Interact();

            }

        }
        
    }


}
