using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;

public class Scene_Manager : SingletonMonoBehaviour<Scene_Manager>
{
    Scene[] scenes;

    const string scene_build_path = "Assets/Scenes";

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeScene(string sceneName,string bgmName)
    {
        ChangeScene(sceneName);
        SoundManager.Instance.PlayBGMByName(bgmName);
    }
    //現在のシーンの取得
    public Scene GetThisScene()
    {
        return SceneManager.GetActiveScene();
    }


}
