using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Leaderboard")]
    [SerializeField] private int _amountLeaderboardSeats = 20;
    [SerializeField] private Transform _playerSeatPf;

    private List<PlayerLeaderboardSeat> _playerSeats;

    private string publicLeaderboardKey = "d81d604434c577ada92028f4924b8705ff02d217f7425832e1e9553f0e9a8314";

    private void Awake()
    {
        GenerateLeaderboard();
    }

    private void GenerateLeaderboard()
    {
        for(int i = 0; i< _amountLeaderboardSeats + 1; i++)
        {

        }
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((callback) =>
        {

        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((callback)=> 
        {
            GetLeaderboard();
        }));
    }
}
