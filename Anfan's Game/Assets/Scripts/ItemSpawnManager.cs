using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{

    private static ItemSpawnManager instance;
    public static ItemSpawnManager MyInstance {

        get {
            if (instance == null) {

                instance = GameObject.FindObjectOfType<ItemSpawnManager>();

            }
            return instance;
        }

    }
   
    private Transform player;

    // The GameObject to instantiate.
    [SerializeField]
    private GameObject[] itemPrefabs;

    // An instance of the ScriptableObject defined above.
    public Item[] spawnManagerValues;

    // This will be appended to the name of the created entities and increment when each is created.
    int instanceNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        
        SpawnEntities(0, 2);
        SpawnEntities(6, 2);
        

    }

    


    

    public void SpawnEntities(int itemIndex, int quantity) {

        
        int currentSpawnPointIndex = 0;
        

        for (int i = 0; i < quantity; i++) {

            float xRandom = Random.Range(-0.5f, 0.5f);
            float yRandom = Random.Range(-0.5f, 0.5f);
            Vector2 itemDropPos = new Vector2(player.position.x + xRandom, player.position.y + yRandom);



            // Creates an instance of the prefab at the current spawn point.
            GameObject currentEntity = Instantiate(itemPrefabs[itemIndex], itemDropPos, Quaternion.identity);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = itemPrefabs[itemIndex].name + "_" + instanceNumber;

            // possible to change sprite of entity as it spawns
            //currentEntity.GetComponent<SpriteRenderer>().sprite = cheese;

            // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
            //currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

            instanceNumber++;
        }
    }




}
