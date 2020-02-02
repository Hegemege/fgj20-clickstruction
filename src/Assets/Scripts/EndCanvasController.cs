using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCanvasController : MonoBehaviour
{
    public GameObject FixerText;
    public GameObject DestructorText;

    void Start()
    {
        GameManager.Instance.EndCanvas = this;
        gameObject.SetActive(false);
    }

    public void OnRematchPressed()
    {
        PoolManager.Instance.ResetPools();
        GameManager.Instance.State = GameState.Loading;
        SceneManager.LoadScene("main");
    }

    public void OnMenuPressed()
    {
        PoolManager.Instance.ResetPools();
        GameManager.Instance.State = GameState.Menu;
        SceneManager.LoadScene("menu");
    }
}
