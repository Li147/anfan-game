using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible for changing our animations as we equip gear
public class GearSocket : MonoBehaviour
{

    public Animator MyAnimator { get; set; }

    protected SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake() {

        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();

        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);

        MyAnimator.runtimeAnimatorController = animatorOverrideController;

    }


    
    public virtual void SetXAndY(float x, float y) {

        // sets animation parameter  to ensure player faces correct direction
        MyAnimator.SetFloat("x", x);
        MyAnimator.SetFloat("y", y);

    }

    public void ActivateLayer(string layerName) {

        for (int i = 0; i < MyAnimator.layerCount; i++) {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);

    }


    public void EquipAnimation(AnimationClip[] animations) {

        spriteRenderer.color = Color.white;

        animatorOverrideController["Spellcast_Up"] = animations[7];
        animatorOverrideController["Spellcast_Right"] = animations[6];
        animatorOverrideController["Spellcast_Left"] = animations[5];
        animatorOverrideController["Spellcast_Down"] = animations[4];

        animatorOverrideController["Idle_Down"] = animations[0];
        animatorOverrideController["Idle_Left"] = animations[1];
        animatorOverrideController["Idle_Right"] = animations[2];
        animatorOverrideController["Idle_Up"] = animations[3];

        animatorOverrideController["Walk_Down"] = animations[8];
        animatorOverrideController["Walk_Left"] = animations[9];
        animatorOverrideController["Walk_Right"] = animations[10];
        animatorOverrideController["Walk_Up"] = animations[11];



    }

    public void DequipAnimation() {

        animatorOverrideController["Spellcast_Up"] = null;
        animatorOverrideController["Spellcast_Right"] = null;
        animatorOverrideController["Spellcast_Left"] = null;
        animatorOverrideController["Spellcast_Down"] = null;

        animatorOverrideController["Idle_Down"] = null;
        animatorOverrideController["Idle_Left"] = null;
        animatorOverrideController["Idle_Right"] = null;
        animatorOverrideController["Idle_Up"] = null;

        animatorOverrideController["Walk_Down"] = null;
        animatorOverrideController["Walk_Left"] = null;
        animatorOverrideController["Walk_Right"] = null;
        animatorOverrideController["Walk_Up"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;


    }
}
