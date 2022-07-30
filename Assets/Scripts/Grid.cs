using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    private Node[,] grid;

    // [ExecuteInEditMode]
    private void OnValidate()
    {
        gridSize.x = Mathf.Clamp(gridSize.x, 0, 30);
        gridSize.y = Mathf.Clamp(gridSize.y, 0, 30);
        grid = new Node[gridSize.x, gridSize.y];
    }

    private void Start()
    {

    }

    private void PopulateGridGizmos()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Vector2 nodePosition = new Vector2(transform.position.x + i + 0.5f, transform.position.y - j - 0.5f);
                // grid[i, j] = new Node(NodeState.NONDESTRUCTABLE, nodePosition);
                Gizmos.DrawWireSphere(nodePosition, 0.5f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 gridGizmoSize = new Vector3(gridSize.x, gridSize.y, 0);
        Vector3 centerPos = new Vector3(transform.position.x + gridSize.x / 2f, transform.position.y - gridSize.y / 2f, 0);
        // Vector3 centerPos = transform.position;
        Gizmos.DrawWireCube(centerPos, gridGizmoSize);
        PopulateGridGizmos();
    }
}
