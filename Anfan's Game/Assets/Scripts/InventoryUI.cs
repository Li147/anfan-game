
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    [SerializeField]
    private Sprite[] itemSprites;

    public bool[] isFull = new bool[10];
    public GameObject[] slots = new GameObject[10];

    public Inventory inventory;


    void Start() {
        
         
    }


    // picked up item, update sprite of the button
    public void UpdateUI() {
        for (int i = 0; i < slots.Length; i++) {

        }
    }

    // clicked on item
    public void UseItem() {

    }

    public void DropItem() {

    }


}
