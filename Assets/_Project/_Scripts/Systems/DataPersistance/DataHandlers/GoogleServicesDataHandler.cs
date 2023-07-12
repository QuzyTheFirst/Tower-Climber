using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

public class GoogleServicesDataHandler
{
    private bool _isSaving = false;
    private bool _isLoading = false;

    private string _dataFileName = "";

    public bool IsSaving { get { return _isSaving; } }
    public bool IsLoading { get { return _isLoading; } }

    public GoogleServicesDataHandler(string dataFileName)
    {
        _dataFileName = dataFileName;
    }

    public async Task<GameData> Load()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Debug.Log("<color=red>You are not authenticated!</color>");
            return null;
        }

        _isLoading = true;

        GameData gameData = null;

        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(_dataFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, (status, metadata) =>
        {
            if (status == SavedGameRequestStatus.Success)
            {
                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(metadata, (status, byteArray) =>
                {
                    if(status == SavedGameRequestStatus.Success)
                    {
                        string loadedGameData = System.Text.Encoding.ASCII.GetString(byteArray);
                        gameData = JsonConvert.DeserializeObject<GameData>(loadedGameData);
                        Debug.Log("<color=cyan>Data successfully loaded!</color>");
                    }
                    else
                    {
                        Debug.Log("<color=red>Something went wrong trying to load data!</color>");
                    }

                    _isLoading = false;
                });
            }
            else
            {
                Debug.Log("<color=red>Something went wrong trying to load data!</color>");
                _isLoading = false;
            }
        });

        while (_isLoading)
        {
            await Task.Delay(500);
        }

        Debug.Log("Returning Game Data");
        return gameData;
    }

    public void Save(GameData data)
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Debug.Log("<color=red>You are not authenticated!</color>");
            return;
        }

        _isSaving = true;

        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(_dataFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, (status, metadata) =>
        {
            if (status == SavedGameRequestStatus.Success)
            {
                SavedGameMetadataUpdate updateForMetadata = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription($"Game Save File Updated: {DateTime.Now}").Build();

                string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);
                byte[] dataToStoreByteArray = System.Text.Encoding.ASCII.GetBytes(dataToStore);

                PlayGamesPlatform.Instance.SavedGame.CommitUpdate(metadata, updateForMetadata, dataToStoreByteArray, SaveGameCallback);
            }
            else
            {
                Debug.Log("<color=red>Something went wrong trying to save data!</color>");
                _isSaving = false;
            }
        });
    }

    private void SaveGameCallback(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("<color=cyan>Data successfully saved!</color>");
        }
        else
        {
            Debug.Log("<color=red>Something went wrong trying to save data!</color>");
        }
        _isSaving = false;
    }
}
