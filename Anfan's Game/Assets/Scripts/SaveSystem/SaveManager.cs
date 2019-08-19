using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// manages saving
public class SaveManager : MonoBehaviour
{
    // array of all items in the game
    [SerializeField]
    private Item[] items;

    private Chest[] chests;

    private CharButton[] equipment;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private SaveMenu[] saveSlots;

    [SerializeField]
    private GameObject dialogue;

    [SerializeField]
    private Text dialogueText;

    private SaveMenu current;


    private string action;

    // This runs first, before all other functions
    void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();

        foreach (SaveMenu save in saveSlots)
        {
            ShowSaveFiles(save);
        }

        
        
    }

    // Runs next
    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            // set default values for player
            Player.MyInstance.SetDefaultValues();
            //Player.MyInstance.transform.position = Player.MyInstance.tileMap.GetCellCenterWorld(new Vector3Int(50, 50, 0));

            Bounds b = Player.MyInstance.tileMap.localBounds;
            Player.MyInstance.transform.position = b.center;
            Debug.Log(b.ToString());
        }
    }

    // Shows a confirmation dialogue screen so player can confirm action
    public void ShowDialogue(GameObject clickButton)
    {
        action = clickButton.name;

        switch (action)
        {
            case "Load":
                dialogueText.text = "Load game?";
                break;
            case "Save":
                dialogueText.text = "Save game?";
                break;
            case "Delete":
                dialogueText.text = "Delete this game save?";
                break;
        }
        current = clickButton.GetComponentInParent<SaveMenu>();
        dialogue.SetActive(true);

    }

    // Method is called when player presses "YES" icon
    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
        }
        CloseDialogue();
    }

    // Method is called when player presses "NO" icon 
    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }


    // Loads an existing save file
    private void LoadScene(SaveMenu savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", savedGame.MyIndex);
            SceneManager.LoadScene(data.MyScene);
      
        }
    }


    // Delete an existing save file
    private void Delete(SaveMenu savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();
    }


    
    // Updates UI to show the available save files
    private void ShowSaveFiles(SaveMenu savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }


    //========================================SAVING CODE BELOW===================================================//

    public void Save(SaveMenu savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

            SaveData data = new SaveData();

            data.MyScene = SceneManager.GetActiveScene().name;

            SaveEquipment(data);
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveChests(data);
            SaveActionButton(data);

            bf.Serialize(file, data);
            file.Close();

            ShowSaveFiles(savedGame);

        }
        catch (System.Exception)
        {
            Delete(savedGame);

            PlayerPrefs.DeleteKey("Load");

        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Player.MyInstance.MyLevel,
            Player.MyInstance.MyExp.MyCurrentValue, Player.MyInstance.MyExp.MyMaxValue,
            Player.MyInstance.MyHealth.MyCurrentValue, Player.MyInstance.MyHealth.MyMaxValue,
            Player.MyInstance.MyMana.MyCurrentValue, Player.MyInstance.MyMana.MyMaxValue,
            Player.MyInstance.MyHunger.MyCurrentValue, Player.MyInstance.MyHunger.MyMaxValue,
            Player.MyInstance.transform.position);
    }

    public void SaveChests(SaveData data)
    {
        // running through all the chests in the game world
        for (int i = 0; i < chests.Length; i++)
        {
            data.MyChestData.Add(new ChestData(chests[i].name));

            foreach (Item item in chests[i].MyItems)
            {
                //check if chest even has items first
                if (chests[i].MyItems.Count > 0)
                {
                    data.MyChestData[i].MyItems.Add(new ItemData(item.MyName, item.MySlot.MyItems.Count, item.MySlot.MyIndex));
                }
            }
        }
    }

    public void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.MyInstance.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.MyInstance.MyBags[i].MySlotCount,
                InventoryScript.MyInstance.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    public void SaveEquipment(SaveData data)
    {
        foreach (CharButton charButton in equipment)
        {
            if (charButton.MyEquippedArmour != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(charButton.MyEquippedArmour.MyName, charButton.name));
            }
        }
    }

    public void SaveActionButton(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUseable != null)
            {

                ActionButtonData action;

                if (actionButtons[i].MyUseable is Spell)
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Spell).MyTitle, false, i);
                }
                else
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Item).MyName, true, i);
                }

                data.MyActionBarData.Add(action);
            }
        }
    }

    public void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyName, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    //========================================SAVING CODE END======================================================//


    //========================================LOADING CODE BELOW===================================================//




    private void Load(SaveMenu savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadEquipment(data);
            LoadBags(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadChests(data);
            LoadActionButtons(data);

        }
        catch (System.Exception)
        {
            // if there's an error, just delete the save file
            Delete(savedGame);
            Player.MyInstance.SetDefaultValues();
            //SceneManager.LoadScene("string scenename");
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.MyInstance.MyLevel = data.MyPlayerData.MyLevel;
        Player.MyInstance.UpdateLevel();
        Player.MyInstance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.MyInstance.MyMana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Player.MyInstance.MyHunger.Initialize(data.MyPlayerData.MyHunger, data.MyPlayerData.MyMaxHunger);
        Player.MyInstance.MyExp.Initialize(data.MyPlayerData.MyXp, data.MyPlayerData.MyMaxXP);
        Player.MyInstance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);
    }

    private void LoadChests(SaveData data)
    {
        foreach (ChestData chest in data.MyChestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.MyName);

            foreach (ItemData itemData in chest.MyItems)
            {
                Item item = Instantiate(Array.Find(items, x => x.MyName == itemData.MyItemName));
                item.MySlot = c.MyBag.MySlots.Find(x => x.MyIndex == itemData.MySlotIndex);
                c.MyItems.Add(item);
            }
        }
    }

    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            
            Bag newBag = (Bag)Instantiate(items[1]);

            newBag.Initialize(bagData.MySlotCount);

            InventoryScript.MyInstance.AddBag(newBag, bagData.MyBagIndex);

        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            CharButton cb = Array.Find(equipment, x => x.name == equipmentData.MyType);

            cb.EquipArmour(Array.Find(items, x => x.MyName == equipmentData.MyName) as Armour);
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.MyActionBarData)
        {
            if (buttonData.IsItem)
            {
                actionButtons[buttonData.MyIndex].SetUseable(InventoryScript.MyInstance.GetUseable(buttonData.MyAction));
            }
            else
            {
                actionButtons[buttonData.MyIndex].SetUseable(SpellBook.MyInstance.GetSpell(buttonData.MyAction));
            }
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Instantiate(Array.Find(items, x => x.MyName == itemData.MyItemName));

            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.MyInstance.PlaceInSpecificSlot(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }


    //========================================LOADING CODE END===================================================//


}

