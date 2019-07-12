using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int health = 100;
    private int hunger = 100;
    public Text healthText;
    public Text hungerText;

    void Update() {

        healthText.text = health.ToString();
        hungerText.text = hunger.ToString();

        if (Input.GetKeyDown(KeyCode.Space)) {
            health--;
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            hunger--;
        }
    }



}
