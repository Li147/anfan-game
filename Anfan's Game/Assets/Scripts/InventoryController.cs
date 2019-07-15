using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    [SerializeField]
    private Sprite[] itemSprites;

    public bool[] isFull = new bool[10];
    public GameObject[] slots = new GameObject[10];

    // picked up item, update sprite of the button
    public void UpdateInventoryUI() {
        for (int i = 0; i < slots.Length; i++) {

        }
    }

    // clicked on item
    public void UseItem() {

    }

    public void DropItem() {

    }


}
