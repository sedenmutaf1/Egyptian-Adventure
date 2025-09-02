using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject difficultyPanel;
    public GameObject instructionsButton;

    public void OnStartButtonClick()
    {
        
        difficultyPanel.SetActive(true);
        startButton.SetActive(false);
        instructionsButton.SetActive(false);

    }
}
