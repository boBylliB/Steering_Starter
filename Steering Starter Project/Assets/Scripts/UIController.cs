using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject winAScreen;
    public GameObject winBScreen;
    public GameObject replayButton;
    public GameObject startButton;
    public GameObject debugToggle;
    public int debugToggleSlideDist = 200;
    public Color debugDisabledColor;
    public Color debugEnabledColor;

    public GameManager gm;

    bool debugEnabled = false;

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
    public void toggleDebug()
    {
        debugEnabled = !debugEnabled;
        debugToggle.GetComponent<Image>().color = debugEnabled ? debugEnabledColor : debugDisabledColor;
        int direction = debugEnabled ? 1 : -1;
        debugToggle.transform.position += new Vector3(direction * debugToggleSlideDist, 0, 0);
        debugToggle.GetComponent<Shadow>().effectDistance += new Vector2(-direction * 2 * debugToggleSlideDist, 0);
        gm.debugEnabled = debugEnabled;
    }
}
