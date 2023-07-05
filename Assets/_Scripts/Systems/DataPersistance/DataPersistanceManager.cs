using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption;

    private GameData _gameData;

    private List<IDataPersistance> _dataPersistanceObjects;

    private FileDataHandler _dataHandler;

    public bool HasGameData { 
        get
        {
            return _gameData != null;
        } 
    }

    public static DataPersistanceManager Instance { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        if(_gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if(_gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started to before data can be loaded");
            return;
        }

        foreach(IDataPersistance dataPersistanceObj in _dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        if(_gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needss to be started before data can be saved");
            return;
        }

        foreach (IDataPersistance dataPersistanceObj in _dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(_gameData);
        }

        _dataHandler.Save(_gameData);
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = 
            FindObjectsOfType<MonoBehaviour>().
            OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
