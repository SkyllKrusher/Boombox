using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int seed = 0;
    [Space]
    [SerializeField] private GameObject tile;
    [SerializeField] private Sprite nonDestructibleTileSprite;
    [SerializeField] private Sprite destructibleTileSprite;
    [SerializeField] private Sprite walkableTileSprite;
    private Node[,] grid;

    // [ExecuteInEditMode]
    private void OnValidate()
    {
        gridSize.x = Mathf.Clamp(gridSize.x, 0, 30);
        gridSize.y = Mathf.Clamp(gridSize.y, 0, 30);
        grid = new Node[gridSize.x, gridSize.y];
        transform.position = new Vector3(-gridSize.x / 2f, +gridSize.y / 2f, transform.position.z);
    }

    private void Start()
    {
        PopulateGrid();
    }

    private void PopulateGrid()
    {
        Random.InitState(seed);
        for (int j = 0; j < gridSize.y; j++)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                Vector2 nodeWorldPosition = new Vector2(transform.position.x + i + 0.5f, transform.position.y - j - 0.5f);
                GameObject newNodeObj = Instantiate(tile, this.transform);
                newNodeObj.transform.position = nodeWorldPosition;
                grid[i, j] = newNodeObj.GetComponent<Node>();
                // newNodeObj.GetComponent<Node>().position = new Vector2(i, j);
                grid[i, j].position = new Vector2(i, j);
                if (i % 2 != 0 && j % 2 != 0)
                {
                    MakeTileNondestructible(i, j);
                }
                else
                {
                    float r = Random.value;
                    Debug.Log("random: " + r);
                    if (r < 0.5f)
                    {
                        MakeTileDestructible(i, j);
                    }
                    else
                    {
                        MakeTileWalkable(i, j);
                    }
                }
                newNodeObj.name = "Tile (" + i + ", " + j + ")";
            }
        }
    }
    private void MakeTileNondestructible(int row, int column)
    {
        grid[row, column].nodeState = NodeState.NONDESTRUCTABLE;
        grid[row, column].spriteRenderer.sprite = nonDestructibleTileSprite;
    }

    private void MakeTileDestructible(int row, int column)
    {
        grid[row, column].nodeState = NodeState.DESTRUCTABLE;
        grid[row, column].spriteRenderer.sprite = destructibleTileSprite;
    }

    private void MakeTileWalkable(int row, int column)
    {
        grid[row, column].nodeState = NodeState.WALKABLE;
        grid[row, column].spriteRenderer.sprite = walkableTileSprite;
    }

    private void PopulateGridGizmos()
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                Vector2 nodePosition = new Vector2(transform.position.x + j + 0.5f, transform.position.y - i - 0.5f);
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
