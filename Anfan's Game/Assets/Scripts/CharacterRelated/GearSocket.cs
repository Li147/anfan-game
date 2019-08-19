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

        // Override idle animations
        animatorOverrideController["Player_Idle_Down"] = animations[0];
        animatorOverrideController["Player_Idle_Left"] = animations[1];
        animatorOverrideController["Player_Idle_Right"] = animations[2];
        animatorOverrideController["Player_Idle_Up"] = animations[3];

        // Override spellcast animations
        animatorOverrideController["Player_Spellcast_Down"] = animations[4];
        animatorOverrideController["Player_Spellcast_Left"] = animations[5];
        animatorOverrideController["Player_Spellcast_Right"] = animations[6];
        animatorOverrideController["Player_Spellcast_Up"] = animations[7];
        
        // Override walk animations
        animatorOverrideController["Player_Walk_Down"] = animations[8];
        animatorOverrideController["Player_Walk_Left"] = animations[9];
        animatorOverrideController["Player_Walk_Right"] = animations[10];
        animatorOverrideController["Player_Walk_Up"] = animations[11];

        // Override slash animations
        animatorOverrideController["Player_Slash_Down"] = animations[12];
        animatorOverrideController["Player_Slash_Left"] = animations[13];
        animatorOverrideController["Player_Slash_Right"] = animations[14];
        animatorOverrideController["Player_Slash_Up"] = animations[15];

        // Override death animation
        animatorOverrideController["Player_Death"] = animations[16];






    }

    public void DequipAnimation() {

        animatorOverrideController["Player_Idle_Down"] = null;
        animatorOverrideController["Player_Idle_Left"] = null;
        animatorOverrideController["Player_Idle_Right"] = null;
        animatorOverrideController["Player_Idle_Up"] = null;

        animatorOverrideController["Player_Spellcast_Up"] = null;
        animatorOverrideController["Player_Spellcast_Right"] = null;
        animatorOverrideController["Player_Spellcast_Left"] = null;
        animatorOverrideController["Player_Spellcast_Down"] = null;

        animatorOverrideController["Player_Walk_Down"] = null;
        animatorOverrideController["Player_Walk_Left"] = null;
        animatorOverrideController["Player_Walk_Right"] = null;
        animatorOverrideController["Player_Walk_Up"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;


    }
}
