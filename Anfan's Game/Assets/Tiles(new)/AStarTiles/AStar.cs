﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TypeOfTile { START, GOAL, WATER, GRASS, PATH}

public class AStar : MonoBehaviour
{
    private TypeOfTile tileType;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask mask;

    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Stack<Vector3Int> path;

    private HashSet<Vector3Int> changedTiles = new HashSet<Vector3Int>();

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private List<Vector3Int> waterTiles = new List<Vector3Int>();

    private bool start, goal;




    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mask);

            if (hit.collider != null)
            {
                // the world position of the mouse
                Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);

                // the cell position of the specific tile you clicked on
                Vector3Int clickPos = tilemap.WorldToCell(mouseWorldPos);

                ChangeTile(clickPos);

            }

        }

        


    }


    private void Initialize()
    {
        current = GetNode(startPos);

        openList = new HashSet<Node>();

        closedList = new HashSet<Node>();

        // adds start to the open list
        openList.Add(current);
    }



    // the main A star algorithm
    public void Algorithm(bool step)
    {
        if (current == null)
        {
            Initialize();
        }

        // loop only runs until the path is not null
        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);

            if (step)
            {
                break;
            }
        }

        if (path != null)
        {
            foreach (Vector3Int position in path)
            {
                if (position != goalPos)
                {
                    tilemap.SetTile(position, tiles[4]);
                }
            }
        }



       

        AStarDebugger.MyInstance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);
    }


    // Returns all neighbors of a node
    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1 ; x++)
        {
            for (int y = -1; y <= 1; y++)
            {

                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                if (y != 0 || x != 0)
                {
                    if (neighborPos != startPos && !waterTiles.Contains(neighborPos) && tilemap.GetTile(neighborPos)) // ensures that tiles outside of the tilemap are not added to neighbors
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                    
                }
            }
        }

        return neighbors;

    }

    // takes all neighbors and add to open list
    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        // run through the 8 neighbors of the current node
        for (int i = 0; i < neighbors.Count; i++)
        {
            // create a reference to the neighbor
            Node neighbor = neighbors[i];

            if (!ConnectedDiagonally(current,neighbor))
            {
                continue;
            }
            
            // calculate the G score for that specific neighbour
            int gScore = DetermineGScore(neighbors[i].Position, current.Position);

            // if the neighbor is already on the open list
            if (openList.Contains(neighbor))
            {
                // check if G score is better with this parent
                if (current.G + gScore < neighbor.G)
                {
                    CalcValues(current, neighbor, gScore);
                }
            }
            // if the neighbor is not on the open list
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);

                openList.Add(neighbor);
            }

        }
    }




    private void CalcValues(Node parent, Node neighbor, int costs)
    {
        neighbor.MyParent = parent;

        neighbor.G = parent.G + costs;

        neighbor.H = ((Math.Abs((neighbor.Position.x - goalPos.x)) + Math.Abs((neighbor.Position.y - goalPos.y))) * 10);

        neighbor.F = neighbor.G + neighbor.H;
    }


    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if(Math.Abs(x-y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;

    }





    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }




    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))   // if it already exists -> find and return node
        {
            return allNodes[position];
        }
        else                           // if it doesnt exist -> create, add to list, and return node
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }



    public void ChangeTileType(TileButton button)
    {
        tileType = button.MyTileType;
        
    }

    private void ChangeTile(Vector3Int clickPos)
    {
        if (tileType == TypeOfTile.WATER)
        {
            tilemap.SetTile(clickPos, tiles[(int) tileType]);
            waterTiles.Add(clickPos);
        }

        if (tileType == TypeOfTile.START)
        {
            if (start)
            {
                tilemap.SetTile(startPos, tiles[3]);
            }
            start = true;
            startPos = clickPos;

        }

        else if (tileType == TypeOfTile.GOAL)
        {
            if (goal)
            {
                tilemap.SetTile(goalPos, tiles[3]);
            }
            goal = true;
            goalPos = clickPos;
        }


        tilemap.SetTile(clickPos, tiles[(int)tileType]);

        changedTiles.Add(clickPos);



    }

    // prevents pathfinding from "cutting corners"
    private bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;

        Vector3Int first = new Vector3Int(currentNode.Position.x + (direct.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(currentNode.Position.x, currentNode.Position.y + (direct.y * -1), currentNode.Position.z);

        if (waterTiles.Contains(first) || waterTiles.Contains(second))
        {
            return false;
        }

        return true;


    }



    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while (current.Position != startPos)
            {
                finalPath.Push(current.Position);

                current = current.MyParent;
            }

            return finalPath;
        }

        return null;

        
    }

    public void Reset()
    {
        AStarDebugger.MyInstance.Reset(allNodes);


        foreach (Vector3Int position in changedTiles)
        {
            tilemap.SetTile(position, tiles[3]);
        }

        foreach (Vector3Int position in path)
        {
            tilemap.SetTile(position, tiles[3]);
        }

        tilemap.SetTile(startPos, tiles[3]);
        tilemap.SetTile(goalPos, tiles[3]);

        waterTiles.Clear();
        allNodes.Clear();

        start = false;
        goal = false;
        path = null;
        current = null;


    }



}
