using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node MyParent { get; set; }

    public Vector3Int Position {get; set; }

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}
