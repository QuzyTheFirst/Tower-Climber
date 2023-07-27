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

    [Header("Animations")]
    [Header("Camera")]
    [SerializeField] private float _cameraAnimDistance = .5f;
    [SerializeField] private float _cameraAnimTime = .2f;
    [Header("Camera Target")]
    [SerializeField] private float _cameraTargetAnimDistance = .5f;
    [SerializeField] private float _cameraTargetAnimTime = .2f;

    private Vector3 _cameraStartPosition;
    private Vector3 _cameraTargetStartPosition;

    private void Awake()
    {
        _cameraStartPosition = _camera.transform.position;
        _cameraTargetStartPosition = _camera.LookAt.position;
    }

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

    public void LeftAnim()
    {
        LeanTween.cancel(_camera.gameObject);
        LeanTween.cancel(_camera.LookAt.gameObject);

        LeanTween.move(_camera.gameObject, _cameraStartPosition + Vector3.right * _cameraAnimDistance, _cameraAnimTime).setEaseOutCubic().setOnComplete(() =>
        {
            LeanTween.move(_camera.gameObject, _cameraStartPosition, _cameraAnimTime).setEaseOutSine();
        });

        LeanTween.move(_camera.LookAt.gameObject, _cameraTargetStartPosition + Vector3.right * _cameraTargetAnimDistance, _cameraTargetAnimTime).setEaseOutCubic().setOnComplete(() =>
        {
            LeanTween.move(_camera.LookAt.gameObject, _cameraTargetStartPosition, _cameraTargetAnimTime).setEaseOutSine();
        });
    }

    public void RightAnim()
    {
        LeanTween.cancel(_camera.gameObject);
        LeanTween.cancel(_camera.LookAt.gameObject);

        LeanTween.move(_camera.gameObject, _cameraStartPosition + Vector3.left * _cameraAnimDistance, _cameraAnimTime).setEaseOutCubic().setOnComplete(() =>
        {
            LeanTween.move(_camera.gameObject, _cameraStartPosition, _cameraAnimTime).setEaseOutSine();
        });

        LeanTween.move(_camera.LookAt.gameObject, _cameraTargetStartPosition + Vector3.left * _cameraTargetAnimDistance, _cameraTargetAnimTime).setEaseOutBack().setOnComplete(() =>
        {
            LeanTween.move(_camera.LookAt.gameObject, _cameraTargetStartPosition, _cameraTargetAnimTime).setEaseOutSine();
        });
    }

    public void UpAnim()
    {
        LeanTween.cancel(_camera.gameObject);
        LeanTween.cancel(_camera.LookAt.gameObject);

        LeanTween.move(_camera.gameObject, _cameraStartPosition + Vector3.up * _cameraAnimDistance, _cameraAnimTime).setEaseOutCubic().setOnComplete(() =>
        {
            LeanTween.move(_camera.gameObject, _cameraStartPosition, _cameraAnimTime).setEaseOutSine();
        });

        LeanTween.move(_camera.LookAt.gameObject, _cameraTargetStartPosition + Vector3.up * _cameraTargetAnimDistance, _cameraTargetAnimTime).setEaseOutCubic().setOnComplete(() =>
        {
            LeanTween.move(_camera.LookAt.gameObject, _cameraTargetStartPosition, _cameraTargetAnimTime).setEaseOutSine();
        });
    }
}
