using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [SerializeField]
    private Text itemName;

    [SerializeField]
    private Text itemDescription;

    [SerializeField]
    private GameObject materialPrefab;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    private List<int> amounts = new List<int>();

    [SerializeField]
    private Recipe selectedRecipe;

    [SerializeField]
    private Text countText;

    [SerializeField]
    private ItemInfo craftItemInfo;


    // max number we can create
    private int maxAmount;

    // the amount we want to create
    private int amount;

    private int MyAmount
    {
        get
        {
            return amount;
        }

        set
        {
            countText.text = value.ToString();
            amount = value;
        }
    }

   

    public void Start()
    {
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
        ShowDescription(selectedRecipe);
    }


    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            selectedRecipe.Deselect();
        }

        this.selectedRecipe = recipe;
        this.selectedRecipe.Select();

        // clears the list so we dont get duplicates every time we click
        foreach (GameObject m in materials)
        {
            Destroy(m);
        }
        materials.Clear();

        itemName.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[recipe.MyOutput.MyQuality], recipe.MyTitle);

        itemDescription.text = recipe.MyDescription;

        craftItemInfo.Initialize(recipe.MyOutput, 1);

        foreach (CraftingMaterial material in recipe.MyMaterials)
        {
            Transform parent = GameObject.Find("MaterialsList").transform;
            GameObject tmp = Instantiate(materialPrefab, parent);

            tmp.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);



            materials.Add(tmp);

        }

        UpdateMaterialCount(null);


    }

    // Update selected recipe's materials every time inventory changes
    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        foreach (GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();


        }
        if (HaveEnoughMaterials())
        {
            maxAmount = amounts[0];

            if (countText.text == "0")
            {
                MyAmount = 1;
            }
            else if (int.Parse(countText.text) > maxAmount)
            {
                MyAmount = maxAmount;
            }
        }
        else
        {
            MyAmount = 0;
            maxAmount = 0;
        }
    }

    public void Craft(bool all)
    {
        if (HaveEnoughMaterials() && !Player.MyInstance.isSpellcasting)
        {
            if (all)
            {
                amounts.Sort();
                countText.text = maxAmount.ToString();
                StartCoroutine(CraftRoutine(amounts[0]));
            }
            else
            {
                StartCoroutine(CraftRoutine(MyAmount));
            }

            
        }
        
    }

    public bool HaveEnoughMaterials()
    {
        bool canCraft = true;
        amounts = new List<int>();



        foreach (CraftingMaterial material in selectedRecipe.MyMaterials)
        {
            int amountInInventory = InventoryScript.MyInstance.GetItemCount(material.MyItem.MyName);

            if (amountInInventory >= material.MyCount)
            {
                amounts.Add(amountInInventory / material.MyCount);
                continue;
            }
            else
            {
                canCraft = false;
                break;
            }
        }
        return canCraft;
    }

    public void ChangeAmount(int i)
    {
        if ((amount + i) > 0 && amount + i <= maxAmount)
        {
            MyAmount += i;
        }
    }








    private IEnumerator CraftRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Player.MyInstance.MyInitRoutine = StartCoroutine(Player.MyInstance.CraftRoutine(selectedRecipe));
        }

        
    }


    public void AddItemsToInventory()
    {
        // materials are only removed IF crafted item is successfully added to inventory
        if (InventoryScript.MyInstance.AddItem(craftItemInfo.MyItem))
        {
            foreach (CraftingMaterial material in selectedRecipe.MyMaterials)
            {
                for (int i = 0; i < material.MyCount; i++)
                {
                    InventoryScript.MyInstance.RemoveItem(material.MyItem);
                }
            }
        }
        
    }

}
