using UnityEngine;
using TMPro;

public class Selector : MonoBehaviour
{
    public Color hoverColor = Color.green;
    public BFS bfs;
    private GameObject selectedCube; 
    private Color originalColor; 
    private Player player;
    public TextMeshProUGUI textMeshPro;


    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!player.isPlayerTurn) return; // disable input if it's not the player's turn

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.CompareTag("SelectableCube"))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (selectedCube != hitObject)
            {
                if (selectedCube != null)
                {
                    ResetCubeColor();
                }

                selectedCube = hitObject; //such that material one time update is done if remained hovered on the same cube.

                Renderer cubeRenderer = selectedCube.GetComponent<Renderer>();

                if (selectedCube.GetComponent<Node>().isActive == false)
                {

                    originalColor = cubeRenderer.material.color;
                    cubeRenderer.material.color = Color.red;


                }
                else
                {
                    originalColor = cubeRenderer.material.color;
                    cubeRenderer.material.color = hoverColor;


                }

                textMeshPro.text = hitObject.GetComponent<Node>().nodePos.ToString();




            }

            if (Input.GetMouseButtonDown(0) && selectedCube.GetComponent<Node>().isActive)
            {

                Node destinationNode = hitObject.GetComponent<Node>();
                bfs.destinationNode = destinationNode;

                player.moveOnPoints.toMove = player.gameObject;
                bfs.Find();

                player.isPlayerTurn = false;

            }




        }
        else
        {
            // if the raycast doesn't hit anything reset the previous cube color
            if (selectedCube != null)
            {
                ResetCubeColor();
            }
        }
    }

    void ResetCubeColor()
    {
        Renderer cubeRenderer = selectedCube.GetComponent<Renderer>();
        cubeRenderer.material.color = originalColor;
        selectedCube = null;
    }

    /*public void OnEnemyTurnEnd()
    {
        player.isPlayerTurn = true; 
    }*/
}
