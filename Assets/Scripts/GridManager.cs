using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Node nodePrefab; 
    public int gridSize = 10;     
    public float nodeSpacing = 1f;
    public Player player;
    public Enemy enemy;

    public Node[,] grid;
    [HideInInspector]
    public bool gridGenerated = false;

    void Awake()
    {
        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
        grid = new Node[gridSize, gridSize];
        GenerateGrid();
        ConnectNodesWithEdges();
        gridGenerated = true;

        player.transform.position = grid[0,0].gameObject.transform.position;
        enemy.transform.position = grid[9,9].gameObject.transform.position;
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 nodePosition = new Vector3(x * nodeSpacing, 0, y * nodeSpacing);

                Node newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);

                grid[x, y] = newNode;

                grid[x, y].nodePos = new Vector2(x, y); 
            }
        }
    }

    void ConnectNodesWithEdges()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Node currentNode = grid[x, y];

                // connect to the right node vertically
                if (x < gridSize - 1)
                {
                    CreateEdge(currentNode, grid[x + 1, y]); //0,0 -> 1,0 | 0,1->1,1
                }

                // connect to the upper node
                if (y < gridSize - 1)
                {
                    CreateEdge(currentNode, grid[x, y + 1]);
                }
            }
        }
    }

    public Node GetNodeAtPosition(int i, int j)
    {
        if (i >= 0 && i < 10 && j >= 0 && j < 10)
        {
            if(grid != null)
                return grid[i, j];
        }
        return null; 
    }

    void CreateEdge(Node a, Node b)
    {
        
        a.myAdjacentNodeList.Add(b);
        b.myAdjacentNodeList.Add(a);
    }


}
