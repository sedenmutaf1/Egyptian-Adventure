using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;
    public GameObject gameManager;
    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI countdownText;
    public float totalTime = 60f;
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI platformsHopped;
    public TextMeshProUGUI platformsHopped2;
    private Coroutine timerCoroutine;
    private float timeRemaining;
    private bool isPaused = false;
    private int platformsHoppedCount = 0;
    public Button continueButton;
    public AudioClip gameOverSound;
    public AudioClip buttonSound;
    public AudioSource buttonAudioSource;
    public AudioSource gameOverAudioSource;
    public AudioClip gameWinSound;
    public AudioSource gameWinAudioSource;

    void Start()
    {
        timeRemaining = totalTime;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(ThreeTwoOneGo());
    }

    public void UpdatePlatformHoppedDisplay(int PlatformsHoppedCount)
    {
        platformsHoppedCount = PlatformsHoppedCount;
        platformsHopped2.text = "Platforms Hopped: " + platformsHoppedCount + "/" + GetComponent<PlatformGenerator>().platformCountLimit;


    }

    public void PauseGame()
    {
        isPaused = true;
        player.GetComponent<PlayerController>().enabled = false;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        countdownText.gameObject.SetActive(false);
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        StartCoroutine(ThreeTwoOneGo()); 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("StartScene");
    }

    public void TriggerGameOver()
    {
        PauseGame();
        GameOverText.text = "Game Over!";
        platformsHopped.text = "Platforms Hopped: " + platformsHoppedCount;
        GameOverText.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
        platformsHopped.gameObject.SetActive(true);
        gameOverAudioSource.PlayOneShot(gameOverSound);

    }
    public void TriggerGameWin()
    {
        PauseGame();
        GameOverText.text = "Congratulations!";
        platformsHopped.text = "Platforms Hopped: " + platformsHoppedCount;
        GameOverText.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
        platformsHopped.gameObject.SetActive(true);
        gameWinAudioSource.PlayOneShot(gameWinSound);

    }

    private IEnumerator TimerRoutine()
    {
        while (timeRemaining > 0f)
        {
            if (!isPaused)
            {
                timeRemaining -= Time.deltaTime;
                timeLeftText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
            }

            yield return null;
        }

        TriggerGameOver();
    }

    private IEnumerator ThreeTwoOneGo()
    {
        
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        player.GetComponent<PlayerController>().enabled = false;

        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        buttonAudioSource.PlayOneShot(buttonSound);
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        buttonAudioSource.PlayOneShot(buttonSound);
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        buttonAudioSource.PlayOneShot(buttonSound);
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;

        timeRemaining = totalTime; 
        timerCoroutine = StartCoroutine(TimerRoutine());
    }
}
