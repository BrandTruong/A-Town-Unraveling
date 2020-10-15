using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    
    public void PauseButton()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButton()
    {
        GameObject theTimeManager = GameObject.Find("TimeManager");
        TimeManager currentTimeInterval = theTimeManager.GetComponent<TimeManager>();
        //set curInteral to end of day 6am 
        currentTimeInterval.curInterval = 48;
        //interval value should be detected and launch gameover screen
    }
}
