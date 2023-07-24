using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeSwapper : MonoBehaviour
{
    private Costume _currentCostume;

    public Costume CurrentCostume { get { return _currentCostume; } }

    public void ChangeCostume(Costume costumeTf)
    {
        if (_currentCostume != null)
            Destroy(_currentCostume.gameObject);

        _currentCostume = Instantiate(costumeTf, transform);
    }
}
