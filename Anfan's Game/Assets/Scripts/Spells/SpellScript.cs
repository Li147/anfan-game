using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Rigidbody2D myRigidBody;

    public Transform MyTarget { get; private set; }
    private Transform source;

    private int damage;
    private float speed;


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

    public void Initialize(Transform target, Transform source, int damage, int speed) {

        this.MyTarget = target;
        this.source = source;
        this.damage = damage;
        this.speed = speed;

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

            Character c = collision.GetComponentInParent<Character>();
            c.TakeDamage(damage, source);

            // set speed to 0 when we hit the target so we don't get flicker
            speed = 0;

            if (animator != null)
            {
                animator.SetTrigger("impact");
                myRigidBody.velocity = Vector2.zero;
                MyTarget = null;
            }
            else
            {
                Destroy(gameObject);
            } 
            
        }
    }
}
