using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private CostumeSwapper _costumeSwapper;
    [SerializeField] private Transform _cameraLookAtObject;
    [SerializeField] private Transform _cameraPosition;

    public CostumeSwapper CostumeSwapper { get { return _costumeSwapper; } }
    public Transform CameraLookAtObject { get { return _cameraLookAtObject; } }
    public Vector3 CameraPosition { get { return _cameraPosition.position; } }
}
