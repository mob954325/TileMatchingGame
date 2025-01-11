using System;
using UnityEngine;

public class LocalManager : MonoBehaviour
{
    private Board board;

    public Action OnGameStart;
    public Action OnGameEnd;

    public float score; 

    private void Awake()
    {
        board = FindAnyObjectByType<Board>();
    }

    private void Start()
    {
        OnGameStart += board.Init;
        board.onGetScore += AddScore;

        OnGameStart?.Invoke();
    }

    private void AddScore(float value)
    {
        score += value;
    }
}