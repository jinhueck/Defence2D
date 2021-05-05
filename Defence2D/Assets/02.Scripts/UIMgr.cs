using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void SceneMove(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SceneMove_Load(string sceneName)
    {
        LoadSceneMgr.LoadScene(sceneName);
    }
}
