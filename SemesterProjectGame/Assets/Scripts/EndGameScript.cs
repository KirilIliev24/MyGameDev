using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI endGameText;
    //private string textFromPlayer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        endGameText.text = PlayerPrefs.GetString("EndGameString");
        if (endGameText.text.Equals("You WON"))
        {
            endGameText.color = Color.green;
        }
    }
    public void StartAgain()
    {
        SceneManager.LoadScene("FirstScene");
    }

    public void OpenStartMenu()
    {
        SceneManager.LoadScene("StartingMenu");
    }
}
