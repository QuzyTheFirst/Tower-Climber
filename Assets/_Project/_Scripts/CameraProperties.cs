using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Transform _cameraLookAtTransform;

    public Transform CameraPositionTf { get { return _cameraPosition; } }
    public Transform CameraLookAtTransform { get { return _cameraLookAtTransform; } }
}
