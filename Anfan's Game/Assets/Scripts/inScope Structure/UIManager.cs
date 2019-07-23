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
    private Button[] actionButton;

    private KeyCode action1, action2, action3;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        //Keybinds
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action1)) {
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(action2)) {
            ActionButtonOnClick(1);

        }
        if (Input.GetKeyDown(action3)) {
            ActionButtonOnClick(2);

        }
    }


    private void ActionButtonOnClick(int btnIndex) {
        actionButton[btnIndex].onClick.Invoke();
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
}
