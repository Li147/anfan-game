﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private Text spellName;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private CanvasGroup canvasGroup;



    [SerializeField]
    private Spell[] spells;

    private Coroutine spellRoutine;

    private Coroutine fadeRoutine;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spell FindSpell(int index) {

        Spell currentSpell = spells[index];

        castingBar.color = currentSpell.MyBarColor;
        castingBar.fillAmount = 0;

        spellName.text = currentSpell.MyName;

        spellRoutine = StartCoroutine(Progress(index));

        fadeRoutine = StartCoroutine(FadeBar());

        return currentSpell;

    }

    private IEnumerator Progress(int index) {

        Spell currentSpell = spells[index];

        float timePassed = Time.deltaTime;

        float rate = 1.0f / currentSpell.MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0) {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (currentSpell.MyCastTime - timePassed).ToString("F2");

            if (currentSpell.MyCastTime - timePassed < 0) {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCasting();

    }

    private IEnumerator FadeBar() {
                
        float rate = 1.0f / 0.25f;

        float progress = 0.0f;

        while (progress <= 1.0) {

            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

    }



    public void StopCasting() {

        if (fadeRoutine != null) {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (spellRoutine != null) {

            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

}