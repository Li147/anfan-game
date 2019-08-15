using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();




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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Algorithm();
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






    private void Algorithm()
    {
        if (current == null)
        {
            Initialize();
        }

        List<Node> neighbors = FindNeighbors(current.Position);
        ExamineNeighbors(neighbors, current);


        UpdateCurrentTile(ref current);

        AStarDebugger.MyInstance.CreateTiles(openList, closedList, startPos, goalPos);
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
                    if (neighborPos != startPos && tilemap.GetTile(neighborPos)) // ensures that tiles outside of the tilemap are not added to neighbors
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
        for (int i = 0; i < neighbors.Count; i++)
        {
            openList.Add(neighbors[i]);

            CalcValues(current, neighbors[i], 0);
        }
    }


    private void CalcValues(Node parent, Node neighbor, int costs)
    {
        neighbor.MyParent = parent;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);
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
        
        if (tileType == TypeOfTile.START)
        {
            startPos = clickPos;
        }
        else if (tileType == TypeOfTile.GOAL)
        {
            goalPos = clickPos;
        }


        tilemap.SetTile(clickPos, tiles[(int)tileType]);
        


        
    }

}
