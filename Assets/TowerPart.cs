using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPart : MonoBehaviour
{
    [SerializeField] private LayerMask _playerMask;

    private TowerController _towerController;

    public void Initialize(TowerController towerController)
    {
        _towerController = towerController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            _towerController.SpawnRandomPart();
        }
    }
}
