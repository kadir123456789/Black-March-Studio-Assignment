using UnityEngine;


[CreateAssetMenu(fileName = "ObstacleData", menuName = "ObstacleData", order = 1)]
public class ObstacleData : ScriptableObject
{
    //public bool[] bools = new bool[4];
    public bool[,] nodeStates = new bool[10, 10];


  

    public bool GetNodeState(int x, int y)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            return nodeStates[x, y];
        }
        return false;

    }

    public void SetNodeState(int x, int y, bool isActive)
    {
        if (x >= 0 && x < 10 && y >= 0 && y < 10)
        {
            nodeStates[x, y] = isActive;
        }
    }
}
