using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerController : PlayerInputHandler, IRestartable
{
    [SerializeField] private float _maxTowerPartsOnScreen = 7;
    [SerializeField] private float _towerDashDegree = 45;
    [SerializeField] private float _towerDashTime = .5f;
    [SerializeField] private float _towerUpDashDistance = 3;
    [SerializeField] private float _towerFallSpeed;
    [SerializeField] private Transform _towerPartsParent;
    [SerializeField] private Transform[] _towerPartsPfs;

    [Header("Camera")]
    [SerializeField] private Transform _cameraLookAtTf;
    [SerializeField] private Transform _cameraPositionTf;

    private List<Transform> _spawnedParts;

    private bool _isInUpDash;
    private bool _isInRightDash;
    private bool _isInLeftDash;

    private bool _hasPlayerEnteredWindow = false;

    private Vector3 _nextPartPosition;

    //Scores
    private float _scorePoints = 0;
    private int _timesHidenInWindow = 0;

    private Vector3 _towerPreferedPosition;
    private Quaternion _towerPreferedRotation;

    private Coroutine _dashCoroutine;

    public Transform CameraLookAtTf { get { return _cameraLookAtTf; } }
    public Vector3 CameraPosition { get { return _cameraPositionTf.position; } }

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

    public int TimesHidenInWindows { get { return _timesHidenInWindow; } }

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

        _towerPartsParent.rotation = Quaternion.Euler(0, 0, 0);

        _towerPreferedPosition = _towerPartsParent.position;
        _towerPreferedRotation = _towerPartsParent.rotation;
    }

    private void TowerPart_OnPlayerHitTopPart(object sender, System.EventArgs e)
    {
        SpawnRandomPart();

        TowerPart towerPart = sender as TowerPart;
        towerPart.BoxCollider.enabled = false;
    }
    private void Window_OnPlayerExitWindow(object sender, System.EventArgs e)
    {
        Window window = sender as Window;

        GameManager.Instance.Player.gameObject.layer = 3;
        _hasPlayerEnteredWindow = false;
    }

    private void Window_OnPlayerEnterWindow(object sender, System.EventArgs e)
    {
        Window window = sender as Window;

        if (window.PlayerHasEntered)
            return;
        window.PlayerHasEntered = true;

        StopDashCoroutines();

        PlayerController player = GameManager.Instance.Player;

        _towerPreferedPosition = _towerPartsParent.transform.position - Vector3.up * window.transform.position.y;

        player.gameObject.layer = 8;

        _hasPlayerEnteredWindow = true;
        _timesHidenInWindow++;
    }

    private void InitializeControls()
    {
        OnLeftSwipe += TowerController_OnLeftSwipe;
        OnRightSwipe += TowerController_OnRightSwipe;
        OnUpSwipe += TowerController_OnUpSwipe;
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

    public void Restart()
    {
        foreach(Transform part in _spawnedParts)
        {
            Destroy(part.gameObject);
        }

        _spawnedParts.Clear();

        _towerPartsParent.localPosition = Vector3.zero;
        _towerPartsParent.rotation = Quaternion.Euler(0, 0, 0);

        _towerPreferedPosition = _towerPartsParent.position;
        _towerPreferedRotation = _towerPartsParent.rotation;

        _hasPlayerEnteredWindow = false;
        _timesHidenInWindow = 0;

        StopDashCoroutines();

        SetUpTower();
    }

    public void SpawnRandomPart()
    {
        if (_spawnedParts.Count != 0)
            _nextPartPosition = _spawnedParts[_spawnedParts.Count - 1].position + Vector3.up * 6;

        Transform randomPart = _towerPartsPfs[Random.Range(0, _towerPartsPfs.Length)];

        Transform part = Instantiate(randomPart);
        part.position = _nextPartPosition;
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
        TowerFall();

        _scorePoints = -_towerPreferedPosition.y;
        GameUIController.Instance.InGameUI.setScore((int)_scorePoints);

        UpdateTowerPositionAndRotation();
    }

    private void UpdateTowerPositionAndRotation()
    {
        if(_towerPartsParent.rotation != _towerPreferedRotation)
        {
            _towerPartsParent.rotation = Quaternion.Lerp(_towerPartsParent.rotation, _towerPreferedRotation, Time.deltaTime * 16);
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
        //Debug.Log(_towerPreferedPosition);
    }

    IEnumerator DoLeftDash()
    {
        if (_isInDash)
            yield return null;
        
        _isInLeftDash = true;

        if (_hasPlayerEnteredWindow)
            _hasPlayerEnteredWindow = false;

        Quaternion endDashRotation = Quaternion.Euler(0, _towerPreferedRotation.eulerAngles.y - _towerDashDegree, 0);

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

        Quaternion endDashRotation = Quaternion.Euler(0, _towerPreferedRotation.eulerAngles.y + _towerDashDegree, 0);

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

    private void StopDashCoroutines()
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
