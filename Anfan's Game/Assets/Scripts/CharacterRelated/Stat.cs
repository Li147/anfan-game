using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;

    [SerializeField]
    private Text statValue;

    [SerializeField]
    private float lerpSpeed;

    private float currentFill;

    private float overflow;

    public float MyMaxValue { get; set; }
    
    public bool isFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }
   
    private float currentValue;



    public float MyCurrentValue {
        get => currentValue;
        set {
            if (value > MyMaxValue) // ensures that we don't get too much health
            {
                MyOverflow = value - MyMaxValue;
                currentValue = MyMaxValue;

            } else if (value < 0) {
                currentValue = 0;
    
            } else {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            if (statValue != null) {

                statValue.text = currentValue + "/" + MyMaxValue;
            }



            
            
        }

    }

    public float MyOverflow
    {
        get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
        set => overflow = value; }


    // Start is called before the first frame update
    void Start()
    {
        
        content = GetComponent<Image>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount) {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }

        
    }

    public void Initialize(float currentValue, float maxValue) {

        if (content == null) {

            content = GetComponent<Image>();

        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }

}
