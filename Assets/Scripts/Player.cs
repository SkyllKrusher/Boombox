using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private float speed;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private Vector2Int gridPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (grid)
        {
            Debug.Log("Grid not null");
            InitPlayer();
        }
    }

    private void InitPlayer()
    {
        transform.position = grid.grid[0, 0].transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

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
}
