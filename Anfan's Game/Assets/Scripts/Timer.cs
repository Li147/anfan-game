using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float currentTime;
    public Text textBox;

    public float MyCurrentTime { get => currentTime; set => currentTime = value; }

    // Start is called before the first frame update
    void Start()
    {
        textBox.text = "Time: " + MyCurrentTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        MyCurrentTime += Time.deltaTime;
        textBox.text = "Time: " + Mathf.Round(MyCurrentTime).ToString();
    }
}
