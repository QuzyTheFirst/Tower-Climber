using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Leaderboard")]
    [SerializeField] private Transform _playerSeatPf;
    [SerializeField] private Transform _playerSeatsParent;

    private int _amountLeaderboardSeats = 20;

    private List<PlayerLeaderboardSeat> _playerSeats;

    private string publicLeaderboardKey = "d81d604434c577ada92028f4924b8705ff02d217f7425832e1e9553f0e9a8314";


    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((callback) =>
        {
            _amountLeaderboardSeats = callback.Length;
            GenerateLeaderboard();
            for(int i = 0; i < callback.Length; i++)
            {
                _playerSeats[i].NameTextField.SetText(callback[i].Username);
                _playerSeats[i].ScoreTextField.SetText(callback[i].Score.ToString());
            }
        }));
    }

    private void GenerateLeaderboard()
    {
        Helper.DeleteAllChildren(_playerSeatsParent);
        _playerSeats.Clear();

        for (int i = 0; i < _amountLeaderboardSeats; i++)
        {
            PlayerLeaderboardSeat seat = Instantiate(_playerSeatPf, _playerSeatsParent).GetComponent<PlayerLeaderboardSeat>();
            _playerSeats.Add(seat);
        }
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((callback)=> 
        {
            GetLeaderboard();
        }));
    }

    public void OpenMainMenu()
    {
        GameUIController.Instance.ToggleMainMenu(true);
        GameUIController.Instance.ToggleLeaderboard(false);
    }
}
