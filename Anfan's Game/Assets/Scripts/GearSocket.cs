using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour
{

    public Animator MyAnimator { get; set; }

    private SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();

        MyAnimator = GetComponent<Animator>();

        //animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        //MyAnimator.runtimeAnimatorController = animatorOverrideController;

    }

    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
