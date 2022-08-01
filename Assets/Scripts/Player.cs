using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask walkableLayers;
    // [SerializeField] private GameObject bombPrefab;
    [Space]
    [Header("Bomb Settings")]
    [SerializeField] private float boomTime;
    [SerializeField] private GameObject bombObj;
    [SerializeField] private Transform bombParentTransform;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Vector2Int playerPositionInGrid;
    private Vector2Int bombPositionInGrid;
    private bool isBombPlaced;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (grid)
        {
            // Debug.Log("Grid not null");
            InitPlayer();
        }
    }

    private void InitPlayer()
    {
        transform.position = grid.grid[0, 0].transform.position;
        isBombPlaced = false;
    }
    // Update is called once per frame
    void Update()
    {
        MovementInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBomb();
        }
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.25f, walkableLayers);
        // Debug.Log(col);
        if (col.tag == "Tile")
        {
            playerPositionInGrid = col.gameObject.GetComponent<Node>().positionInGrid;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Tile")
    //     {
    //         playerPositionInGrid = other.gameObject.GetComponent<Node>().positionInGrid;
    //         // Debug.Log("Player Pos in grid: " + playerPositionInGrid);
    //     }
    // }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(transform.position, 0.25f);
    // }
    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SetMoveDirection(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            SetMoveDirection(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            SetMoveDirection(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            SetMoveDirection(Vector2.down);
        }
        else
        {
            SetMoveDirection(Vector2.zero);
        }
    }

    private void SetMoveDirection(Vector2 _moveDirection)
    {
        moveDirection = _moveDirection;
    }

    private void Move()
    {
        Vector2 currentPosition = rb.position;
        Vector2 translation = moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(currentPosition + translation);
    }

    private void DropBomb()
    {
        if (isBombPlaced)
            return;

        isBombPlaced = true;
        bombPositionInGrid = playerPositionInGrid;
        // Debug.Log("Bomb: " + bombPositionInGrid);
        // bombObj = Instantiate(bombPrefab, bombParentTransform);
        bombObj.SetActive(true);
        bombObj.transform.position = grid.grid[playerPositionInGrid.x, playerPositionInGrid.y].transform.position;
        StartCoroutine(ExplodeBombAfterDelay());
    }

    private IEnumerator ExplodeBombAfterDelay()
    {
        yield return new WaitForSeconds(boomTime);
        grid.BombGridAt(bombPositionInGrid);
        isBombPlaced = false;
    }
}
