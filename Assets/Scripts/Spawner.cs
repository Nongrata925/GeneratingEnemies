using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _spawnPoints;
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: CreateEnemy,
            actionOnGet: (enemy) => ActivateEnemy(enemy),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }
    
    private void Start()
    {
        StartCoroutine(Spawn(_repeatRate));
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_prefab);
        
        return enemy;
    }

    private void ActivateEnemy(Enemy enemy)
    {
        int randomIndex = Random.Range(0, _spawnPoints.Length);
        SpawnPoint spawnPoint = _spawnPoints[randomIndex];
        
        enemy.transform.position = spawnPoint.transform.position;
        enemy.ReleaseZoneReached += ReleaseEnemy;
        enemy.Initialize(spawnPoint.SetMovementDirection());
        enemy.gameObject.SetActive(true);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        enemy.ReleaseZoneReached -= ReleaseEnemy;
        _pool.Release(enemy);
    }

    private IEnumerator Spawn(float delay)
    {
        bool isSpawned = true;
        
        while (isSpawned)
        {
            yield return new WaitForSeconds(delay);
        
            _pool.Get();
        }
    }
}