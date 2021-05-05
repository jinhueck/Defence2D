using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneMgr : MonoBehaviour
{
    public static string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float tempTime = 0.0f;
        while(!op.isDone)
        {
            yield return null;

            tempTime += Time.deltaTime;
            if (tempTime > 5.0f)
                op.allowSceneActivation = true;
        }
    }
}
