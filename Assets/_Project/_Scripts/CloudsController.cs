using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour
{
    [Header("Rocks Controller")]
    [SerializeField] private Transform _cloudsParent;

    [Header("Spawn Position")]
    [SerializeField] private float _spawningHeight = 10f;
    [SerializeField] private float _spawnDistanceFromTowerCenter;

    [Header("Properties")]
    [SerializeField] private float _minFloatingSpeed;
    [SerializeField] private float _maxFloatingSpeed;
    [SerializeField] private float _destroyCloudsAfter = 20f;

    [Header("Timings")]
    [SerializeField] private float _startingSpawningRatePerSecond = .1f;
    [SerializeField] private float _endingSpawningRatePerSecond = .3f;
    [SerializeField] private float _timeToGetToEndingPace = 90;

    private float _currentSpawningRate;
    private float _cloudsSpawningRateChangerTimer = 0f;

    [Header("Prefabs")]
    [SerializeField] private Transform[] _cloudsPfs;

    private List<Transform> _spawnedClouds;

    private void Awake()
    {
        _spawnedClouds = new List<Transform>();

        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            _currentSpawningRate = Mathf.Lerp(_startingSpawningRatePerSecond, _endingSpawningRatePerSecond, _cloudsSpawningRateChangerTimer / _timeToGetToEndingPace);
            float timeBetweenClouds = 1 / _currentSpawningRate;

            SpawnCloudRandomly();

            yield return new WaitForSeconds(timeBetweenClouds);

            _cloudsSpawningRateChangerTimer += timeBetweenClouds;
        }
    }

    private void SpawnCloudRandomly()
    {
        Transform randomCloud = _cloudsPfs[Random.Range(0, _cloudsPfs.Length)];

        float randomSpeed = Random.Range(_minFloatingSpeed, _maxFloatingSpeed);
        float randomRotation = Random.Range(0f, 360f);

        Quaternion cloudRotation = Quaternion.Euler(0, randomRotation, 0);

        Vector3 spawnPosition = cloudRotation * Vector3.forward * _spawnDistanceFromTowerCenter + Vector3.up * _spawningHeight;

        Transform cloud = Instantiate(randomCloud);
        cloud.position = spawnPosition;
        cloud.localRotation = Quaternion.Euler(90, cloudRotation.eulerAngles.y, 0);
        cloud.parent = _cloudsParent;

        int cloudMovementDirection = (int)Mathf.Sign(Random.Range(-1f, 1f));

        cloud.GetComponent<Rigidbody>().velocity = cloudMovementDirection * cloud.transform.right * randomSpeed;

        Destroy(cloud.gameObject, _destroyCloudsAfter);

        _spawnedClouds.Add(cloud);
    }

    public void Restart()
    {
        foreach (Transform cloud in _spawnedClouds)
        {
            if (cloud != null)
                Destroy(cloud.gameObject);
        }

        _currentSpawningRate = _startingSpawningRatePerSecond;
        _cloudsSpawningRateChangerTimer = 0;

        _spawnedClouds.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _spawningHeight);
        Gizmos.DrawLine(transform.position + Vector3.up * _spawningHeight, transform.position + Vector3.up * _spawningHeight + Vector3.right * _spawnDistanceFromTowerCenter);
    }
}
