using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject winAScreen;
    public GameObject winBScreen;
    public GameObject replayButton;
    public GameObject startButton;

    public GameManager gm;

    public void win(bool teamA)
    {
        if (teamA)
            winAScreen.SetActive(true);
        else
            winBScreen.SetActive(true);
        replayButton.SetActive(true);
    }
    public void startGame()
    {
        startButton.SetActive(false);
        gm.startGame();
    }
    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
