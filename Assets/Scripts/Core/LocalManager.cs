using System;
using UnityEngine;

public class LocalManager : MonoBehaviour
{
    private Board board;

    public Action OnGameStart;
    public Action OnGameEnd;
    public Action<float> OnScoreChange;

    public float score;

    private float Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChange?.Invoke(score);
        }
    }

    private void Awake()
    {
        board = FindAnyObjectByType<Board>();
    }

    private void Start()
    {
        OnGameStart += board.Init;
        board.onGetScore += AddScore;

        OnGameStart?.Invoke();
        Score = 0;
    }

    private void AddScore(float value)
    {
        Score += value;
    }
}