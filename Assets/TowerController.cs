using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerController : PlayerInputHandler
{
    [SerializeField] private float _maxTowerPartsOnScreen = 7;
    [SerializeField] private float _towerRotatingSpeed;
    [SerializeField] private float _towerDashDistance;
    [SerializeField] private float _towerNormalFallSpeed;
    [SerializeField] private float _towerAcceleratedFallSpeed;
    [SerializeField] private Transform _towerPartsParent;
    [SerializeField] private Transform[] _towerPartsPfs;

    private List<Transform> _spawnedParts;

    private float _towerFallSpeed;

    private bool _isInDash = false;

    private bool _isLeftButtonPressed;
    private bool _doLeftDash;

    private bool _isRightButtonPressed;
    private bool _doRightDash;

    private bool _isPlayerHidenInWindow = false;

    private Vector3 _nextPartPosition;

    // Dash
    private Quaternion _endingRotation;


    protected override void OnEnable()
    {
        base.OnEnable();

        TowerPart.OnPlayerHitTopPart += TowerPart_OnPlayerHitTopPart;

        Window.OnPlayerEnterWindow += Window_OnPlayerEnterWindow;
        Window.OnPlayerExitWindow += Window_OnPlayerExitWindow;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        TowerPart.OnPlayerHitTopPart -= TowerPart_OnPlayerHitTopPart;

        Window.OnPlayerEnterWindow -= Window_OnPlayerEnterWindow;
        Window.OnPlayerExitWindow -= Window_OnPlayerExitWindow;
    }

    protected override void Awake()
    {
        base.Awake();

        InitializeControls();

        SetUpTower();
    }

    private void TowerPart_OnPlayerHitTopPart(object sender, System.EventArgs e)
    {
        SpawnRandomPart();
    }
    private void Window_OnPlayerExitWindow(object sender, System.EventArgs e)
    {
        GameManager.Instance.Player.gameObject.layer = 3;
        GameManager.Instance.Player.ChangeColor(Color.green);
        _isPlayerHidenInWindow = false;
    }

    private void Window_OnPlayerEnterWindow(object sender, System.EventArgs e)
    {
        GameManager.Instance.Player.gameObject.layer = 8;
        GameManager.Instance.Player.ChangeColor(Color.yellow);
        _isPlayerHidenInWindow = true;
    }

    private void InitializeControls()
    {
        LeftPartPressPerformed += TowerController_LeftPartPressPerformed;
        LeftPartPressCanceled += TowerController_LeftPartPressCanceled;
        LeftPartMultiTapPerformed += TowerController_LeftPartMultiTapPerformed;

        RightPartPressPerformed += TowerController_RightPartPressPerformed;
        RightPartPressCanceled += TowerController_RightPartPressCanceled;
        RightPartMultiTapPerformed += TowerController_RightPartMultiTapPerformed;

        RestartPerformed += TowerController_RestartPerformed;
    }

    private void TowerController_RestartPerformed(object sender, System.EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetUpTower()
    {
        _spawnedParts = new List<Transform>();

        _nextPartPosition = transform.position - Vector3.up * 18;

        for(int i = 0; i < _maxTowerPartsOnScreen; i++)
        {
            SpawnRandomPart();
        }
    }

    public void SpawnRandomPart()
    {
        if (_spawnedParts.Count != 0)
            _nextPartPosition = _spawnedParts[_spawnedParts.Count - 1].position + Vector3.up * 6;

        Transform randomPart = _towerPartsPfs[Random.Range(0, _towerPartsPfs.Length)];
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0f);

        Transform part = Instantiate(randomPart, _nextPartPosition, randomRotation);
        part.parent = _towerPartsParent;

        _spawnedParts.Add(part);

        if(_spawnedParts.Count >= _maxTowerPartsOnScreen)
        {
            DeleteOldestPart();
        }
    }

    private void DeleteOldestPart()
    {
        Transform partToDelete = _spawnedParts[0];
        _spawnedParts.RemoveAt(0);
        Destroy(partToDelete.gameObject);
    }

    private void Update()
    {
        HandlePlayerInput();

        TowerFall();
    }

    private void TowerFall()
    {
        float speedMultiplier = _isPlayerHidenInWindow ? 0 : 1;

        _towerPartsParent.position += Vector3.down * _towerFallSpeed * speedMultiplier * Time.deltaTime;
    }

    private void HandlePlayerInput()
    {
        _towerFallSpeed = _isLeftButtonPressed && _isRightButtonPressed ? _towerAcceleratedFallSpeed : _towerNormalFallSpeed;

        if (_isLeftButtonPressed && /*_player.CanGoLeft() &&*/ !_isInDash)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - _towerRotatingSpeed * Time.deltaTime, 0);
        }

        if (_isRightButtonPressed && /*_player.CanGoRight() &&*/ !_isInDash)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + _towerRotatingSpeed * Time.deltaTime, 0);
        }

        if (_doLeftDash)
        {
            /**if (!_player.CanGoLeft())
            {
                _doLeftDash = false;
                _isInDash = false;
                return;
            }*/


            transform.rotation = Quaternion.Lerp(transform.rotation, _endingRotation, Time.deltaTime * 8);

            if (Mathf.Abs(transform.rotation.eulerAngles.y - _endingRotation.eulerAngles.y) < 2)
            {
                _doLeftDash = false;
                _isInDash = false;
            }
        }

        if (_doRightDash)
        {
            /*if (!_player.CanGoRight())
            {
                _doRightDash = false;
                _isInDash = false;
                return;
            }*/

            transform.rotation = Quaternion.Lerp(transform.rotation, _endingRotation, Time.deltaTime * 8);

            if (Mathf.Abs(transform.rotation.eulerAngles.y - _endingRotation.eulerAngles.y) < 2)
            {
                _doRightDash = false;
                _isInDash = false;
            }
        }
    }

    private void TowerController_RightPartMultiTapPerformed(object sender, System.EventArgs e)
    {
        _endingRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + _towerDashDistance, 0);
        _doRightDash = true;
        _isInDash = true;
    }

    private void TowerController_RightPartPressCanceled(object sender, System.EventArgs e)
    {
        _isRightButtonPressed = false;
    }

    private void TowerController_RightPartPressPerformed(object sender, System.EventArgs e)
    {
        _isRightButtonPressed = true;
    }

    private void TowerController_LeftPartMultiTapPerformed(object sender, System.EventArgs e)
    {
        _endingRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - _towerDashDistance, 0);
        _doLeftDash = true;
        _isInDash = true;
    }

    private void TowerController_LeftPartPressCanceled(object sender, System.EventArgs e)
    {
        _isLeftButtonPressed = false;
    }

    private void TowerController_LeftPartPressPerformed(object sender, System.EventArgs e)
    {
        _isLeftButtonPressed = true;
    }
}
