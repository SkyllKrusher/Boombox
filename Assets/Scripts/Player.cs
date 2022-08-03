using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask walkableLayers;
    [SerializeField] private Bomb bomb;
    [SerializeField] private float deathAnimationTime;
    // [SerializeField] private GameObject bombPrefab;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Vector2Int playerPositionInGrid;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (Input.GetKey(KeyCode.D))
        {
            SetMoveDirection(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SetMoveDirection(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            SetMoveDirection(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.S))
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

    //TODO: move some lines in Bomb methods to Bomb.cs script
    private void DropBomb()
    {
        if (bomb.IsBombPlaced())
            return;

        // Debug.Log("Bomb: " + bombPositionInGrid);

        bomb.Place(playerPositionInGrid);
    }

    private void DeathAnimation()
    {
        StartCoroutine(DestroyPlayerAfterDeathAnimations());
    }

    private IEnumerator DestroyPlayerAfterDeathAnimations()
    {
        yield return new WaitForSeconds(deathAnimationTime);
        gameObject.SetActive(false);
    }

    public void Init()
    {
        gameObject.SetActive(true);
        Vector2Int playerStartGridPos = new Vector2Int(0, 0);
        transform.position = grid.GetWorldPositionFromNodePosition(playerStartGridPos);
    }

    public void Death()
    {
        this.enabled = false;
        DeathAnimation();
    }


}
