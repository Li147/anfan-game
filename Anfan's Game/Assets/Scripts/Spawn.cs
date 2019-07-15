using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject item;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    public void SpawnDroppedItem() {

        float xRandom = Random.Range(-1, 1);
        float yRandom = Random.Range(-1, 1);

        Vector2 itemDropPos = new Vector2(player.position.x + xRandom, player.position.y + yRandom);

        Instantiate(item, itemDropPos, Quaternion.identity);
    }

    

  
}
