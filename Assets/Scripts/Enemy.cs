using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyController enemyController;
    private float speed;
    private Grid grid;
    private Rigidbody2D rb;
    private Vector2Int moveDirection;
    private Vector2Int currentGridPos;
    private Vector2Int targetGridPos;
    private Vector2 targetWorldPos;
    // private List<Vector2> neighbourWorld;


    private void SetRandomDirection()
    {
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                moveDirection = Vector2Int.up;
                break;
            case 1:
                moveDirection = Vector2Int.right;
                break;
            case 2:
                moveDirection = Vector2Int.down;
                break;
            case 3:
                moveDirection = Vector2Int.left;
                break;
            default:
                Debug.Log("that's the wrong direction !!SIKEE!!");
                break;
        }
        Debug.Log(moveDirection);
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     //TODO: add layer or tag
    //     if (other.gameObject.tag == "Unwalkable")
    //     {
    //         targetGridPos -= 2 * moveDirection;//reverse direction if hit a wall
    //     }
    // }

    private void SetNextNeighbour()
    {
        List<Vector2Int> neighbourWorldPositions = new List<Vector2Int>();
        Debug.Log("A");
        neighbourWorldPositions = grid.GetWalkableNeighboursWorldPositions(currentGridPos);
        Debug.Log("B: " + neighbourWorldPositions.Count);
        //select random neighbour
        if (neighbourWorldPositions.Count > 0)
            return;

        int rand = Random.Range(0, neighbourWorldPositions.Count);

        moveDirection = Vector2Int.RoundToInt(neighbourWorldPositions[rand] - (Vector2)transform.position);


        UpdateTargetPos();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //refactor this function for better clarity
    private void UpdateTargetPos() //calculates from a node position
    {
        targetGridPos = currentGridPos + new Vector2Int(moveDirection.x, moveDirection.y);
        targetWorldPos = GetTargetWorldPos();
        // Debug.Log(rb.position);
    }

    private Vector2 GetTargetWorldPos()
    {
        return grid.GetWorldPositionFromNodePosition(targetGridPos);
    }

    private void Move()
    {
        // Debug.Log("pos: " + transform.position);
        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(currentPosition, targetWorldPos, speed * Time.fixedDeltaTime);

        if ((Vector2)transform.position == targetWorldPos)
        {
            Debug.Log((Vector2)transform.position);
            ReachedNode();
        }
    }

    private void ReachedNode()
    {
        SetNextNeighbour();
    }

    public void Init(Vector2Int startPos, float _speed, EnemyController _enemyController)
    {
        grid = FindObjectOfType<Grid>();
        rb = GetComponent<Rigidbody2D>();
        enemyController = _enemyController;
        currentGridPos = startPos;
        transform.position = grid.GetWorldPositionFromNodePosition(startPos);
        Random.InitState(enemyController.seed);
        speed = _speed;
        SetNextNeighbour();
    }
}
