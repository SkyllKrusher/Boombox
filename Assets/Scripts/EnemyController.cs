using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float moveSpeed;
    private List<Enemy> enemies;
    public int seed { get; private set; }

    private void CreateEnemy(Vector2Int enemyPos)
    {
        Enemy enemy = Instantiate(enemyPrefab, transform);
        enemy.Init(enemyPos, moveSpeed, this);
        enemies.Add(enemy);
    }

    public void Init()
    {
        enemies = new List<Enemy>();
        seed = 0;
        CreateEnemy(new Vector2Int(1, 0));
    }
}
