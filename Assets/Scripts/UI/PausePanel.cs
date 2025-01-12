using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    LocalManager localManager;

    private CanvasGroup canvasGroup;
    private Button exitButton;
    private TextMeshProUGUI endScoreText;

    private void Awake()
    {
        localManager = FindAnyObjectByType<LocalManager>();
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        exitButton = child.GetComponent<Button>();

        child = transform.GetChild(1);
        endScoreText = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        exitButton.onClick.AddListener(() => { localManager.OnExit(); });
        localManager.OnGamePause += () => 
        { 
            if(canvasGroup.alpha > 0.9f)
            {
                OnDeactivePanel();
            }
            else
            {
                OnActivePanel();
            }
        };
        localManager.OnGameEnd += () => 
        {
            OnActivePanel();
            endScoreText.text = $"Score : {localManager.Score}"; 
        };

        OnDeactivePanel();
    }

    private void OnActivePanel()
    {
        // 게임 중간에 실행
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        endScoreText.text = $"Paused";
    }

    private void OnDeactivePanel()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}