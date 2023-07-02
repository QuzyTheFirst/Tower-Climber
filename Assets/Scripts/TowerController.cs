using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerController : PlayerInputHandler
{
    [SerializeField] private float _maxTowerPartsOnScreen = 7;
    [SerializeField] private float _towerRotatingSpeed;
    [SerializeField] private float _towerRightLeftDashDegreeDistance = 40;
    [SerializeField] private float _towerDashTime = .5f;
    [SerializeField] private float _towerUpDashDistance = 3;
    [SerializeField] private float _towerNormalFallSpeed;
    [SerializeField] private float _towerAcceleratedFallSpeed;
    [SerializeField] private Transform _towerPartsParent;
    [SerializeField] private Transform[] _towerPartsPfs;

    private List<Transform> _spawnedParts;

    private float _towerFallSpeed;

    private bool _isFirstFingerOnLeftSide;
    private bool _isFirstFingerOnRightSide;

    private bool _isSecondFingerOnLeftSide;
    private bool _isSecondFingerOnRightSide;

    private bool _isInUpDash;
    private bool _isInRightDash;
    private bool _isInLeftDash;

    private bool _hasPlayerEnteredWindow = false;

    private Vector3 _nextPartPosition;

    //Scores
    private Vector3 _lastTowerPos;
    public float _scorePoints = 0;

    private Vector3 _towerPreferedPosition;
    private Quaternion _towerPreferedRotation;

    private Coroutine _dashCoroutine;


    private bool _isFingerOnRightPart
    {
        get
        {
            return _isFirstFingerOnRightSide || _isSecondFingerOnRightSide;
        }
    }

    private bool _isFingerOnLeftPart
    {
        get
        {
            return _isFirstFingerOnLeftSide || _isSecondFingerOnLeftSide;
        }
    }

    private bool _isInDash
    {
        get
        {
            return _isInRightDash || _isInLeftDash || _isInUpDash;
        }
    }

    public int ScorePoints
    {
        get
        {
            return Mathf.RoundToInt(_scorePoints);
        }
    }

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

        _towerPreferedPosition = _towerPartsParent.position;
        _towerPreferedRotation = transform.rotation;
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
        _hasPlayerEnteredWindow = false;
    }

    private void Window_OnPlayerEnterWindow(object sender, System.EventArgs e)
    {
        Window window = sender as Window;

        if (window.PlayerHasEntered)
            return;
        window.PlayerHasEntered = true;

        StopDashCoroutine();

        PlayerController player = GameManager.Instance.Player;

        Vector3 toPlayer = player.transform.position - transform.position;
        Vector2 toPlayerDir = new Vector2(toPlayer.x, toPlayer.z).normalized;
        float playerDegree = Mathf.Atan2(toPlayerDir.y, toPlayerDir.x) * Mathf.Rad2Deg;

        Vector3 toWindow = (window.transform.position - transform.position);
        Vector2 toWindowDir = new Vector2(toWindow.x, toWindow.z).normalized;
        float windowDegree = Mathf.Atan2(toWindowDir.y, toWindowDir.x) * Mathf.Rad2Deg;

        float degree = windowDegree - playerDegree;
        _towerPreferedRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + degree , 0);
        _towerPreferedPosition = _towerPartsParent.transform.position - Vector3.up * window.transform.position.y;

        player.gameObject.layer = 8;
        player.ChangeColor(Color.yellow);
        _hasPlayerEnteredWindow = true;
    }

    private void InitializeControls()
    {
        OnLeftSwipe += TowerController_OnLeftSwipe;
        OnRightSwipe += TowerController_OnRightSwipe;
        OnUpSwipe += TowerController_OnUpSwipe;

        OnFirstFingerOnLeftPart += TowerController_OnFirstFingerOnLeftPart;
        OnFirstFingerOnRightPart += TowerController_OnFirstFingerOnRightPart;
        OnFirstFingerCanceled += TowerController_OnFirstFingerCanceled;

        OnSecondFingerOnLeftPart += TowerController_OnSecondFingerOnLeftPart;
        OnSecondFingerOnRightPart += TowerController_OnSecondFingerOnRightPart;
        OnSecondFingerCanceled += TowerController_OnSecondFingerCanceled;
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
        _lastTowerPos = _towerPreferedPosition;

        HandlePlayerInput();

        TowerFall();

        _scorePoints = -_towerPreferedPosition.y;
        GameUIController.Instance.setInGameScoreText((_scorePoints).ToString("##."));

        UpdateTowerPositionAndRotation();
    }

    private void UpdateTowerPositionAndRotation()
    {
        if(transform.rotation != _towerPreferedRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _towerPreferedRotation, Time.deltaTime * 16);
        }

        if (_towerPartsParent.position != _towerPreferedPosition)
        {
            _towerPartsParent.position = Vector3.Lerp(_towerPartsParent.position, _towerPreferedPosition, Time.deltaTime * 16);
        }
    }

    private void TowerFall()
    {
        float speedMultiplier = _hasPlayerEnteredWindow || _isInUpDash ? 0 : 1;

        _towerPreferedPosition += Vector3.down * _towerFallSpeed * speedMultiplier * Time.deltaTime;
    }

    private void HandlePlayerInput()
    {
        _towerFallSpeed = _isFingerOnLeftPart && _isFingerOnRightPart ? _towerAcceleratedFallSpeed : _towerNormalFallSpeed;

        if(!_isInDash && !_hasPlayerEnteredWindow)
        {
            if (_isFingerOnLeftPart)
            {
                _towerPreferedRotation = Quaternion.Euler(0, _towerPreferedRotation.eulerAngles.y - _towerRotatingSpeed * Time.deltaTime, 0);
            }

            if (_isFingerOnRightPart)
            {
                _towerPreferedRotation = Quaternion.Euler(0, _towerPreferedRotation.eulerAngles.y + _towerRotatingSpeed * Time.deltaTime, 0);
            }
        }
    }

    IEnumerator DoLeftDash()
    {
        if (_isInDash)
            yield return null;
        
        _isInLeftDash = true;

        if (_hasPlayerEnteredWindow)
            _hasPlayerEnteredWindow = false;

        Quaternion endDashRotation = Quaternion.Euler(0, _towerPreferedRotation.eulerAngles.y - _towerRightLeftDashDegreeDistance, 0);

        float dashTimer = 0;

        while (dashTimer >= 1)
        {
            dashTimer += Time.deltaTime / _towerDashTime;
            _towerPreferedRotation = Quaternion.Lerp(_towerPreferedRotation, endDashRotation, dashTimer);
            yield return new WaitForEndOfFrame();
        }

        _towerPreferedRotation = endDashRotation;

        _isInLeftDash = false;

        //Debug.Log($"Left Dash Done: {Time.time}");
    }

    IEnumerator DoRightDash()
    {
        if (_isInDash)
            yield return null;
        
        _isInRightDash = true;

        if(_hasPlayerEnteredWindow)
            _hasPlayerEnteredWindow = false;

        Quaternion endDashRotation = Quaternion.Euler(0, _towerPreferedRotation.eulerAngles.y + _towerRightLeftDashDegreeDistance, 0);

        float dashTimer = 0;

        while (dashTimer >= 1)
        {
            dashTimer += Time.deltaTime / _towerDashTime;
            _towerPreferedRotation = Quaternion.Lerp(_towerPreferedRotation, endDashRotation, dashTimer);
            yield return new WaitForEndOfFrame();
        }

        _towerPreferedRotation = endDashRotation;

        _isInRightDash = false;

        //Debug.Log($"Right Dash Done: {Time.time}");
    }

    IEnumerator DoUpDash() 
    {
        if (_isInDash || !_hasPlayerEnteredWindow) 
            yield return null;

        _isInUpDash = true;

        _hasPlayerEnteredWindow = false;

        Vector3 endDashPosition = _towerPartsParent.position - Vector3.up * _towerUpDashDistance;

        float dashTimer = 0;

        while (dashTimer >= 1)
        {
            dashTimer += Time.deltaTime / _towerDashTime;
            _towerPreferedPosition = Vector3.Lerp(_towerPreferedPosition, endDashPosition, dashTimer);
            yield return new WaitForEndOfFrame();
        }

        _towerPreferedPosition = endDashPosition;

        _isInUpDash = false;

        //Debug.Log($"Up Dash Done: {Time.time}");
    }

    private void StopDashCoroutine()
    {
        if (_isInDash)
        {
            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            _isInLeftDash = false;
            _isInRightDash = false;
            _isInUpDash = false;
        }
    }

    #region Controls
    private void TowerController_OnSecondFingerCanceled(object sender, System.EventArgs e)
    {
        _isSecondFingerOnRightSide = false;
        _isSecondFingerOnLeftSide = false;
    }

    private void TowerController_OnSecondFingerOnRightPart(object sender, System.EventArgs e)
    {
        _isSecondFingerOnRightSide = true;
        _isSecondFingerOnLeftSide = false;
    }

    private void TowerController_OnSecondFingerOnLeftPart(object sender, System.EventArgs e)
    {
        _isSecondFingerOnRightSide = false;
        _isSecondFingerOnLeftSide = true;
    }

    private void TowerController_OnFirstFingerCanceled(object sender, System.EventArgs e)
    {
        _isFirstFingerOnLeftSide = false;
        _isFirstFingerOnRightSide = false;
    }

    private void TowerController_OnFirstFingerOnRightPart(object sender, System.EventArgs e)
    {
        _isFirstFingerOnLeftSide = false;
        _isFirstFingerOnRightSide = true;
    }

    private void TowerController_OnFirstFingerOnLeftPart(object sender, System.EventArgs e)
    {
        _isFirstFingerOnLeftSide = true;
        _isFirstFingerOnRightSide = false;
    }

    private void TowerController_OnUpSwipe(object sender, System.EventArgs e)
    {
        if (_isInDash || !_hasPlayerEnteredWindow)
            return;

        _dashCoroutine = StartCoroutine(DoUpDash());
    }

    private void TowerController_OnRightSwipe(object sender, System.EventArgs e)
    {
        if (_isInDash)
            return;

        _dashCoroutine = StartCoroutine(DoRightDash());
    }

    private void TowerController_OnLeftSwipe(object sender, System.EventArgs e)
    {
        if (_isInDash)
            return;

        _dashCoroutine = StartCoroutine(DoLeftDash());
    }
    #endregion
}
