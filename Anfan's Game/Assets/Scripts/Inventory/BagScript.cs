using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;


    public void AddSlots(int slotCount) {

        for (int i = 0; i < slotCount; i++) {

            // Create i times slotprefabs, each a chhild of the BagScript
            Instantiate(slotPrefab, transform);

        }

    }
}
