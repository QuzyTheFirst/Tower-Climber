using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerController : PlayerInputHandler
{
    [SerializeField] private float _maxTowerPartsOnScreen = 7;
    [SerializeField] private float _towerRotatingSpeed;
    [SerializeField] private float _towerRightLeftDashDegreeDistance = 40;
    [SerializeField] private float _towerUpDashDistance = 3;
    [SerializeField] private float _towerNormalFallSpeed;
    [SerializeField] private float _towerAcceleratedFallSpeed;
    [SerializeField] private Transform _towerPartsParent;
    [SerializeField] private Transform[] _towerPartsPfs;

    private List<Transform> _spawnedParts;

    private float _towerFallSpeed;

    private bool _isInDash = false;

    private bool _isUpButtonPressed;
    private bool _doUpDash;

    private bool _isLeftButtonPressed;
    private bool _doLeftDash;

    private bool _isRightButtonPressed;
    private bool _doRightDash;

    private bool _isPlayerHidenInWindow = false;

    private Vector3 _nextPartPosition;

    private bool _enteringWindowAnimation = false;
    private Quaternion _preferedTowerRotation;
    private Vector3 _preferedTowerPosition;

    // Dash
    private Quaternion _endDashRotation;
    private Vector3 _endDashPosition;

    //Scores
    private Vector3 _lastTowerPos;
    public float _scorePoints = 0;


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
        Window window = sender as Window;

        GameManager.Instance.Player.gameObject.layer = 3;
        GameManager.Instance.Player.ChangeColor(Color.green);
        _isPlayerHidenInWindow = false;
    }

    private void Window_OnPlayerEnterWindow(object sender, System.EventArgs e)
    {
        Window window = sender as Window;
        if (window.PlayerHasEntered)
            return;

        window.PlayerHasEntered = true;

        PlayerController player = GameManager.Instance.Player;

        Vector3 toPlayer = player.transform.position - transform.position;
        Vector2 toPlayerDir = new Vector2(toPlayer.x, toPlayer.z).normalized;
        float playerDegree = Mathf.Atan2(toPlayerDir.y, toPlayerDir.x) * Mathf.Rad2Deg;

        Vector3 toWindow = (window.transform.position - transform.position);
        Vector2 toWindowDir = new Vector2(toWindow.x, toWindow.z).normalized;
        float windowDegree = Mathf.Atan2(toWindowDir.y, toWindowDir.x) * Mathf.Rad2Deg;

        float degree = windowDegree - playerDegree;
        _preferedTowerRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + degree , 0);
        _preferedTowerPosition = _towerPartsParent.transform.position - Vector3.up * window.transform.position.y;

        player.gameObject.layer = 8;
        player.ChangeColor(Color.yellow);
        _isPlayerHidenInWindow = true;
        _enteringWindowAnimation = true;
    }

    private void InitializeControls()
    {
        LeftPartPressPerformed += TowerController_LeftPartPressPerformed;
        LeftPartPressCanceled += TowerController_LeftPartPressCanceled;
        LeftPartMultiTapPerformed += TowerController_LeftPartMultiTapPerformed;

        RightPartPressPerformed += TowerController_RightPartPressPerformed;
        RightPartPressCanceled += TowerController_RightPartPressCanceled;
        RightPartMultiTapPerformed += TowerController_RightPartMultiTapPerformed;

        UpPressPerformed += TowerController_UpPressPerformed;
        UpPressCanceled += TowerController_UpPressCanceled;

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
        _lastTowerPos = _towerPartsParent.localPosition;

        HandlePlayerInput();

        TowerFall();

        if (_enteringWindowAnimation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _preferedTowerRotation, Time.deltaTime * 8);
            _towerPartsParent.position = Vector3.Lerp(_towerPartsParent.position, _preferedTowerPosition, Time.deltaTime * 8);

            bool rotationBool = Mathf.Abs(transform.rotation.eulerAngles.y - _preferedTowerRotation.eulerAngles.y) < 2;
            bool positionBool = Vector3.Distance(_towerPartsParent.position, _preferedTowerPosition) < .05f;
            
            if(rotationBool && positionBool)
            {
                _enteringWindowAnimation = false;
            }
        }

        _scorePoints = -_towerPartsParent.localPosition.y;
        GameUIController.Instance.setInGameScoreText((_scorePoints).ToString("##."));
    }

    private void TowerFall()
    {
        if (_enteringWindowAnimation)
            return;

        float speedMultiplier = _isPlayerHidenInWindow || _doUpDash ? 0 : 1;

        _towerPartsParent.position += Vector3.down * _towerFallSpeed * speedMultiplier * Time.deltaTime;
    }

    private void HandlePlayerInput()
    {
        if (_enteringWindowAnimation)
            return;

        _towerFallSpeed = _isLeftButtonPressed && _isRightButtonPressed ? _towerAcceleratedFallSpeed : _towerNormalFallSpeed;

        if (_isLeftButtonPressed && !_isInDash)
        {
            if (!_isPlayerHidenInWindow)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - _towerRotatingSpeed * Time.deltaTime, 0);
            }
            else
            {
                _endDashRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - _towerRightLeftDashDegreeDistance, 0);
                _doLeftDash = true;
                _isInDash = true;

                _isPlayerHidenInWindow = false;
            }
        }


        if (_isRightButtonPressed && !_isInDash)
        {
            if (!_isPlayerHidenInWindow) 
            { 
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + _towerRotatingSpeed * Time.deltaTime, 0);
            }
            else
            {
                _endDashRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + _towerRightLeftDashDegreeDistance, 0);
                _doRightDash = true;
                _isInDash = true;

                _isPlayerHidenInWindow = false;
            }
        }

        if (_doUpDash)
        {
            if (_isPlayerHidenInWindow)
            {
                _doUpDash = false;
                _isInDash = false;
            }

            _towerPartsParent.position = Vector3.Lerp(_towerPartsParent.position, _endDashPosition, Time.deltaTime * 8);

            if (Vector3.Distance(_towerPartsParent.position, _endDashPosition) < .05f)
            {
                _towerPartsParent.position = _endDashPosition;
                _doUpDash = false;
                _isInDash = false;

                _isPlayerHidenInWindow = false;
            }
        }

        if (_doLeftDash)
        {
            if (_isPlayerHidenInWindow)
            {
                _doLeftDash = false;
                _isInDash = false;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, _endDashRotation, Time.deltaTime * 8);

            if (Mathf.Abs(transform.rotation.eulerAngles.y - _endDashRotation.eulerAngles.y) < 2)
            {
                _doLeftDash = false;
                _isInDash = false;
            }
        }

        if (_doRightDash)
        {
            if (_isPlayerHidenInWindow)
            {
                _doRightDash = false;
                _isInDash = false;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, _endDashRotation, Time.deltaTime * 8);

            if (Mathf.Abs(transform.rotation.eulerAngles.y - _endDashRotation.eulerAngles.y) < 2)
            {
                _doRightDash = false;
                _isInDash = false;
            }
        }
    }

    private void TowerController_RightPartMultiTapPerformed(object sender, System.EventArgs e)
    {
        if (_isInDash)
            return;

        _endDashRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + _towerRightLeftDashDegreeDistance, 0);
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
        if (_isInDash)
            return;

        _endDashRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - _towerRightLeftDashDegreeDistance, 0);
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

    private void TowerController_UpPressCanceled(object sender, System.EventArgs e)
    {
        _isUpButtonPressed = false;
    }

    private void TowerController_UpPressPerformed(object sender, System.EventArgs e)
    {
        if (_isInDash || !_isPlayerHidenInWindow)
            return;

        _isInDash = true;
        _doUpDash = true;
        _endDashPosition = _towerPartsParent.position - Vector3.up * _towerUpDashDistance;
        _isUpButtonPressed = true;

        _isPlayerHidenInWindow = false;
    }
}
