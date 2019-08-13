using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, ICastable
{
    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private int outputCount;

    [SerializeField]
    private string description;

    [SerializeField]
    private Image highlight;

    [SerializeField]
    private Text recipeText;

    [SerializeField]
    private Image recipeIcon;

    [SerializeField]
    private float craftTime;

    [SerializeField]
    private Color barColor;

    private Sprite icon;

    public CraftingMaterial[] MyMaterials { get => materials; set => materials = value; }
    public Item MyOutput { get => output;}
    public int MyOutputCount { get => outputCount; set => outputCount = value; }
    public string MyDescription { get => description;}

    public string MyTitle
    {
        get
        {
            return output.MyName;
        }
    }

    public Sprite MyIcon
    {
        get
        {
            return output.MyIcon;
        }
    }

    public float MyCastTime
    {
        get
        {
            return craftTime;
        }
    }
    public Color MyBarColor
    {
        get
        {
            return barColor;
        }
    }



    private void Start()
    {
        recipeText.text = MyOutput.MyName;
        recipeIcon.sprite = MyOutput.MyIcon;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = 0.3f;
        highlight.color = c;
    }

    public void Deselect()
    {
        Color c = highlight.color;
        c.a = 0.0f;
        highlight.color = c;
    }

}
