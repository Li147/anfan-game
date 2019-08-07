using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE {DAMAGE, HEAL, EXP}

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;

    public static CombatTextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;

    public void CreateText(Vector2 position, string text, SCTTYPE type, bool crit)
    {
        // offset

        position.y += 0.2f;

        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        sct.transform.position = position;

        string before = string.Empty;
        string after = string.Empty;

        switch (type)
        {
            case SCTTYPE.DAMAGE:
                before = "-";
                sct.color = Color.red;
                break;
            case SCTTYPE.HEAL:
                before = "+";
                sct.color = Color.green;
                break;
            case SCTTYPE.EXP:
                before = "+";
                after = "Exp";
                sct.color = Color.yellow;
                break;

        }

        sct.text = before + text + after;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("crit", crit);
        }
    }
}
