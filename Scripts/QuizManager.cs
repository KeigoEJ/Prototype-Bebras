using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    [Header("Quiz UI")]
    public GameObject quizPanel;
    public Image    questionImageUI;
    public Text     questionTextUI;
    public Button[] optionButtons;

    [Header("Result UI")]
    public GameObject correctResultPanel;
    public Text       correctResultText;
    public Button     correctExitButton;

    public GameObject wrongResultPanel;
    public Text       wrongResultText;
    public Button     wrongTryAgainButton;

    [Header("HUD")]
    public Text hudScoreText;

    [Header("Prompt UI")]
    public Text interactPromptText;    // drag your “Press E…” Text here

    private int playerScore;
    private HashSet<string> takenQuizzes;
    private QuizData currentQuiz;
    private int wrongAttempts;
    private PlayerHandler player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerScore  = 0;
        takenQuizzes = new HashSet<string>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
        => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<PlayerHandler>();
        HideAll();
        UpdateHUD();
        HidePrompt();
    }

    void Start()
    {
        // In case first scene
        player = FindObjectOfType<PlayerHandler>();
        HideAll();
        UpdateHUD();
        HidePrompt();
    }

    // —— Prompt helpers ——
    public void ShowPrompt(string msg)
    {
        if (interactPromptText == null) return;
        interactPromptText.text = msg;
        interactPromptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (interactPromptText == null) return;
        interactPromptText.gameObject.SetActive(false);
    }

    // —— Quiz logic ——

    public void StartQuiz(QuizData quiz)
    {
        if (takenQuizzes.Contains(quiz.quizID)) return;
        currentQuiz   = quiz;
        wrongAttempts = 0;
        if (player != null) player.canMove = false;

        quizPanel.SetActive(true);
        correctResultPanel.SetActive(false);
        wrongResultPanel.SetActive(false);

        questionImageUI.sprite = quiz.questionImage;
        questionTextUI.text    = quiz.questionText;
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int idx = i;
            optionButtons[i].GetComponentInChildren<Text>().text = quiz.options[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnAnswerSelected(idx));
        }

        HidePrompt();
    }

    private void OnAnswerSelected(int idx)
    {
        if (idx == currentQuiz.correctOptionIndex)
        {
            int gain = Mathf.Max(20 - wrongAttempts * 5, 0);
            playerScore += gain;
            takenQuizzes.Add(currentQuiz.quizID);

            correctResultText.text = $"Score +{gain}";
            ShowCorrectPanel();
            UpdateHUD();
        }
        else
        {
            wrongAttempts++;
            ShowWrongPanel();
        }
    }

    private void ShowCorrectPanel()
    {
        quizPanel.SetActive(false);
        wrongResultPanel.SetActive(false);
        correctResultPanel.SetActive(true);
        correctExitButton.onClick.RemoveAllListeners();
        correctExitButton.onClick.AddListener(HideAll);
    }

    private void ShowWrongPanel()
    {
        quizPanel.SetActive(false);
        correctResultPanel.SetActive(false);
        wrongResultPanel.SetActive(true);
        wrongTryAgainButton.onClick.RemoveAllListeners();
        wrongTryAgainButton.onClick.AddListener(() =>
        {
            wrongResultPanel.SetActive(false);
            quizPanel.SetActive(true);
        });
    }

    private void HideAll()
    {
        quizPanel.SetActive(false);
        correctResultPanel.SetActive(false);
        wrongResultPanel.SetActive(false);
        if (player != null) player.canMove = true;
    }

    private void UpdateHUD()
        => hudScoreText.text = $"Score: {playerScore}";
}
