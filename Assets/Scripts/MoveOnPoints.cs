using System.Collections.Generic;
using UnityEngine;

public class MoveOnPoints : MonoBehaviour
{
    public List<GameObject> waypoints;
    private int currentWaypointIndex = 0;
    private bool hasReachedDestination = false;
    public float moveSpeed = 3f;
    public GameObject toMove;

    public void SetWayPoints(List<GameObject> waypoints)
    {
        this.waypoints = waypoints;
        currentWaypointIndex = 0;
        hasReachedDestination = false;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0 || hasReachedDestination) return;

        toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * moveSpeed);

        if (toMove.transform.position == waypoints[currentWaypointIndex].transform.position)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                // Reached the final destination
                Debug.Log($"{gameObject.name} reached the desti.");
                hasReachedDestination = true;
                currentWaypointIndex = 0;

                OnReachDestination();
            }
        }
    }

    private void OnReachDestination()
    {
        ICharacter character = toMove.GetComponent<ICharacter>();
        if(character != null)
        {
            character.OnReachDestination();
        }
    }
}
