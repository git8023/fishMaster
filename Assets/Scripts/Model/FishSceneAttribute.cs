using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 场景配置
/// </summary>
[Serializable]
public class FishSceneAttribute
{
    [Header("鱼种类预制体")]
    public GameObject[] fishPrefabs;

    [Header("入场等级(-1不可进入)")]
    public int entryLevel;

    [Header("背景")]
    public Sprite background;

    [Header("鱼群生成周期")]
    public float waveGenWaitTime = 0.5f;

    [Header("鱼群中单条鱼间隔")]
    public float fishGenWaitTime = 0.6f;
}
