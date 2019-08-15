using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    [SerializeField]
    private TypeOfTile tileType;

    public TypeOfTile MyTileType { get => tileType;}
}
