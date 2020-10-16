using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放特效
/// </summary>
public class EffectPlayer : MonoBehaviour
{

    [Header("特效列表")]
    public GameObject[] effectPrefabs;

    public void Play()
    {
        foreach (var effectPrefab in effectPrefabs)
        {
            Instantiate(effectPrefab);
        }
    }
}
