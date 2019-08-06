using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WeaponSocket : GearSocket
{
    private float currentY;

    [SerializeField]
    private SpriteRenderer parentRenderer;

    public override void SetXAndY(float x, float y) {

        base.SetXAndY(x, y);

        if (currentY != y) {

            if (y == 1) {

                //back
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1;

            }
            else {

                //front
                spriteRenderer.sortingOrder = parentRenderer.sortingOrder + 5;

            }

        }

    }


}
