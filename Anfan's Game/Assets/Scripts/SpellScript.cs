using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{

    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; private set; }

    private int damage;

            
    // Start is called before the first frame update
    void Start()
    {
        // Creates reference to spell's rigidbody
        myRigidBody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Transform target, int damage) {

        this.MyTarget = target;
        this.damage = damage;

    }




    private void FixedUpdate() {
        
        if (MyTarget != null) {

            // Spell will move towards target

            Vector2 direction = MyTarget.position - transform.position;

            myRigidBody.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotates spell so face the target
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
                
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.tag == "hitbox" && collision.transform == MyTarget) {

            // set speed to 0 when we hit the target so we don't get flicker
            speed = 0;

            collision.GetComponentInParent<Enemy>().TakeDamage(damage);


            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
