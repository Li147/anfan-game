using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{

    private Image content;

    private float currentFill;

    public float MyMaxValue { get; set; }
    

   
    private float currentValue;

    public float MyCurrentValue {
        get => currentValue;
        set {
            if (value > MyMaxValue) {
                currentValue = MyMaxValue;

            } else if (value < 0) {
                currentValue = 0;
    
            } else {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;
            
        }

    }






    // Start is called before the first frame update
    void Start()
    {
        
        content = GetComponent<Image>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        content.fillAmount = currentFill;
        
    }

    public void Initialize(float currentValue, float maxValue) {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }

}
