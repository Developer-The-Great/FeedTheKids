using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    private SoundManager soundManager;

    public void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        soundManager.BackgroundMusic.Play();
    }

    public void StartGame()
    {
        soundManager.BackgroundMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("works");
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }


}
