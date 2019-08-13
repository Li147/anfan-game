using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text stack;

    private int quantityRequired;

    public Item MyItem { get => item; set => item = value; }

    public void Initialize(Item item, int count)
    {
        this.MyItem = item;
        this.image.sprite = item.MyIcon;
        this.title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[item.MyQuality], item.MyName);
        this.quantityRequired = count;

        if (quantityRequired > 1)
        {
            stack.enabled = true;
            stack.text = InventoryScript.MyInstance.GetItemCount(item.MyName).ToString() + "/" + quantityRequired.ToString();
        }
    }

    public void UpdateStackCount()
    {
        stack.text = InventoryScript.MyInstance.GetItemCount(item.MyName) + "/" + quantityRequired.ToString();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(new Vector2(0, 0), transform.position, MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

  
}
