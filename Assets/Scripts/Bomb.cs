using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombState
{
    POCKET,
    TICKING,
    EXPLODING
}
public class Bomb : MonoBehaviour
{
    // [SerializeField] private float boomTime;

    // public void StartBomb(Vector2Int bombPos)
    // {
    //     bombPositionInGrid = bombPos;
    //     StartCoroutine(ExplodeAfterWait());
    // }

    // private IEnumerator ExplodeAfterWait()
    // {
    //     yield return new WaitForSeconds(boomTime);
    // }
    [SerializeField] private float boomTime;
    private Vector2Int bombPositionInGrid;
    private Grid grid;
    private Animator animator;
    private static string Explosion_3x3_Animation = "Explosion_3x3";
    private BombState bombState;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        animator = GetComponent<Animator>();
        bombState = BombState.POCKET;
    }

    private void Start()
    {
    }

    private void Explode()
    {
        Debug.Log("booom!!");
        grid.BombGridAt(bombPositionInGrid);
        bombState = BombState.EXPLODING;
        animator.Play(Explosion_3x3_Animation);
    }

    public void Place(Vector2Int _bombPosinGrid)
    {
        gameObject.SetActive(true);
        bombPositionInGrid = _bombPosinGrid;
        transform.position = grid.GetWorldPositionFromNodePosition(bombPositionInGrid);
        bombState = BombState.TICKING;
        StartCoroutine(ExplodeBombAfterDelay());
    }

    private IEnumerator ExplodeBombAfterDelay()
    {
        yield return new WaitForSeconds(boomTime);
        Explode();

        float bombAnimationTime = 2f;
        yield return new WaitForSeconds(bombAnimationTime);
        bombState = BombState.POCKET;
        gameObject.SetActive(false);
    }

    public bool IsBomBPlaced()
    {
        if (bombState == BombState.POCKET)
        {
            return false;
        }
        return true;
    }

    // private void Update()
    // {
    //     if (bombState == BombState.Exploding)
    //     {

    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Player")
    //     {
    //         if (bombState == BombState.Exploding)
    //         {
    //             Debug.Log("ded :p");
    //         }
    //     }
    // }
}
