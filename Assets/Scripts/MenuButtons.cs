using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject CreditsPanel;
    public bool creditsOn = false;

    public void PlayButton()
    {
        SceneManager.LoadScene("Town Proto");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void CreditsButton() {
        if (creditsOn)
        {
            creditsOn = false;
            CreditsPanel.SetActive(false);
        }
        else
        {
            creditsOn = true;
            CreditsPanel.SetActive(true);
        }
        
    }
}
