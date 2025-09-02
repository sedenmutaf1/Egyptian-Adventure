using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyButtonHandler : MonoBehaviour
{
    public void StartEasy()
    {
        DifficultyData.speed = 3f;
        DifficultyData.difficulty = "Easy";
        SceneManager.LoadScene("GameScene");
    }

    public void StartNormal()
    {
        DifficultyData.speed = 5f;
        DifficultyData.difficulty = "Normal";
        SceneManager.LoadScene("GameScene");
    }

    public void StartHard()
    {
        DifficultyData.speed = 7f;
        DifficultyData.difficulty = "Hard";
        SceneManager.LoadScene("GameScene");
    }
}
