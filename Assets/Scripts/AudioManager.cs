using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 声音管理器
/// </summary>
public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [Header("海浪转场")]
    public AudioClip seaWaveClip;
    [Header("开炮")]
    public AudioClip fireClip;
    [Header("换炮")]
    public AudioClip changeClip;
    [Header("金币入袋")]
    public AudioClip goldClip;
    [Header("升级")]
    public AudioClip levelUpgradeClip;
    [Header("领取奖励")]
    public AudioClip bigAwardClip;
    [Header("背景音乐")]
    public AudioClip bgmClip;
    [Header("背景音效播放器")]
    public AudioSource bgmAudioSource;

    public bool IsMute { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
        PlayerPrefsEnum.IS_MUTE.Get(out int muteVal, 0);
        IsMute = (0 < muteVal);
        DoMute();
    }

    /// <summary>
    /// 切换静音模式
    /// </summary>
    public void SwitchMuteState(bool isOn)
    {
        IsMute = !isOn;
        DoMute();
    }

    public void DoMute()
    {
        if (IsMute)
        {
            bgmAudioSource.Pause();
        }
        else
        {
            bgmAudioSource.Play();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip">音效</param>
    public void PlayEffectSound(AudioClip clip)
    {
        if (!IsMute)
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

}
