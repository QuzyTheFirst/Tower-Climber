using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerController _playerController;

    public PlayerController Player { get { return _playerController; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
