using UnityEngine;
using UnityEngine.UI;

public class InstructionsButton : MonoBehaviour
{
    public GameObject instructionsPanel;

    public void ShowInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
            Invoke("HideInstructions", 10f); 
        }
    }

    void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
