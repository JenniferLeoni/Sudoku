using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void LoadTutor1()
    {
        SceneManager.LoadScene("Tutorial1");
    }
    public void LoadTutor2()
    {
        SceneManager.LoadScene("Tutorial2");
    }
}

