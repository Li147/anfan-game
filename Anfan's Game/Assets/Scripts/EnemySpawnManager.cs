using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;


    [SerializeField]
    Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPos = map.GetCellCenterWorld(new Vector3Int(3, 3, 0));

        Instantiate(enemy, spawnPos, Quaternion.identity);

        //SpawnEnemies();

        InvokeRepeating("SpawnEnemy", 30.0f, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnEnemy()
    {
        // find where the player currently is
        Vector3 spawnPos = Player.MyInstance.transform.position;

        // add some variation to the spawn coordinates
        float xVariation = Random.Range(10f, 20f) * (Random.Range(0, 2) * 2 - 1);
        float yVariation = Random.Range(10f, 20f) * (Random.Range(0, 2) * 2 - 1);

        spawnPos.x += xVariation;
        spawnPos.y += yVariation;

   

        //check if this position is over WATER
        Vector3Int pos = map.WorldToCell(spawnPos);

        Debug.Log(pos.ToString());
       



        if (map.GetTile(pos) is null || map.GetTile(pos).name == "water_plain_tile")
        {
            return;
        }
        else
        {
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }



        
    }


    public void SpawnEnemies()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Instantiate(enemy, map.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
            }



        }
    }
}
