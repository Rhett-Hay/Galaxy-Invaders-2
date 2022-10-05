using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Handle to Score_text
    [SerializeField] private TMPro.TMP_Text _scoreText;
    // Access to the 4 different Lives sprite sheets
    [SerializeField] private Sprite[] _liveSprites;
    // Access the Lives image component
    [SerializeField] private Image _livesImage;
    // Access Game Over text object
    [SerializeField] private TMPro.TMP_Text _gameOverText;
    // Access Restart Level text object
    [SerializeField] private TMPro.TMP_Text _restartLevelText;
    // Access the Game Manager
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Assign Score_text to zero at the beginning of the game
        _scoreText.text = "SCORE:      " + 0;
        // Turn off Game Over text object when game starts
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
        _gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL!");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "SCORE:      " + playerScore.ToString();
    }

    // Update Lives image onscreen
    public void UpdateLives(int currentLives)
    {
        // Show the current image,
        // then change to a new image based on the current lives index
        _livesImage.sprite = _liveSprites[currentLives];

        // If currentLives equals 0, turn on Game Over text object
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        // Game is over officially
        _gameManager.GameOver();
        // turn on Game Over text
        _gameOverText.gameObject.SetActive(true);
        // turn on Restart Level text
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
