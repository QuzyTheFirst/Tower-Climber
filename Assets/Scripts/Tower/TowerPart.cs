using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerPart : MonoBehaviour
{
    public static event EventHandler OnPlayerHitTopPart;

    private BoxCollider _boxCollider;

    [SerializeField] private GameObject[] _coinCollumns;
    private static int _chanceToSpawnCoins = 4;

    public BoxCollider BoxCollider { get { return _boxCollider; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        foreach(GameObject collumn in _coinCollumns)
        {
            int randomNumber = Random.Range(0, _chanceToSpawnCoins);
            bool spawnCoins = randomNumber == 0 ? true : false;
            collumn.SetActive(spawnCoins);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerHitTopPart?.Invoke(this, EventArgs.Empty);
        }
    }
}
