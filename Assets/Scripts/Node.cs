using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Node : MonoBehaviour
{
    public Vector2 nodePos;
    public static UnityAction<GameObject> nodeClicked;
    public static UnityAction<Node> nodeSpawned;
    public bool isActive = false;
    public List<Node> myAdjacentNodeList;
    public Node parentNode = null;
    public bool visited = false;
    private Stack<(Node, Node)> edgeRemovalStack = new Stack<(Node, Node)>();

    // Start is called before the first frame update


    void Start()
    {
        nodeSpawned?.Invoke(this);

        
    }
    [ContextMenu("deactivate")]
    public void NodeDeactivate()
    {
        isActive = false;
       // Node a = this;
        foreach (Node b in myAdjacentNodeList)
        {
            Node a = this;
            if (a.myAdjacentNodeList.Contains(b) && b.myAdjacentNodeList.Contains(a))
            {
                b.myAdjacentNodeList.Remove(a);                

                edgeRemovalStack.Push((a, b));
            }
            
           
        }
        myAdjacentNodeList.Clear();



    }
    [ContextMenu("active")]
    public void NodeActivate()
    {
        isActive = true;
        while (edgeRemovalStack.Count > 0)
        {
            var edge = edgeRemovalStack.Pop();
            Node a = edge.Item1;
            Node b = edge.Item2;

            // Re-add the edge between a and b
            if (!a.myAdjacentNodeList.Contains(b))
            {
                a.myAdjacentNodeList.Add(b);
            }

            if (!b.myAdjacentNodeList.Contains(a))
            {
                b.myAdjacentNodeList.Add(a);
            }
        }
    }

   

    private void OnEnable()
    {
        
    }


    

    
}
