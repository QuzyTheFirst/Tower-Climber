using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerPart : MonoBehaviour
{
    public static event EventHandler OnPlayerHitTopPart;

    private BoxCollider _boxCollider;

    [SerializeField] private int _chanceToSpawnCoinsOneTo = 4;
    [SerializeField] private int _chanceToSpawnWindowOneTo = 4;

    [SerializeField] private GameObject[] _coinsCollumns;
    [SerializeField] private GameObject[] _windows;
    
    public BoxCollider BoxCollider { get { return _boxCollider; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        foreach(GameObject coins in _coinsCollumns)
        {
            float chanceToSpawn = 1f / _chanceToSpawnCoinsOneTo;
            bool spawnCoins = Random.value <= chanceToSpawn ? true : false;
            coins.SetActive(spawnCoins);
        }

        foreach (GameObject window in _windows)
        {
            float chanceToSpawn = 1f / _chanceToSpawnWindowOneTo;
            bool spawnWindow = Random.value <= chanceToSpawn ? true : false;
            window.SetActive(spawnWindow);
        }
    }

    private void Start()
    {
        int rotationValue = 45 * Random.Range(0, 8);
        Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0f);
        transform.localRotation = randomRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerHitTopPart?.Invoke(this, EventArgs.Empty);
        }
    }
}
