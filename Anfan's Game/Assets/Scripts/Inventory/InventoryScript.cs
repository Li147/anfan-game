using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance {

        get {
            if (instance == null) {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }



    }

    // fromSlot is the slot which we have just picked an item from with our mouse
    private SlotScript fromSlot;

    // List of all equipped bags
    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    // FOR DEBUGGING
    [SerializeField]
    private Item[] items;

    public bool CanAddBag {

        get { return bags.Count < 3; }

    }

    public int MyEmptySlotCount {

        get {
            int count = 0;

            foreach (Bag bag in bags) {

                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }

    }

    public int MyTotalSlotCount {

        get {
            int count = 0;

            foreach (Bag bag in bags) {

                count += bag.MyBagScript.MySlots.Count;

            }

            return count;
        }

    }

    public int MyFullSlotCount {

        get {
            return MyTotalSlotCount - MyEmptySlotCount;
        }

    }



    public SlotScript FromSlot {
        get => fromSlot;
        set {

            fromSlot = value;

            if (value != null) {
                fromSlot.MyIcon.color = Color.grey;
            }

        }

    }

    private void Awake() {

        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(20);
        bag.Use();

    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.J)) {

            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            AddItem(bag);

        }

        if (Input.GetKeyDown(KeyCode.K)) {

            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            AddItem(bag);

        }

        if (Input.GetKeyDown(KeyCode.L)) {

            HealthPotion potion = (HealthPotion) Instantiate(items[1]);
            AddItem(potion);

        }

        if (Input.GetKeyDown(KeyCode.H)) {

            
            AddItem((Armour)Instantiate(items[2]));
            AddItem((Armour)Instantiate(items[3]));
            AddItem((Armour)Instantiate(items[4]));
            AddItem((Armour)Instantiate(items[5]));
            AddItem((Armour)Instantiate(items[6]));
            AddItem((Armour)Instantiate(items[7]));
            AddItem((Armour)Instantiate(items[8]));
            AddItem((Armour)Instantiate(items[9]));


        }


    }

                    
    public void AddBag(Bag bag) {

        foreach(BagButton bagButton in bagButtons) {

            if (bagButton.MyBag == null) {

                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                break;
            }

        }

    }

    public void AddBag(Bag bag, BagButton bagButton) {

        bags.Add(bag);
        bagButton.MyBag = bag;
    }

    public void RemoveBag(Bag bag) {

        // Finds bag from list and removes it
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);

    }

    public void SwapBags(Bag oldBag, Bag newBag) {

        int newSlotCount = (MyTotalSlotCount - oldBag.Slots) + newBag.Slots;

        if (newSlotCount - MyFullSlotCount >= 0) {

            // do swap
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton;
            newBag.Use();

            foreach (Item item in bagItems) {

                if (item != newBag) { // ensure no duplicate bags

                    AddItem(item);
                }

            }

            AddItem(oldBag);

            HandScript.MyInstance.Drop();

            MyInstance.fromSlot = null;



        }

    }






    public bool AddItem(Item item) {

        // is the item a stackable?
        if (item.MyStackSize > 0) {

            // try to place it in a stack...
            if (PlaceInStack(item)) {
                return true;
            }
        }

        // ... if it doesnt work, place in an empty slot
        return PlaceInEmpty(item);

    }

    private bool PlaceInEmpty(Item item) {

        foreach (Bag bag in bags) {

            if (bag.MyBagScript.AddItem(item)) {

                OnItemCountChanged(item);
                return true;
            }
        }

        // if code gets to here, it means inventory is FULL
        return false;

    }



    private bool PlaceInStack(Item item) {

        foreach  (Bag bag in bags) {

            foreach (SlotScript slots in bag.MyBagScript.MySlots) {

                if (slots.StackItem(item)) {

                    OnItemCountChanged(item);
                    return true;

                }

            }

        }

        return false;

    }


    public void OpenClose() {

        // find any bag which is closed
        bool closedbag = bags.Find(x => !x.MyBagScript.IsOpen);

        // if a single closed bag -> open all closed bags
        // if not a single closed bag -> close all open bags

        foreach(Bag bag in bags) {

            if (bag.MyBagScript.IsOpen != closedbag) {

                bag.MyBagScript.OpenClose();

            }

        }

    }

    public Stack<IUseable> GetUseables(IUseable type) {

        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in bags) {

            foreach (SlotScript slot in bag.MyBagScript.MySlots) {

                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType()) {

                    foreach (Item item in slot.MyItems) {

                        useables.Push(item as IUseable);

                    }

                }

            }

        }
        return useables;

    }

    // good code practice to put your event inside a method titled On.XXXXX
    public void OnItemCountChanged(Item item) {

        if (itemCountChangedEvent != null) {

            itemCountChangedEvent.Invoke(item);

        }

    }

    
}
