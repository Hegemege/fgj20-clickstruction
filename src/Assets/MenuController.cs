using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image Tutorial;
    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }

    void Update()
    {
        if (Tutorial.gameObject.activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                StartGame();
            }
        }
    }

    public void ShowTutorial()
    {
        Tutorial.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
