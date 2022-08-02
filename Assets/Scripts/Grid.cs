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
    [SerializeField] private Sprite boundaryTileSprite;
    [SerializeField] private Sprite destructibleTileSprite;
    [SerializeField] private Sprite walkableTileSprite;
    [SerializeField] private Transform gridBoundary;
    public Node[,] grid;

    #endregion

    #region ------------------- Monobehaviour Methods ---------------
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
    #endregion

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
                grid[i, j].positionInGrid = new Vector2Int(i, j);

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
                    newNodeObj.GetComponent<Node>().spriteRenderer.sprite = boundaryTileSprite;
                    newNodeObj.GetComponent<Node>().nodeState = NodeState.NONDESTRUCTiBLE;
                }
            }
        }
    }
    #endregion

    #region --------------- Set Tile type methods ----------------------
    private void MakeTileNondestructible(int row, int column)
    {
        grid[row, column].nodeState = NodeState.NONDESTRUCTiBLE;
        grid[row, column].spriteRenderer.sprite = nonDestructibleTileSprite;
        grid[row, column].GetComponent<Collider2D>().isTrigger = false;
    }

    private void MakeTileDestructible(int row, int column)
    {
        grid[row, column].nodeState = NodeState.DESTRUCTIBLE;
        grid[row, column].spriteRenderer.sprite = destructibleTileSprite;
        grid[row, column].GetComponent<Collider2D>().isTrigger = false;
    }

    private void MakeTileWalkable(int row, int column)
    {
        grid[row, column].nodeState = NodeState.WALKABLE;
        grid[row, column].spriteRenderer.sprite = walkableTileSprite;
        grid[row, column].GetComponent<Collider2D>().isTrigger = true;
        grid[row, column].gameObject.layer = 6; //walkable layer
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

    // private void OnDrawGizmos()
    // {
    //     Vector3 gridGizmoSize = new Vector3(gridSize.x, gridSize.y, 0);
    //     Vector3 centerPos = new Vector3(transform.position.x + gridSize.x / 2f, transform.position.y - gridSize.y / 2f, 0);
    //     // Vector3 centerPos = transform.position;
    //     Gizmos.DrawWireCube(centerPos, gridGizmoSize);
    //     PopulateGridGizmos();
    // }
    #endregion

    public void BombGridAt(Vector2Int explosionCenter, int explosionRadius = 2)
    {
        int rowMin = Mathf.Max(explosionCenter.x - explosionRadius, 0);
        int columnMin = Mathf.Max(explosionCenter.y - explosionRadius, 0);
        int rowMax = Mathf.Min(explosionCenter.x + explosionRadius, gridSize.x - 1);
        int columnMax = Mathf.Min(explosionCenter.y + explosionRadius, gridSize.y - 1);
        // Debug.Log("row Min: " + rowMin + ", row max: " + rowMax + ", column min: " + columnMin + ", column max:" + columnMax + "\nTo destroy: ");
        for (int row = rowMin; row <= rowMax; row++)
        {
            for (int column = columnMin; column <= columnMax; column++)
            {
                // Debug.Log(row + ", " + column);
                if (row == explosionCenter.x || column == explosionCenter.y) //TODO: write more efficient loop
                {
                    if (grid[row, column].nodeState == NodeState.DESTRUCTIBLE)
                    {
                        MakeTileWalkable(row, column);
                    }
                }
            }
        }
    }

    public Vector2 GetWorldPositionFromNodePosition(Vector2Int nodePosition)
    {
        return grid[nodePosition.x, nodePosition.y].transform.position;
    }
}
