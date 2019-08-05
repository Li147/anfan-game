using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality { Common, Uncommon, Rare, Epic }

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>() {

        {Quality.Common, "#C37547" },
        {Quality.Uncommon, "#BAB9A7" },
        {Quality.Rare, "#F8CD1E" },
        {Quality.Epic, "#F81EE9" },


    };

    public static Dictionary<Quality, string> MyColors { get => colors; }
}
