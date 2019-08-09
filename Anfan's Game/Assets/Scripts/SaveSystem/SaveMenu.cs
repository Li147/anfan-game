using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image hunger;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text hungerText;

    [SerializeField]
    private Text manaText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Text expText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int MyIndex { get => index; set => index = value; }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);
        dateTime.text = "Date: " + saveData.MyDateTime.ToString("dd/MM/yyyy") + "\nTime: " + saveData.MyDateTime.ToString("H:mm");

        health.fillAmount = saveData.MyPlayerData.MyHealth / saveData.MyPlayerData.MyMaxHealth;
        healthText.text = saveData.MyPlayerData.MyHealth + " / " + saveData.MyPlayerData.MyMaxHealth;

        hunger.fillAmount = saveData.MyPlayerData.MyHunger / saveData.MyPlayerData.MyMaxHunger;
        hungerText.text = saveData.MyPlayerData.MyHunger + " / " + saveData.MyPlayerData.MyMaxHunger;

        mana.fillAmount = saveData.MyPlayerData.MyMana / saveData.MyPlayerData.MyMaxMana;
        manaText.text = saveData.MyPlayerData.MyMana + " / " + saveData.MyPlayerData.MyMaxMana;

        levelText.text = "Level " + saveData.MyPlayerData.MyLevel;
        expText.text = saveData.MyPlayerData.MyXp + " / " + saveData.MyPlayerData.MyMaxXP;
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }


}
