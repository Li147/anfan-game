using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    public ObservableStack<Item> MyItems { get => items; }

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    // A reference to the bag the slot belongs to
    public BagScript MyBag { get; set; }

    public int MyIndex { get; set; }

    public bool IsEmpty {
        get {
            return MyItems.Count == 0;
        }
    }

    public bool isFull
    {
        get {
            if (IsEmpty || MyCount < MyItem.MyStackSize) {
                return false;
            }
            return true;
            
        }

    }

    public Item MyItem
    {
        get {
            if (!IsEmpty) {
                return MyItems.Peek();
            }
            return null;
        }
    }

    public Image MyIcon {

        get {
            return icon;
        }
        set {
            icon = value;
        }

    }

    public int MyCount {
        get {
            return MyItems.Count;
        }
    }

    public Text MyStackText => stackSize;

    

    private void Awake() {
        // every time these events are raised -> we call the update slot function to update UI
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);

    }

    public bool AddItem(Item item) {

        MyItems.Push(item);

        // Set the sprite of the slot to the  image of the item
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        

        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems) {

        // Check if the slot is empty OR if the stack is the same type
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType()) {

            int count = newItems.Count;

            for (int i = 0; i < count; i++) {

                if (isFull) {
                    return false;
                }

                AddItem(newItems.Pop());


            }
            return true;

        }
        return false;


    }



    public void RemoveItem(Item item)
    {
        if (!IsEmpty) {

           InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            

        }

    }

    public void Clear()
    {
        int initCount = MyItems.Count;

        if (initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
        }
    }

    // responsible
    public void OnPointerClick(PointerEventData eventData) {

        // If we LEFT-CLICK on the item, drag it
        if (eventData.button == PointerEventData.InputButton.Left) {

            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) { // If we haven't got something to move

                if (HandScript.MyInstance.MyMoveable != null) {

                    if (HandScript.MyInstance.MyMoveable is Bag) {

                        if (MyItem is Bag) {
                            InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                        }

                    }
                    // picking up an armour from character panel and clicking on inventory
                    else if (HandScript.MyInstance.MyMoveable is Armour) {

                        if (MyItem is Armour && (MyItem as Armour).MyArmourType == (HandScript.MyInstance.MyMoveable as Armour).MyArmourType) {

                            (MyItem as Armour).Equip();
                            
                            HandScript.MyInstance.Drop();

                        }

                    }

                    

                } else {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }

               

            } else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty) { // if slotscript is empty

                // dequip bag from character, add bag to bag
                if (HandScript.MyInstance.MyMoveable is Bag) {

                    Bag bag = (Bag)HandScript.MyInstance.MyMoveable;



                    if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.MySlotCount > 0) {

                        // Adds bag item to inventory
                        AddItem(bag);

                        bag.MyBagButton.RemoveBag();
                        HandScript.MyInstance.Drop();

                    }

                }
                // Dequip armour from character, add armor to bag
                else if (HandScript.MyInstance.MyMoveable is Armour) {

                    Armour armour = (Armour) HandScript.MyInstance.MyMoveable;
                    CharacterPanel.MyInstance.MySelectedButton.DequipArmour();
                    AddItem(armour);
                    HandScript.MyInstance.Drop();


                }
                
               



            } else if (InventoryScript.MyInstance.FromSlot != null) { // If we have something to move

                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems)) {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }

            }


        }

        // If we RIGHT-CLICK on the item, use the item
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMoveable == null) {
            UseItem();
        }
    }

    

          
    public void UseItem()
    {
        if (MyItem is IUseable) {

            (MyItem as IUseable).Use();

        } 

        else if(MyItem is Armour) {

            (MyItem as Armour).Equip();

        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize){
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
        
    }

    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;

        }
        return false;

    }

    //If (the item i'm moving)  != (the item i am clicking on) OR (# of potions in my hand + # of potions i am clicking on) > than total stack size
    // then I need to SWAP the items
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

       
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
        {
            // Copy all items from the old slot
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            // clear items from old slot
            from.MyItems.Clear();

            // Transfer destination items to old slot
            from.AddItems(MyItems);

            // Clear new slot
            MyItems.Clear();

            // Copy items into new slot
            AddItems(tmpFrom);

            return true;

        }
        return false;

    }


    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() == MyItem.GetType() && !isFull)
        {
            // How many free slots do we have for the merge?
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++) {

                if (from.MyCount > 0) {
                    AddItem(from.MyItems.Pop());
                }

                
            }

            return true;

        }

        return false;           
    }


    private void UpdateSlot() {

        UIManager.MyInstance.UpdateStackSize(this);

    }

    // Method for showing tooltip of an item
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, MyItem);
        }   
    }

    // Method for hiding tooltip
    public void OnPointerExit(PointerEventData eventData) {

        UIManager.MyInstance.HideTooltip();
    }
}
