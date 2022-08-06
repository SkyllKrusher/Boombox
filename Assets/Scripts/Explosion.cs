using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;
    private Vector2 hitboxCenter;
    private LayerMask layerMask;

    //collider changing through animation
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.Death();
            Debug.Log("Player exploded");
        }
    }

    // private void Update()
    // {
    //     if (Physics2D.OverlapBox(hitboxCenter, new Vector2(0.8f, 0.8f), 0, layerMask))
    //     {
    //     }
    // }

    private IEnumerator DestroyObjAfter(float explosionAnimationTime)
    {
        yield return new WaitForSeconds(explosionAnimationTime);
        Destroy(gameObject);
    }

    public void DestroyObjAfterExplosion(float time)
    {
        StartCoroutine(DestroyObjAfter(time));
    }

    public void InitExplosion(Vector2 pos, Vector2 direction, Vector2 _hitboxCenter)
    {
        layerMask = LayerMask.GetMask("Player");
        animator = GetComponent<Animator>();
        transform.position = pos;
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        hitboxCenter = _hitboxCenter;

    }

    public void PlayExplosion(string explosionClip)
    {
        animator.Play(explosionClip);
    }
}
