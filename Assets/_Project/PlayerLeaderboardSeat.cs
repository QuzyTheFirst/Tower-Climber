using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLeaderboardSeat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameTextField;
    [SerializeField] private TextMeshProUGUI _scoreTextField;

    public TextMeshProUGUI NameTextField { get { return _nameTextField; } }
    public TextMeshProUGUI ScoreTextField { get { return _scoreTextField; } }
}
