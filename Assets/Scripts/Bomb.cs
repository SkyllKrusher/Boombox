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
    [SerializeField] private float boomTime;
    [SerializeField] private int explosionRadius;
    [SerializeField] private GameObject bombSpriteObj;
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private Transform explosionsParentTransform;
    [SerializeField] private float explosionAnimationTime;
    private Vector2Int bombPositionInGrid;
    private Grid grid;
    private Animator animator;
    private static string Explosion_Center = "CenterBoom";
    private static string Explosion_Side = "SideBoom";
    private BombState bombState;
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        animator = GetComponent<Animator>();
        bombState = BombState.POCKET;
    }


    private IEnumerator ExplodeBombAfterDelay()
    {
        yield return new WaitForSeconds(boomTime);
        Explode();

        yield return new WaitForSeconds(explosionAnimationTime);
        bombState = BombState.POCKET;
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        Debug.Log("booom!!");
        bombState = BombState.EXPLODING;
        grid.BombGridAt(bombPositionInGrid);
        // animator.Play(Explosion_3x3_Animation);
        bombSpriteObj.SetActive(false);
        // DoExplosionAnimations();

        ExplosionAnimationAtPosition(bombPositionInGrid, Vector2.zero, Vector2.zero, Explosion_Center);


        ExplodeAnimation(bombPositionInGrid, Vector2.up, explosionRadius);
        ExplodeAnimation(bombPositionInGrid, Vector2.down, explosionRadius);
        ExplodeAnimation(bombPositionInGrid, Vector2.left, explosionRadius);
        ExplodeAnimation(bombPositionInGrid, Vector2.right, explosionRadius);
    }

    private void ExplodeAnimation(Vector2Int position, Vector2 direction, int length)
    {
        if (length <= 0)
            return;

        position += new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));

        if (position.x < 0 || position.y < 0 || position.x >= grid.GridSize.x || position.y >= grid.GridSize.y)
            return;

        if (grid.grid[position.x, position.y].nodeState == NodeState.NONDESTRUCTIBLE)
            return;

        ExplosionAnimationAtPosition(position, direction, -direction / 2, Explosion_Side);
        ExplodeAnimation(position, direction, length - 1);
    }


    private IEnumerator DestroyObjAfterExplosionAnimation(GameObject obj)
    {
        yield return new WaitForSeconds(explosionAnimationTime);
        Destroy(obj);
    }

    private void ExplosionAnimationAtPosition(Vector2Int nodePos, Vector2 direction, Vector2 offset, string animationClipName)
    {
        //TODO: Object pool
        GameObject explosionObj = Instantiate(this.explosionObj, explosionsParentTransform);

        if (direction.x == 0)
        {
            direction *= -1;
            offset *= -1;
        }
        explosionObj.transform.position = grid.GetWorldPositionFromNodePosition(nodePos) + offset;
        float angle = Mathf.Atan2(direction.y, direction.x);
        explosionObj.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        explosionObj.GetComponent<Animator>().Play(animationClipName);

        StartCoroutine(DestroyObjAfterExplosionAnimation(explosionObj));
    }

    public void Place(Vector2Int _bombPosinGrid)
    {
        if (!(bombState == BombState.POCKET))
            return;

        gameObject.SetActive(true);
        bombSpriteObj.SetActive(true);
        bombState = BombState.TICKING;
        bombPositionInGrid = _bombPosinGrid;
        transform.position = grid.GetWorldPositionFromNodePosition(bombPositionInGrid);
        StartCoroutine(ExplodeBombAfterDelay());
    }

    public bool IsBombPlaced()
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
    // private void DoExplosionAnimations()
    // {
    //     //center expl
    //     List<Vector2Int> explosionPositions;
    //     //forlooop for side explos, change direction set position etc
    //     // for
    //     explosionPositions = grid.GetSideExplosionPositionsFromCenter(bombPositionInGrid);
    //     float rotation = 0f;
    //     Vector2 offset = Vector2.zero;
    //     Vector2Int center = bombPositionInGrid;
    //     // ExplosionAnimationAtPosition(center, rotation, offset, Explosion_Center);

    //     for (int i = 0; i < explosionPositions.Count; i++)
    //     {
    //         if (explosionPositions[i].y == center.y)
    //         {
    //             if (explosionPositions[i].x < center.x)
    //             {
    //                 offset = new Vector2(-0.5f, 0);
    //                 rotation = 180;
    //             }
    //             else
    //             {
    //                 offset = new Vector2(+0.5f, 0);
    //             }
    //         }
    //         else
    //         {
    //             offset = new Vector2(0, 0.5f);

    //             Debug.Log(explosionPositions[i].y + " != " + center.y);
    //             if (explosionPositions[i].y > center.y)
    //             {
    //                 rotation = -90;
    //             }
    //             else
    //             {
    //                 rotation = 90;
    //             }
    //         }

    //         // ExplosionAnimationAtPosition(explosionPositions[i], rotation, offset, Explosion_Side);
    //     }
    // }
}
