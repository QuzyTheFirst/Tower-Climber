using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocksController : MonoBehaviour, IRestartable
{
    [Header("Rocks Controller")]
    [SerializeField] private Transform _rocksParent;

    [Header("Spawn Position")]
    [SerializeField] private float _spawningHeight = 10f;
    [SerializeField] private float _spawnDistanceFromTowerCenter;

    [Header("Timings")]
    [SerializeField] private int _startingAmountOfRocksPerSecond = 1;
    [SerializeField] private int _endingAmountOfRocksPerSecond = 4;

    [SerializeField, Tooltip("Time To Get To Ending Pace")] private float _timeToGetToEndingPace = 90;
    private float _rocksPerSecondChangerTimer = 0f;

    [SerializeField] private float _minFallingSpeed;
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private float _destroyRocksAfter = 5f;

    private int _currentAmountOfRocksPerSecond;
    private float _timeBetweenRocks;

    [Header("Prefabs")]
    [SerializeField] private Transform[] _rocksPfs;

    private List<Transform> _spawnedRocks;

    private int[] _rotationsToFall = {0, 45, 90, 135, 180, 225, 270, 315};
    private List<int> _randomizedRotationsToFall;
    private int _currentFallRotation;

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
        _randomizedRotationsToFall = new List<int>();

        _currentAmountOfRocksPerSecond = _startingAmountOfRocksPerSecond;
        _timeBetweenRocks = 1 / _currentAmountOfRocksPerSecond;

        RandomizeRotationsToFall();

        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        while (true)
        {
            _currentAmountOfRocksPerSecond = (int)Mathf.Lerp(_startingAmountOfRocksPerSecond, _endingAmountOfRocksPerSecond, _rocksPerSecondChangerTimer / _timeToGetToEndingPace);
            _timeBetweenRocks = 1 / (float)_currentAmountOfRocksPerSecond;

            SpawnRockRandomly();

            yield return new WaitForSeconds(_timeBetweenRocks);

            _rocksPerSecondChangerTimer += _timeBetweenRocks;
        }
    }

    private void SpawnRockRandomly()
    {
        Transform randomRock = _rocksPfs[Random.Range(0, _rocksPfs.Length)];

        float randomSpeed = Random.Range(_minFallingSpeed, _maxFallingSpeed);

        Quaternion randomRotation = GetNextFallingRotation();

        Vector3 spawnPosition = randomRotation * Vector3.forward * _spawnDistanceFromTowerCenter + Vector3.up * _spawningHeight;

        Transform rock = Instantiate(randomRock);
        rock.position = spawnPosition;
        rock.localRotation = Quaternion.Euler(0, randomRotation.eulerAngles.y, 0);
        rock.parent = _rocksParent;

        rock.GetComponent<Rigidbody>().velocity = Vector3.down * randomSpeed;

        Destroy(rock.gameObject, _destroyRocksAfter);

        _spawnedRocks.Add(rock);
    }

    private void RandomizeRotationsToFall()
    {
        _currentFallRotation = 0;
        _randomizedRotationsToFall.Clear();

        for (int i = 0; i < _rotationsToFall.Length; i++)
        {
            int number = Random.Range(0, _rotationsToFall.Length);
            while (_randomizedRotationsToFall.Contains(_rotationsToFall[number]))
            {
                number = Random.Range(0, _rotationsToFall.Length);
            }
            _randomizedRotationsToFall.Add(_rotationsToFall[number]);
        }
    }
    private Quaternion GetNextFallingRotation()
    {
        Quaternion rotation = Quaternion.Euler(0, _randomizedRotationsToFall[_currentFallRotation], 0);

        _currentFallRotation++;

        if (_currentFallRotation == _rotationsToFall.Length)
            RandomizeRotationsToFall();

        return rotation;
    }

    public void Restart()
    {
        foreach(Transform rock in _spawnedRocks)
        {
            if(rock != null)
                Destroy(rock.gameObject);
        }

        _spawnedRocks.Clear();
    }

    private void Rock_OnRockHitPlayer(object sender, System.EventArgs e)
    {
        GameManager.Instance.KillPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _spawningHeight);
        Gizmos.DrawLine(transform.position + Vector3.up * _spawningHeight, transform.position + Vector3.up * _spawningHeight + Vector3.right * _spawnDistanceFromTowerCenter);
    }
}
