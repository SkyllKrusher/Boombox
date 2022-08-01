using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // [SerializeField] private float boomTime;
    // [SerializeField] private Vector2Int bombPositionInGrid;

    // public void StartBomb(Vector2Int bombPos)
    // {
    //     bombPositionInGrid = bombPos;
    //     StartCoroutine(ExplodeAfterWait());
    // }

    // private IEnumerator ExplodeAfterWait()
    // {
    //     yield return new WaitForSeconds(boomTime);
    // }

    [SerializeField] private Animator animator;
    private static string Explosion_3x3_Animation = "Explosion_3x3";

    public void Explode()
    {
        Debug.Log("booom!!");
        animator.Play(Explosion_3x3_Animation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

        }
    }
}
