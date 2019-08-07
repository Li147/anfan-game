using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    public GameObject crosshairs;
    public GameObject bow;
    public GameObject arrowPrefab;
    public GameObject arrowStart;
    public Player player;

    public float arrowSpeed = 5.0f;


    private Vector3 target;


    // Start is called before the first frame update
    void Start()
    {
        // this code if we dont want to see cursor
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        crosshairs.transform.position = new Vector2(target.x, target.y);

        Vector3 difference = target - bow.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        bow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 225);


        if (Input.GetMouseButtonDown(0))
        {
            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            direction.Normalize();
            FireBullet(direction, rotationZ);
          
        }


    }

    public void FireBullet(Vector2 direction, float rotationZ)
    {
        ArrowScript tmp = Instantiate(arrowPrefab).GetComponent<ArrowScript>();
        tmp.Initialize(player.transform);
        tmp.transform.position = arrowStart.transform.position;
        tmp.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 45);
        tmp.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;

    }
}
