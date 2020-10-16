using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 主场景UI交互
/// </summary>
public class MainSceneUI : MonoBehaviour
{

    [Header("设置面板")]
    public GameObject settingsPanel;

    [Header("音效开关")]
    public Toggle soundToggle;

    private void Awake()
    {
        soundToggle.isOn = !AudioManager.Instance.IsMute;
    }

    /// <summary>
    /// 点开设置按钮
    /// </summary>
    public void OnSettingClick()
    {
        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// 点击返回按钮
    /// </summary>
    public void OnBackButtonClick()
    {
        // 保存数据并返回开始场景
        PlayerPrefsEnum.GOLD.Set(LevelManager.Instance.gold);
        PlayerPrefsEnum.LEVEL.Set(LevelManager.Instance.level);
        PlayerPrefsEnum.EXP.Set(LevelManager.Instance.Exp);
        PlayerPrefsEnum.IS_MUTE.Set(AudioManager.Instance.IsMute ? 1 : 0);
        PlayerPrefsEnum.BIG_AWARD_COUNTDOWN.Set(LevelManager.Instance.BigCoundown);

        // 返回第一个场景
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 游戏音效开关
    /// </summary>
    public void OnSwitchMuteClick(bool isOn)
    {
        AudioManager.Instance.SwitchMuteState(isOn);
    }

    /// <summary>
    /// 关闭设置面板
    /// </summary>
    public void OnCloseSettingClick()
    {
        settingsPanel.SetActive(false);
    }

}
