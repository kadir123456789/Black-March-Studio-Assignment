using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BFS : MonoBehaviour
{
    public Node sourceNode;
    public Node destinationNode;
    public GridManager gridManager;
    [SerializeField] Player player;
    [SerializeField] MoveOnPoints moveOnPoints;
    



    public List<Node> graph = new List<Node>();

    public List<GameObject> shorestPathVertices = new List<GameObject>();


    List<Node> queueList = new List<Node>();


    List<GameObject> shorestPathEdges = new List<GameObject>();

    
    public int prevNodeIndex = 0;

    private void OnEnable()
    {
        Node.nodeSpawned += addNodeToGraph;
    }
    private void OnDisable()
    {
        Node.nodeSpawned -= addNodeToGraph;

    }

    private void addNodeToGraph(Node arg0)
    {
        graph.Add(arg0);
    }

    void Start()
    {
        Application.targetFrameRate = 120;

        sourceNode = gridManager.grid[0,0]; // sry it was hardcoded.

     

    }

  

     [ContextMenu("Find Shortest Path")]

    public void Find()
    {
        if(sourceNode != null && destinationNode != null)
        {
            FindShortestPath(sourceNode, destinationNode);

        }
        else
        {
            Debug.LogWarning("Source or Destination is Null");
        }
    }

    public void FindShortestPath(Node src, Node destination)
    {
    
        for(int i = 0; i < shorestPathEdges.Count; i++)
        {
            Destroy(shorestPathEdges[i]);
        }

        ResetGraph();


        queueList.Add(src);
        queueList[0].visited = true;


        while (queueList.Count > 0)
        {
            foreach (Node node in queueList[0].myAdjacentNodeList)
            {
                if (!node.visited)
                {

                    node.visited = true;
                    queueList.Add(node);
                    node.parentNode = queueList[0];

                }
            }
            Debug.Log(queueList[0].name);
            queueList.RemoveAt(0);
        }

        Node currentNode = destination;



        shorestPathEdges = new List<GameObject>();
        shorestPathVertices.Clear();
        shorestPathVertices = new List<GameObject>();

        while (currentNode != null)
        {

            shorestPathVertices.Add(currentNode.gameObject);

            

            if (currentNode.parentNode == null)
            {
                if(src != currentNode)
                {
                    Debug.LogWarning("No path found Bro, restart needed");
                    player.enabled = false;
                    
                    return;

                }
            }
            currentNode = currentNode.parentNode;
        }


        shorestPathVertices.Reverse();

        moveOnPoints.SetWayPoints(shorestPathVertices);

       

        

    }

    

    


    

   

    private void ResetGraph()
    {
        foreach (Node node in graph)
        {
            node.visited = false;
            node.parentNode = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
