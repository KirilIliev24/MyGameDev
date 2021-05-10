using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
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
