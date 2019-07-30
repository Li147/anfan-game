using UnityEngine;
using System.Collections;

// anything that is clickable in game will be an IUseable
public interface IUseable {

    Sprite MyIcon {

        get;
    }

    void Use();




}


