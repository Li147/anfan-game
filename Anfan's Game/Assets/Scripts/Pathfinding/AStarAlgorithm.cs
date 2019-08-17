using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarAlgorithm : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Stack<Vector3> path;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private List<Vector3Int> waterTiles = new List<Vector3Int>();

    private bool start, goal;

    public Tilemap MyTilemap { get => tilemap; set => tilemap = value; }


    // the main A star algorithm
    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = MyTilemap.WorldToCell(start);
        goalPos = MyTilemap.WorldToCell(goal);

        current = GetNode(startPos);

        // a list of nodes that we might want to look at later
        openList = new HashSet<Node>();

        // a list of nodes that we have examined
        closedList = new HashSet<Node>();

        openList.Add(current);

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.MyParent = null;
        }

        allNodes.Clear();

        path = null;


        // loop only runs until the path is not null
        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }

        if (path != null)
        {
            return path;
        }
        return null;

    }


    // Returns all neighbors of a node
    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1 ; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0)
                {

                    Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                    if (neighborPos != startPos && !waterTiles.Contains(neighborPos) && MyTilemap.GetTile(neighborPos)) // ensures that tiles outside of the tilemap are not added to neighbors
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
                    CalcValues(current, neighbor, goalPos, gScore);
                }
            }
            // if the neighbor is not on the open list
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, goalPos, gScore);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }

                
            }

        }
    }

    // prevents pathfinding from "cutting corners"
    private bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        // find the direction of the neighbor
        Vector3Int direct = currentNode.Position - neighbor.Position;

        Vector3Int first = new Vector3Int(currentNode.Position.x + (direct.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(currentNode.Position.x, currentNode.Position.y + (direct.y * -1), currentNode.Position.z);

        // checks if both nodes are empty
        if (GameManager.MyInstance.Blocked.Contains(first) || GameManager.MyInstance.Blocked.Contains(second))
        {
            return false;
        }

        return true;


    }


    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Math.Abs(x - y) % 2 == 1)
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

    private Stack<Vector3> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3> finalPath = new Stack<Vector3>();

            while (current != null)
            {
                finalPath.Push(current.Position);

                current = current.MyParent;
            }

            return finalPath;
        }

        return null;
    }


    private void CalcValues(Node parent, Node neighbor, Vector3Int goalPos, int cost)
    {
        // set parent node
        neighbor.MyParent = parent;

        // calculate this node's g cost (parent g cost + what it costs to move to this node)
        neighbor.G = parent.G + cost;

        neighbor.H = ((Math.Abs((neighbor.Position.x - goalPos.x)) + Math.Abs((neighbor.Position.y - goalPos.y))) * 10);

        neighbor.F = neighbor.G + neighbor.H;
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

}

public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node MyParent { get; set; }
    public Vector3Int Position { get; set; }

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}
