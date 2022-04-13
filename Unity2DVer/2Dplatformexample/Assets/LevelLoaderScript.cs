using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public static LevelLoaderScript instance;
    bool loadingLevel = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReloadScene()
    {
        if (!loadingLevel)
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
        }

    }

    public void LoadNextScene()
    {

        if (!loadingLevel)
        {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator LoadScene(int sceneNumber)
    {
        loadingLevel = true;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sceneNumber);
        yield return new WaitForSeconds(0.3f);
        loadingLevel = false;
    }
}