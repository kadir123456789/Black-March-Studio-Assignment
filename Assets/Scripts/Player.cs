using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] private BFS bfs;
    public Enemy enemy;
    public MoveOnPoints moveOnPoints;

    public Node prevNode;

    public bool isPlayerTurn = true;
    void Start()
    {
        prevNode = enemy.gridManager.grid[0, 0];
        prevNode.isActive = false;
    }

   

    public void OnReachDestination()
    {
        Debug.Log("player reached the destination.");
        prevNode.isActive = true; 
        prevNode = bfs.destinationNode; //previous node = current node 

        prevNode.NodeDeactivate();
        enemy.prevNode.NodeActivate();

       // bfs.destinationNode = moveOnPoints.waypoints[moveOnPoints.waypoints.Count - 1].GetComponent<Node>();
       // Debug.Log(bfs.destinationNode.nodePos + " bfs desti");
        
        enemy.OnPlayerReachedDestination();
    }

    public void OnEnemyTurnEnd()
    {
        isPlayerTurn = true;
        bfs.sourceNode = prevNode;
       // SetWayPoints(bfs.shorestPathVertices);
    }
}
