using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;

    [SerializeField] CameraProperties _shop;
    [SerializeField] CameraProperties _tower;
    [SerializeField] CameraProperties _playerFall;

    [SerializeField] private float _timeToChangeCameraPosition = 1f;
    [SerializeField] private float _timeToChangeCameraLookAtTransform = 1f;

    public CameraProperties Shop { get { return _shop; } }
    public CameraProperties Tower { get { return _tower; } }
    public CameraProperties PlayerFall { get { return _playerFall; } }

    public void SwitchCameraToMainMenu()
    {
        LeanTween.cancel(_camera.gameObject);
        LeanTween.cancel(_camera.LookAt.gameObject);

        LeanTween.move(_camera.gameObject, Tower.CameraPositionTf.position, _timeToChangeCameraPosition).setIgnoreTimeScale(true);
        LeanTween.move(_camera.LookAt.gameObject, Tower.CameraLookAtTransform.position, _timeToChangeCameraLookAtTransform).setIgnoreTimeScale(true);
    }

    public void SwitchCameraToCostumeShop()
    {
        LeanTween.cancel(_camera.gameObject);
        LeanTween.cancel(_camera.LookAt.gameObject);

        Debug.Log(_camera.transform.position + " | " + Shop.CameraPositionTf.position);

        LeanTween.move(_camera.gameObject, Shop.CameraPositionTf.position, _timeToChangeCameraPosition).setIgnoreTimeScale(true);
        LeanTween.move(_camera.LookAt.gameObject, Shop.CameraLookAtTransform.position, _timeToChangeCameraLookAtTransform).setIgnoreTimeScale(true);
    }

    public void SwitchCameraToPlayerFalling()
    {
        LeanTween.cancel(_camera.gameObject);
        LeanTween.cancel(_camera.LookAt.gameObject);

        LeanTween.move(_camera.gameObject, PlayerFall.CameraPositionTf.position, _timeToChangeCameraPosition).setIgnoreTimeScale(true);
        LeanTween.move(_camera.LookAt.gameObject, PlayerFall.CameraLookAtTransform.position, _timeToChangeCameraLookAtTransform).setIgnoreTimeScale(true);
    }
}
