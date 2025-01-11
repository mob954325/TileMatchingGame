using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    LocalManager localManager;
    TextMeshProUGUI scoreText;

    private float scoreChangeSpeed = 2f;
    private float currentScore = 0;

    private void Awake()
    {
        localManager = FindAnyObjectByType<LocalManager>();
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetScoreUI(0);
        localManager.OnScoreChange += SetScoreUI;
    }

    public void SetScoreUI(float score)
    {
        StopCoroutine(scoreChangeProcess(currentScore));
        StartCoroutine(scoreChangeProcess(score));
    }

    private IEnumerator scoreChangeProcess(float changedValue)
    {
        float timeElapsed = 0f;

        while(timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * scoreChangeSpeed;

            float lerp = Mathf.Lerp(currentScore, changedValue, timeElapsed);
            scoreText.text = $"{lerp:F0}";
            yield return null;
        }

        currentScore = changedValue;
    }
}