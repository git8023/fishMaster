using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System;

namespace Extensions
{
    public static class AudioClipExtion
    {

        /// <summary>
        /// 调用AudioManager播放音效
        /// </summary>
        /// <param name="clip">目标音效</param>
        public static void PlayAudio(this AudioClip clip)
        {
            AudioManager.Instance.PlayEffectSound(clip);
        }
    }

    public enum PlayerPrefsEnum
    {
        /// <summary>
        /// 金币
        /// </summary>
        GOLD,

        /// <summary>
        /// 等级
        /// </summary>
        LEVEL,

        /// <summary>
        /// 经验
        /// </summary>
        EXP,

        /// <summary>
        /// 静音
        /// </summary>
        IS_MUTE,

        /// <summary>
        /// 大奖倒计时
        /// </summary>
        BIG_AWARD_COUNTDOWN
    }

    public static class PlayerPrefsEnumExtension
    {

        public static void Set(this PlayerPrefsEnum key, int val)
        {
            string prefsKey = Enum.GetName(typeof(PlayerPrefsEnum), key);
            PlayerPrefs.SetInt(prefsKey, val);
            PlayerPrefs.Save();
        }

        public static void Set(this PlayerPrefsEnum key, float val)
        {
            string prefsKey = Enum.GetName(typeof(PlayerPrefsEnum), key);
            PlayerPrefs.SetFloat(prefsKey, val);
            PlayerPrefs.Save();
        }

        public static void Set(this PlayerPrefsEnum key, string val)
        {
            string prefsKey = Enum.GetName(typeof(PlayerPrefsEnum), key);
            PlayerPrefs.SetString(prefsKey, val);
            PlayerPrefs.Save();
        }

        public static void Get(this PlayerPrefsEnum key, out string outVal, string defaultValue = null)
        {
            string prefsKey = Enum.GetName(typeof(PlayerPrefsEnum), key);
            outVal = PlayerPrefs.GetString(prefsKey, defaultValue);
        }

        public static void Get(this PlayerPrefsEnum key,out int outVal, int defaultValue)
        {
            string prefsKey = Enum.GetName(typeof(PlayerPrefsEnum), key);
            outVal = PlayerPrefs.GetInt(prefsKey, defaultValue);
        }

        public static void Get(this PlayerPrefsEnum key, out float outVal, float defaultValue)
        {
            string prefsKey = Enum.GetName(typeof(PlayerPrefsEnum), key);
            outVal = PlayerPrefs.GetFloat(prefsKey, defaultValue);
        }
    }

}