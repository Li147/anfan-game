using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private CanvasGroup[] menus;
       
    [SerializeField]
    private GameObject targetFrame;

    private Stat enemyHealthStat;

    [SerializeField]
    private Text enemyLevelText;

    [SerializeField]
    private Text enemyNameText;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;

    [SerializeField]
    private CanvasGroup gameOverCanvas;

    [SerializeField]
    private TextMeshProUGUI gameOverText;


    private GameObject[] keybindButtons;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private CharacterPanel charPanel;


    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;


    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }


    // Start is called before the first frame update
    void Start()
    {
        enemyHealthStat = targetFrame.GetComponentInChildren<Stat>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenClose(menus[5]);

        }

    }



    public void ShowTargetFrame(Enemy target) {

        targetFrame.SetActive(true);
        enemyHealthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        enemyLevelText.text = target.MyLevel.ToString();
        enemyNameText.text = target.MyName;

        // I have an event on target called healthchanged, 
        // updatetargetframe listens for a change in HealthChanged
        // if event is triggered, it executes function UpdateTargetFrame
        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        // change level text colour of enemy relative to player's level
        if (target.MyLevel >= Player.MyInstance.MyLevel + 5)
        {
            enemyLevelText.color = Color.red;
        }
        else if (target.MyLevel == Player.MyInstance.MyLevel + 3 || target.MyLevel == Player.MyInstance.MyLevel + 4)
        {
            enemyLevelText.color = new Color32(255, 124, 0, 255);
        }
        else if (target.MyLevel >= Player.MyInstance.MyLevel -2 && target.MyLevel <= Player.MyInstance.MyLevel + 2)
        {
            enemyLevelText.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.MyInstance.MyLevel - 3 && target.MyLevel > EXPManager.CalculateGrayLevel())
        {
            enemyLevelText.color = Color.green;
        }
        else
        {
            enemyLevelText.color = Color.gray;
        }

    }


    public void HideTargetFrame() {

        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float value) {

        enemyHealthStat.MyCurrentValue = value;

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

   
    // open or closes a menu 
    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    // Opens a single menu
    public void OpenSingle(CanvasGroup canvasGroup)
    {
        // closes all other menus
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        // open specific menu we want to show
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    // closes a single menu
    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
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

    public void RefreshCharacterFrame()
    {

    }

    public void ShowGameOver()
    {
        StartCoroutine(GameOverScreen());
    }

    public IEnumerator GameOverScreen()
    {
        int timeSurvived = (int) Timer.MyInstance.MyCurrentTime;
        gameOverText.text = "You survived for: " + timeSurvived + " seconds.";

        yield return new WaitForSeconds(5);


        gameOverCanvas.alpha = 1;
        gameOverCanvas.blocksRaycasts = true;

    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(1);
    }

}
