using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void OnNewGameClick()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 继续已存在的游戏
    /// </summary>
    public void OnOldGameClick()
    {
        SceneManager.LoadScene(1);
    }

}
