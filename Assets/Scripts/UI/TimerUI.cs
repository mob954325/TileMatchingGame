using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    LocalManager localManager;
    TextMeshProUGUI scoreText;

    private float time = 0;

    private void Awake()
    {
        localManager = FindAnyObjectByType<LocalManager>();
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetTimeUI(99);
        localManager.OnTimerValueChange += SetTimeUI;
    }

    public void SetTimeUI(float value)
    {
        time = value;
        scoreText.text = $"{value:F0}";
    }
}
