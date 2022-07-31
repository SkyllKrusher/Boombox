using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region -------------------  Variables --------------------------
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int seed = 0;
    [Space]
    [SerializeField] private GameObject tile;
    [SerializeField] private Sprite nonDestructibleTileSprite;
    [SerializeField] private Sprite destructibleTileSprite;
    [SerializeField] private Sprite walkableTileSprite;
    [SerializeField] private Transform gridBoundary;
    public Node[,] grid;

    #endregion

    // [ExecuteInEditMode]

    private void OnValidate()
    {
        gridSize.x = Mathf.Clamp(gridSize.x, 0, 30);
        gridSize.y = Mathf.Clamp(gridSize.y, 0, 30);
        // grid = new Node[gridSize.x, gridSize.y];
        transform.position = new Vector3(-gridSize.x / 2f, +gridSize.y / 2f, transform.position.z);
        // PopulateGrid();
    }

    private void Awake()
    {
        CreateGrid();
        PopulateGrid();
        PopulateGridBoundary();
    }

    #region ------------------- Grid creation Methods ----------------------

    private void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];
        for (int j = 0; j < gridSize.y; j++)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                Vector2 nodeWorldPosition = new Vector2(transform.position.x + i + 0.5f, transform.position.y - j - 0.5f);

                GameObject newNodeObj = Instantiate(tile, this.transform);
                newNodeObj.transform.position = nodeWorldPosition;
                newNodeObj.name = "Tile (" + i + ", " + j + ")";

                grid[i, j] = newNodeObj.GetComponent<Node>();
                grid[i, j].position = new Vector2(i, j);

            }
        }
    }

    private void PopulateGrid()
    {
        Debug.Log("Populating grid");
        Random.InitState(seed);
        for (int j = 0; j < gridSize.y; j++)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                //Fixed indestructible tiles with spacing
                if (i % 2 != 0 && j % 2 != 0) { MakeTileNondestructible(i, j); }

                //Randomize all other as walkable or destroyable
                else
                {
                    float r = Random.value;
                    // Debug.Log("random: " + r);
                    if (r < 0.5f) { MakeTileDestructible(i, j); }
                    else { MakeTileWalkable(i, j); }
                }
            }
        }

        //Default walkable space in top left
        for (int i = 0; i < gridSize.x && i < 2; i++)
        {
            for (int j = 0; j < gridSize.y && j < 2; j++)
            {
                if (i % 2 == 0 || j % 2 == 0)
                    MakeTileWalkable(i, j);
            }
        }
    }
    private void PopulateGridBoundary()
    {
        for (int i = 0; i < gridSize.x + 2; i++)
        {
            for (int j = 0; j < gridSize.y + 2; j++)
            {
                if (i == 0 || j == 0 || i == gridSize.x + 1 || j == gridSize.y + 1)
                {
                    Vector2 tileWorldPosition = new Vector2(transform.position.x + i - 0.5f, transform.position.y - j + 0.5f);

                    GameObject newNodeObj = Instantiate(tile, gridBoundary.transform);
                    newNodeObj.transform.position = tileWorldPosition;
                    newNodeObj.GetComponent<Node>().spriteRenderer.sprite = nonDestructibleTileSprite;
                    newNodeObj.GetComponent<Node>().nodeState = NodeState.NONDESTRUCTABLE;
                }
            }
        }
    }
    #endregion

    #region --------------- Set Tile type methods ----------------------
    private void MakeTileNondestructible(int row, int column)
    {
        grid[row, column].nodeState = NodeState.NONDESTRUCTABLE;
        grid[row, column].spriteRenderer.sprite = nonDestructibleTileSprite;
        grid[row, column].GetComponent<Collider2D>().isTrigger = false;
    }

    private void MakeTileDestructible(int row, int column)
    {
        grid[row, column].nodeState = NodeState.DESTRICTABLE;
        grid[row, column].spriteRenderer.sprite = destructibleTileSprite;
        grid[row, column].GetComponent<Collider2D>().isTrigger = false;
    }

    private void MakeTileWalkable(int row, int column)
    {
        grid[row, column].nodeState = NodeState.WALKABLE;
        grid[row, column].spriteRenderer.sprite = walkableTileSprite;
        grid[row, column].GetComponent<Collider2D>().isTrigger = true;
    }
    #endregion

    #region ---------------------- Draw Gizmos Methods---------------------------------
    // private void PopulateGridGizmos()
    // {
    //     for (int i = 0; i < gridSize.y; i++)
    //     {
    //         for (int j = 0; j < gridSize.x; j++)
    //         {
    //             Vector2 nodePosition = new Vector2(transform.position.x + j + 0.5f, transform.position.y - i - 0.5f);
    //             // grid[i, j] = new Node(NodeState.NONDESTRUCTABLE, nodePosition);
    //             Gizmos.DrawWireSphere(nodePosition, 0.5f);
    //         }
    //     }
    // }

    private void OnDrawGizmos()
    {
        Vector3 gridGizmoSize = new Vector3(gridSize.x, gridSize.y, 0);
        Vector3 centerPos = new Vector3(transform.position.x + gridSize.x / 2f, transform.position.y - gridSize.y / 2f, 0);
        // Vector3 centerPos = transform.position;
        Gizmos.DrawWireCube(centerPos, gridGizmoSize);
        // PopulateGridGizmos();
    }
    #endregion

    public void DestroyTilesNear(int row, int column, int explosionRadius = 2)
    {
        for (int i = row - explosionRadius; i < row + explosionRadius && i < gridSize.x; i++)
        {
            for (int j = row - explosionRadius; j < column + explosionRadius && j < gridSize.y; j++)
            {
                if (grid[i, j].nodeState == NodeState.DESTRICTABLE)
                {
                    MakeTileWalkable(i, j);
                }
            }
        }
    }
}
