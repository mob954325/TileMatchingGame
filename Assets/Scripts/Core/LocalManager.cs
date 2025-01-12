using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalManager : MonoBehaviour
{
    private Board board;
    private InputController input;

    public Action OnGameStart;
    public Action OnGameEnd;
    public Action OnGamePause;
    public Action<float> OnScoreChange;
    public Action<float> OnTimerValueChange;

    private float MaxTime = 2;
    private float remainTime = -1;

    private bool isPause = false;
    public float RemainTime
    {
        get => remainTime;
        set 
        { 
            remainTime = Math.Clamp(value, 0, MaxTime);
            OnTimerValueChange?.Invoke(remainTime);

            if(remainTime <= 0)
            {
                OnGameEnd?.Invoke();
            }
        }
    }

    private float score;
    public float Score
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
        input = FindAnyObjectByType<InputController>();
    }

    private void Start()
    {
        input.OnPauseInput += () => 
        {
            if (remainTime <= 0f) return;

            if(isPause)
            {
                OnUnpause();
            }
            else
            {
                OnPause();
            }            
        };
        Init();
    }

    private void Update()
    {
        if (isPause) return;

        RemainTime -= Time.deltaTime;
    }

    private void Init()
    {
        OnGameStart += board.Init;
        board.onGetScore += AddScore;

        OnGameStart?.Invoke();
        Score = 0;
        RemainTime = MaxTime;
        isPause = false;
    }

    private void AddScore(float value)
    {
        Score += value;
    }

    public void OnPause()
    {
        isPause = true;
        OnGamePause?.Invoke();
    }

    public void OnUnpause()
    {
        isPause = false;
        OnGamePause?.Invoke();
    }

    /// <summary>
    /// 메인 메뉴씬으로 씬 변경하는 함수
    /// </summary>
    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}