using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; private set; }
    private Transform source;

    [SerializeField]
    private int damage;
    private float lifetime = 2;


    // Start is called before the first frame update
    void Start()
    {
        // Creates reference to arrow's rigidbody
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Transform source)
    {
        this.source = source;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "enemy")
        {

            Character c = collision.GetComponentInParent<Character>();

            // set speed to 0 when we hit the target so we don't get flicker
            speed = 0;

            c.TakeDamage(damage, source);


            //GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
            Destroy(gameObject);
            
        }
        Destroy(gameObject, lifetime);
    }
}
