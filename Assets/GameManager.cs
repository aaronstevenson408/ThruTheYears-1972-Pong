using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Serializable]
    public class Player
    {
        public string name;
        public Paddle paddle;
        public ScoreZone scoreZone;
        public TextMeshProUGUI scoreText;
        public int score;
    }

    public Player player1;
    public Player player2;

    public int winningScore = 10;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button replayButton;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePlayers();
        SetupReplayButton();
        HideGameOverPanel();
    }

    private void InitializePlayers()
    {
        SetupPlayer(player1, "Player 1");
        SetupPlayer(player2, "Player 2");
    }

    private void SetupPlayer(Player player, string playerReference)
    {
        if (player == null)
        {
            Debug.LogError($"{playerReference} is not set in the Inspector.");
            return;
        }

        player.score = 0;

        if (player.paddle == null)
        {
            Debug.LogError($"Paddle for {playerReference} is not set in the Inspector.");
        }
        else
        {
            player.paddle.player = player;
        }

        if (player.scoreZone == null)
        {
            Debug.LogError($"ScoreZone for {playerReference} is not set in the Inspector.");
        }
        else
        {
            player.scoreZone.scoringPlayer = player;
        }

        UpdateScoreDisplay(player);
    }

    public void AddScore(Player player)
    {
        if (isGameOver) return;

        if (player == null)
        {
            Debug.LogError("Attempted to add score to a null player.");
            return;
        }

        player.score++;
        UpdateScoreDisplay(player);
        Debug.Log($"{player.name} scored! New score: {player.score}");

        CheckForGameOver();
    }

    private void UpdateScoreDisplay(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Attempted to update score display for a null player.");
            return;
        }

        if (player.scoreText == null)
        {
            Debug.LogError($"Score Text for player {player.name} is not set in the Inspector.");
            return;
        }

        player.scoreText.text = player.score.ToString();
    }

    private void CheckForGameOver()
    {
        if (player1.score >= winningScore || player2.score >= winningScore)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        isGameOver = true;
        Player winner = (player1.score > player2.score) ? player1 : player2;
        ShowGameOverPanel($"{winner.name} wins!");
        // Disable paddles, ball movement, etc.
    }

    private void ShowGameOverPanel(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (gameOverText != null)
        {
            gameOverText.text = message;
        }
    }

    private void HideGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void SetupReplayButton()
    {
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(ResetGame);
        }
        else
        {
            Debug.LogError("Replay Button is not set in the Inspector.");
        }
    }

    private void ResetGame()
    {
        isGameOver = false;
        player1.score = 0;
        player2.score = 0;
        UpdateScoreDisplay(player1);
        UpdateScoreDisplay(player2);
        HideGameOverPanel();
        // Reset ball position, paddle positions, etc.
    }

    // Add other game management methods as needed
}