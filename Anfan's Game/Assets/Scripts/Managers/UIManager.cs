using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton -> a class that is instantiated into only one object, no more no less
    // Why? -> useful for retaining an object for whole time playing the game

    private static UIManager instance;

    public static UIManager MyInstance {

        get {
            if (instance == null) {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }



    }


    [SerializeField]
    private ActionButton[] actionButtons;
       
    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;


    private GameObject[] keybindButtons;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private CharacterPanel charPanel;


    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;



    private void Awake() {

        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();

    }




    // Start is called before the first frame update
    void Start()
    {

        
        healthStat = targetFrame.GetComponentInChildren<Stat>();

                     
    }

    // Update is called once per frame
    void Update()
    {
        // Opens the keybind menu
        if (Input.GetKeyDown(KeyCode.Escape)) {

            OpenClose(keybindMenu);

        }

        // Opens the spellbook menu
        if (Input.GetKeyDown(KeyCode.P)) {

            OpenClose(spellBook);

        }

        // Opens the inventory menu
        if (Input.GetKeyDown(KeyCode.B)) {

            InventoryScript.MyInstance.OpenClose();
            
        }

        if (Input.GetKeyDown(KeyCode.C)) {

            charPanel.OpenClose();

        }

    }



    public void ShowTargetFrame(NPC target) {

        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        // I have an event on target called healthchanged, 
        // updatetargetframe listens for a change in HealthChanged
        // if event is triggered, it executes function UpdateTargetFrame
        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

    }


    public void HideTargetFrame() {

        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float value) {

        healthStat.MyCurrentValue = value;

    }

   
    public void UpdateKeyText(string key, KeyCode code) {

        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();

        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName) {

        // look through actionbutton array and find the correct name
        // then click and invoke
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();

    }

   

    public void OpenClose(CanvasGroup canvasGroup) {

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;

    }

    public void UpdateStackSize(IClickable clickable) {

        if (clickable.MyCount > 1) {

            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;

        } else {

            ClearStackCount(clickable);
        }


        // If there are no more items left in the clickable slot
        if (clickable.MyCount == 0) {

            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);

        }
    }


    public void ClearStackCount(IClickable clickable) {

        clickable.MyStackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;

    }


    // Shows tooltip containing item description
    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description) {

        tooltipRect.pivot = pivot;


        tooltip.SetActive(true);
        tooltip.transform.position = position;

        tooltipText.text = description.GetDescription();

    }

    public void HideTooltip() {

        tooltip.SetActive(false);

    }

    public void RefreshTooltip(IDescribable description) {

        tooltipText.text = description.GetDescription();


    }


}
