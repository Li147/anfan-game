using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {

    [SerializeField]
    private Sprite[] itemSprites;

    [SerializeField]
    private int itemID;

    private Inventory inventory;
    public GameObject itemButton;
   
    

    private void Start() {
        
        
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

    }


    private void OnTriggerEnter2D(Collider2D other) {

        //if (other.CompareTag("Player")) {
            
        //    for (int i = 0; i < inventory.slots.Length; i++) {

        //        if (inventory.isFull[i] == false) {

        //            inventory.isFull[i] = true;
        //            Instantiate(itemButton, inventory.slots[i].transform, false);
        //            Destroy(gameObject);
        //            break;
        //        }
        //    }
        //}

        

        if (other.CompareTag("Player")) {

            for (int i = 0; i < inventory.slots.Length; i++) {

                if (inventory.isFull[i] == false) {

                    inventory.isFull[i] = true;

                    GameObject button = (GameObject) Instantiate(itemButton, inventory.slots[i].transform, false);
                    //button.GetComponent<Image>().sprite = itemSprites[1];

                    itemButton.GetComponent<Image>().sprite = itemSprites[itemID];
                    

                    Destroy(gameObject);
                    break;
                }
            }
        }


    }


}

    
