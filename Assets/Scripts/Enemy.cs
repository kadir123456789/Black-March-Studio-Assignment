using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    private Node targetNode;
    public BFS bfs;
    public GridManager gridManager;
    public MoveOnPoints moveOnPoints;
    public Player player;

    public Node prevNode;
    private Color originalColor;

    void Start()
    {
        prevNode = gridManager.grid[9, 9];
        prevNode.isActive = false;
    }

    

    

    public void OnReachDestination()
    {
        Debug.Log("enemy reached the destination.");

        prevNode.isActive = true; //previous source node is set to active.

        prevNode = targetNode; // previous node = current node.

        player.prevNode.NodeActivate();
        player.prevNode.isActive = false; // such that cannot click on the node in which player is.
        
        prevNode.NodeDeactivate();

        targetNode.GetComponent<Renderer>().material.color = originalColor;

        player.OnEnemyTurnEnd();





    }

    public void OnPlayerReachedDestination()
    {

        bfs.sourceNode = prevNode;

        ChooseAdjacentNode();


        moveOnPoints.toMove = gameObject;

        bfs.Find();
    }

    private void ChooseAdjacentNode()
    {
        if (bfs.destinationNode == null || gridManager == null)
        {
            Debug.LogWarning("Destination node or GridManager is null.");
            return;
        }

        Vector2 playerGridPosition = bfs.destinationNode.nodePos;

        List<Node> possibleNodes = new List<Node>();

        // since enemy has to go to only 4 adjacent tiles
        Vector2[] directions = new Vector2[]
        {
            new Vector2Int(0, 1),   
            new Vector2Int(0, -1),  
            new Vector2Int(-1, 0),  
            new Vector2Int(1, 0)    
        };

        foreach (var dir in directions)
        {
            Vector2 adjacentPosition = playerGridPosition + dir;
            Node adjacentNode = gridManager.GetNodeAtPosition((int)adjacentPosition.x, (int)adjacentPosition.y);

            if (adjacentNode != null && adjacentNode.isActive)
            {
                possibleNodes.Add(adjacentNode);
            }
        }

        if (possibleNodes.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleNodes.Count);
            targetNode = possibleNodes[randomIndex];
            bfs.destinationNode = targetNode;

            Renderer cubeRenderer = targetNode.GetComponent<Renderer>();
            originalColor = cubeRenderer.material.color;
            cubeRenderer.material.color = Color.yellow;

        }
        else
        {
            Debug.Log("None of the 4 adjacent tiles available.");
        }
    }
}
