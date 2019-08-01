﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour {

    private Transform target;

    private float xMax, xMin, yMin, yMax;

    [SerializeField]
    private Tilemap tilemap;

    private Player player;




    // Start is called before the first frame update
    void Start() {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>();

        // Cell to world returns the world position of a tile
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        // Sets limits of where camera can move
        SetLimits(minTile, maxTile);

        // Sets limits of where player can move
        player.SetLimits(minTile, maxTile);



    }

    // Called after fixed update -> camera will always be following player
    private void LateUpdate() {

        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);


    }


    private void SetLimits(Vector3 minTile, Vector3 maxTile) {

        Camera cam = Camera.main;

        // height of the camera, width of camera
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;


    }



}