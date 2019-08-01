using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup;

    private List<SlotScript> slots = new List<SlotScript>();


    public bool IsOpen {

        get {
            return canvasGroup.alpha > 0;
        }

    }

    public List<SlotScript> MySlots { get => slots;}

    private void Awake() {

        canvasGroup = GetComponent<CanvasGroup>();

    }

    // Gets all the contents of a bag and returns as a List
    public List<Item> GetItems() {

        List<Item> items = new List<Item>();

        foreach (SlotScript slot in slots) {

            if (!slot.IsEmpty) {

                foreach (Item item in slot.MyItems) {

                    items.Add(item);

                }

            }

        }

        return items;

    }




    public int MyEmptySlotCount {

        get {
            int count = 0;

            foreach(SlotScript slot in MySlots) {

                if (slot.IsEmpty) {
                    count++;
                }

            }
            return count;
        }

    }





    public void AddSlots(int slotCount) {

        for (int i = 0; i < slotCount; i++) {

            // Create i times slotprefabs, each a chhild of the BagScript
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyBag = this;
            MySlots.Add(slot);

        }

    }

    public bool AddItem(Item item) {

        foreach(SlotScript slot in MySlots) {
            if (slot.IsEmpty) {
                slot.AddItem(item);
                return true;
            }
        }
        return false;

    }


    public void OpenClose() {

        // makes inventory visible / invisible
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        // makes inventory block mouse clicks
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;

    }
}
