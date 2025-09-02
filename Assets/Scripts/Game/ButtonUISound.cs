using UnityEngine;
using UnityEngine.UI;


public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioSource audioSource;


    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}