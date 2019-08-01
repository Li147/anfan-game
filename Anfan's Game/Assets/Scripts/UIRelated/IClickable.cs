using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// Interface for clickable slots
public interface IClickable {

    Image MyIcon {
        get;
        set;
    }

    int MyCount {
        get;
    }

    Text MyStackText {

        get;
    }

}