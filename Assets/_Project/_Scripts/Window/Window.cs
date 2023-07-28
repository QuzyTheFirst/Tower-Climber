using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Window : MonoBehaviour
{
    public static event EventHandler OnPlayerEnterWindow;
    public static event EventHandler OnPlayerExitWindow;

    [SerializeField] private Princess _princess;
    [SerializeField] private int _chanceToSpawnPrincessOneTo = 10;

    private bool _playerHasEntered = false;
    [SerializeField] private bool _hasAPrincess = false;

    private Animator _anim;

    public bool HasAPrincess { get { return _hasAPrincess; } }
    public Princess Princess { get { return _princess; } }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SpawnPrincess()
    {
        _princess.ToggleVisuals(true);
        _hasAPrincess = true;
        Debug.Log("Princess spawned!");
    }

    public void RandomlySpawnPrincess()
    {
        float chanceToSpawn = 1f / _chanceToSpawnPrincessOneTo;
        bool spawnPrincess = Random.value <= chanceToSpawn ? true : false;

        if (spawnPrincess)
        {
            _hasAPrincess = true;
            _princess.ToggleVisuals(true);
            Debug.Log("Princess spawned!");
        }
        else
        {
            _princess.ToggleVisuals(false);
            _hasAPrincess = false;
        }
    }

    public void HidePrincess()
    {
        _princess.ToggleVisuals(false);
    }

    public void OpenWindow()
    {
        _anim.SetTrigger("Open");
    }

    public void CloseWindow()
    {
        _anim.SetTrigger("Close");
    }

    public void OpenCloseWindow()
    {
        _anim.SetTrigger("FastCloseOpen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform && !_playerHasEntered)
        {
            OnPlayerEnterWindow?.Invoke(this, EventArgs.Empty);
            _playerHasEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            OnPlayerExitWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
