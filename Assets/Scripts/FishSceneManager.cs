using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishSceneManager : MonoBehaviour
{
    [Header("背景控件")]
    public Image backgroundImage;

    [Header("场景配置")]
    public FishSceneAttribute[] configures;

    [Header("初始场景")]
    public int index = 0;

    [Header("鱼群初始位置")]
    public Transform[] fishInitialPosList;

    [Header("鱼群容器")]
    public Transform fishHolder;

    [Header("场景切换特效")]
    public GameObject seaWaveEffect;

    private void Awake()
    {
        index = Math.Max(0, Math.Min(configures.Length - 1, index));

        // 修正鱼群
        UpgradeFishs();

        // 切换渔场
        ChangeFishScene();
    }

    /// <summary>
    /// 每个场景可多生成后面场景中2种鱼群, 最后一个场景不做处理
    /// </summary>
    private void UpgradeFishs()
    {
        for (int i= configures.Length-2; i>=0; i--)
        {
            FishSceneAttribute curr = configures[i];
            FishSceneAttribute next = configures[i+1];

            List<GameObject> fishs = new List<GameObject>(curr.fishPrefabs);

            for (int j = 0; j < 2; j++)
            {
                int index = UnityEngine.Random.Range(0, next.fishPrefabs.Length);
                fishs.Add(next.fishPrefabs[index]);
            }

            curr.fishPrefabs = fishs.ToArray();
        }
    }

    /// <summary>
    /// 切换渔场
    /// </summary>
    private void ChangeFishScene()
    {
        backgroundImage.sprite = configures[index].background;
        Instantiate(seaWaveEffect);
        Invoke(nameof(StartFishCreate), 2);
    }

    /// <summary>
    /// 开始生成鱼
    /// </summary>
    private void StartFishCreate()
    {
        // 指定鱼预制体
        FishCreation.Instance.fishPrefabs = configures[index].fishPrefabs;
        FishCreation.Instance.isCreating = true;
    }
}
