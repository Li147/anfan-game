using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler {
    
    public IUseable MyUseable { get; set; }

    private Stack<IUseable> useables = new Stack<IUseable>();

    [SerializeField]
    private Text stackSize;

    private int count;

    public Button MyButton { get; private set; }
    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount => count;

    public Text MyStackText {

        get {
            return stackSize;
        }
    }


    [SerializeField]
    private Image icon;

    // Start is called before the first frame update
    void Start()
    {

        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {

        if (HandScript.MyInstance.MyMoveable == null) {

            if (MyUseable != null) {

                MyUseable.Use();

            }
            if (useables != null && useables.Count > 0) {

                useables.Peek().Use();

            }

        }

        

    }



    public void OnPointerClick(PointerEventData eventData) {
        
        if (eventData.button == PointerEventData.InputButton.Left) {

            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable) {

                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);



            }

        }
    }


    public void SetUseable(IUseable useable) {
        
        // If the useable I dragged onto is an item...
        if (useable is Item) {

            useables = InventoryScript.MyInstance.GetUseables(useable);
            count = useables.Count;

            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot = null;

        } else { // ...else just set useable as useable

            this.MyUseable = useable;

        }

        
        // needs (1) change icon (2) update stack size text
        UpdateVisual();

    }

    public void UpdateVisual() {

        MyIcon.sprite = HandScript.MyInstance.Put().MyIcon;
        MyIcon.color = Color.white;

        if (count > 1) {

            UIManager.MyInstance.UpdateStackSize(this);

        }

    }

    public void UpdateItemCount(Item item) {

        // When we pick something up
        // check if item is the same item as the one on the button

        if (item is IUseable && useables.Count > 0) {

            // Checks if item I have on ActionButton is the SAME type as the item 
            // that just got added to my bag
            if (useables.Peek().GetType() == item.GetType()) {

                useables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = useables.Count;

                UIManager.MyInstance.UpdateStackSize(this);

            }

        }

    }

    public void OnPointerEnter(PointerEventData eventData) {

        IDescribable tmp = null;
                
        if (MyUseable != null && MyUseable is IDescribable) {

            tmp = (IDescribable)MyUseable;
            //UIManager.MyInstance.ShowTooltip(transform.position);

        } else if (useables.Count > 0) {

            //UIManager.MyInstance.ShowTooltip(transform.position);

        }

        if (tmp != null) {
            UIManager.MyInstance.ShowTooltip(new Vector2(2,0), transform.position, tmp);
        }
       
    }




    public void OnPointerExit(PointerEventData eventData) {

        UIManager.MyInstance.HideTooltip();
    }
}
