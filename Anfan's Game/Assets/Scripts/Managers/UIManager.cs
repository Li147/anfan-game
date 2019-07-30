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



    private void Awake() {

        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

    }




    // Start is called before the first frame update
    void Start()
    {

        
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("Firebolt"));
        SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("Icebolt"));
        SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("Thunderbolt"));



    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            OpenClose(keybindMenu);

        }

        if (Input.GetKeyDown(KeyCode.P)) {

            OpenClose(spellBook);

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

    public void SetUseable(ActionButton btn, IUseable useable) {

        btn.MyIcon.sprite = useable.MyIcon;
        btn.MyIcon.color = Color.white;

        btn.MyUseable = useable;

    }

    public void OpenClose(CanvasGroup canvasGroup) {

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;

    }

}
