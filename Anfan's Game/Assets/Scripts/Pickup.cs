using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pickup : MonoBehaviour{

    // FOR DEBUGGING
    [SerializeField]
    private Item item;

    private void Start() {

        
   

    }


    private void OnTriggerEnter2D(Collider2D other) {

                
        if (other.CompareTag("Player")) {

            if (InventoryScript.MyInstance.AddItem(item)) {

                Destroy(gameObject);
                UIManager.MyInstance.HideTooltip();

            }

                       
        }

    }

    public void OnMouseEnter() {

        Vector3 tmp = Camera.main.WorldToScreenPoint(this.transform.position);

        UIManager.MyInstance.ShowTooltip(new Vector2(1,0), tmp, item);

    }

    public void OnMouseExit() {

        UIManager.MyInstance.HideTooltip();
    }

}

    
