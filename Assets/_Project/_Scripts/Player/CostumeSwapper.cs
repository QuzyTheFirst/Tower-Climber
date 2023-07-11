using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeSwapper : MonoBehaviour
{
    private Transform _currentCostume;

    public void ChangeCostume(Transform costumeTf)
    {
        if (_currentCostume != null)
            Destroy(_currentCostume.gameObject);

        _currentCostume = Instantiate(costumeTf, transform);
    }
}
