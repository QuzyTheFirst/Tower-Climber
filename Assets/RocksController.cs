using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocksController : MonoBehaviour
{
    [Header("Rocks Controller")]
    [SerializeField] private Transform _rocksParent;

    [Header("Spawn Position")]
    [SerializeField] private float _spawningHeight = 10f;
    [SerializeField] private float _spawnDistanceFromTowerCenter;
    [SerializeField] private PlayerController _playerController;

    [Header("Timings")]
    [SerializeField] private float _minTimeBetweenFalls;
    [SerializeField] private float _maxTimeBetweenFalls;
    [SerializeField] private float _minFallingSpeed;
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private float _destroyRocksAfter = 5f;

    [SerializeField] private int _everyNRockFallOnPlayer = 4;
    private int _currentFallenRockNumber;

    [Header("Prefabs")]
    [SerializeField] private Transform[] _rocksPfs;

    private List<Transform> _spawnedRocks;

    private void OnEnable()
    {
        Rock.OnRockHitPlayer += Rock_OnRockHitPlayer;
    }

    private void OnDisable()
    {
        Rock.OnRockHitPlayer -= Rock_OnRockHitPlayer;
    }

    private void Awake()
    {
        _spawnedRocks = new List<Transform>();

        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        while (true)
        {
            _currentFallenRockNumber++;
            if(_currentFallenRockNumber % _everyNRockFallOnPlayer == 0)
            {
                SpawnRockOnPlayer();
            }
            else
            {
                SpawnRockRandomly();
            }

            float randomTime = Random.Range(_minTimeBetweenFalls, _maxTimeBetweenFalls);
            yield return new WaitForSeconds(randomTime);
        }
    }

    private void SpawnRockRandomly()
    {
        Transform randomRock = _rocksPfs[Random.Range(0, _rocksPfs.Length)];

        float randomDegree = Random.Range(0, 360f);
        float randomSpeed = Random.Range(_minFallingSpeed, _maxFallingSpeed);

        Quaternion randomRotation = Quaternion.Euler(0, randomDegree, 0f);

        Vector3 spawnPosition = randomRotation * Vector3.forward * _spawnDistanceFromTowerCenter + Vector3.up * _spawningHeight;

        Transform rock = Instantiate(randomRock, spawnPosition, Quaternion.Euler(0, randomDegree, 0));
        rock.parent = _rocksParent;

        rock.GetComponent<Rigidbody>().velocity = Vector3.down * randomSpeed;

        Destroy(rock.gameObject, _destroyRocksAfter);

        _spawnedRocks.Add(rock);
    }

    private void SpawnRockOnPlayer()
    {
        Transform randomRock = _rocksPfs[Random.Range(0, _rocksPfs.Length)];

        float randomSpeed = Random.Range(_minFallingSpeed, _maxFallingSpeed);

        Vector3 dirToPlayer = (_playerController.transform.position - transform.position).normalized;
        float degree = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, degree, 0);

        Vector3 spawnPosition = rotation * Vector3.forward * _spawnDistanceFromTowerCenter + Vector3.up * _spawningHeight;

        Transform rock = Instantiate(randomRock, spawnPosition, Quaternion.Euler(0, degree, 0));
        rock.parent = _rocksParent;

        rock.GetComponent<Rigidbody>().velocity = Vector3.down * randomSpeed;

        Destroy(rock.gameObject, _destroyRocksAfter);

        _spawnedRocks.Add(rock);
    }

    private void Rock_OnRockHitPlayer(object sender, System.EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _spawningHeight);
        Gizmos.DrawLine(transform.position + Vector3.up * _spawningHeight, transform.position + Vector3.up * _spawningHeight + Vector3.right * _spawnDistanceFromTowerCenter);
    }
}
