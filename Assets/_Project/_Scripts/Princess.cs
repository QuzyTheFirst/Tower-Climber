using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour
{
    [SerializeField] private Transform _visuals;
    [SerializeField] private ParticleSystem _heartExplosion;

    public void ToggleVisuals(bool value)
    {
        _visuals.gameObject.SetActive(value);
    }

    public void HeartExplosion()
    {
        _heartExplosion.Play();
    }
}
