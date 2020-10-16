using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鱼创建脚本
/// </summary>
public class FishCreation : MonoBehaviour
{

    public static FishCreation Instance { get; private set; }

    [Header("鱼实体容器")]
    public Transform fishHolder;

    [Header("创建位置")]
    public Transform[] genPosList;

    [Header("鱼预制体")]
    public GameObject[] fishPrefabs;

    [Header("鱼群生成周期")]
    public float waveGenWaitTime = 0.5f;

    [Header("鱼群中单条鱼间隔")]
    public float fishGenWaitTime = 0.6f;

    /// <summary>
    /// 是否生成鱼群
    /// </summary>
    public bool isCreating = false;

    void Start()
    {
        Instance = this;
        InvokeRepeating(nameof(MakeFish), 0, waveGenWaitTime);
    }

    void MakeFish()
    {
        if (!isCreating)
            return;

        // 生成位置
        int posIndex = Random.Range(0, genPosList.Length);

        // 鱼种类
        int fishIndex = Random.Range(0, fishPrefabs.Length);
        FishAttribute fishAttribute = fishPrefabs[fishIndex].GetComponent<FishAttribute>();
        int fishTotal = Random.Range((fishAttribute.maxNumOfRound / 2) + 1, fishAttribute.maxNumOfRound);
        float speed = Random.Range(fishAttribute.maxSpeed / 2, fishAttribute.maxSpeed);

        // 行动类型
        // 0: 直行鱼群
        // 1: 转弯鱼群
        int moveType = Random.Range(0, 2);
        if (0 == moveType)
        {
            // 通过携程方式调用, 防止同时创建多条鱼重叠在一起
            StartCoroutine(GenStraightFish(fishIndex, posIndex, fishTotal, speed));
        }
        else
        {
            StartCoroutine(GenTurnFish(fishIndex, posIndex, fishTotal, speed));
        }

    }

    /// <summary>
    /// 生成转弯鱼群
    /// </summary>
    /// <param name="fishIndex">鱼类型</param>
    /// <param name="posIndex">出生位置</param>
    /// <param name="fishTotal">数量</param>
    /// <param name="speed">速度</param>
    private IEnumerator GenTurnFish(int fishIndex, int posIndex, int fishTotal, float speed)
    {
        // 随机角速度
        float anguleSpeed;
        if (0 == Random.Range(0, 2))
        {
            // 右转弯
            anguleSpeed = Random.Range(-20, -9);
        }
        else
        {
            // 左转弯
            anguleSpeed = Random.Range(9, 20);
        }

        for (int i = 0; i < fishTotal; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishIndex]);

            // 指定父级
            // false: 制作预制体时已经计算好了大小和方向, 这里不需要重复计算
            fish.transform.SetParent(fishHolder, false);

            // UGUI混用时, 需要设置localPosition和localRotation
            Transform pos = genPosList[posIndex];
            fish.transform.localPosition = pos.localPosition;
            fish.transform.localRotation = pos.localRotation;

            // 防止鱼重叠时相同的层内排序导致渲染错乱
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;

            // 添加脚本
            fish.AddComponent<EffectAutoMove>().speed = speed;
            fish.AddComponent<EffectAutoTurn>().anguleSpeed = anguleSpeed;

            // 使用携程等待
            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }

    /// <summary>
    /// 生成直行鱼群
    /// </summary>
    /// <param name="fishIndex">鱼类型</param>
    /// <param name="posIndex">出生位置</param>
    /// <param name="fishTotal">数量</param>
    /// <param name="speed">速度</param>
    private IEnumerator GenStraightFish(int fishIndex, int posIndex, int fishTotal, float speed)
    {
        // 随机旋转范围: -22~22
        int angleOffset = Random.Range(-22, 22);

        for (int i = 0; i < fishTotal; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishIndex]);

            // 指定父级
            // false: 制作预制体时已经计算好了大小和方向, 这里不需要重复计算
            fish.transform.SetParent(fishHolder, false);

            // UGUI混用时, 需要设置localPosition和localRotation
            Transform pos = genPosList[posIndex];
            fish.transform.localPosition = pos.localPosition;
            fish.transform.localRotation = pos.localRotation;

            // 防止鱼重叠时相同的层内排序导致渲染错乱
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;

            // 前进方向偏移量
            // 在Z轴方向随机旋转
            fish.transform.Rotate(0, 0, angleOffset);

            // 添加移动脚本
            fish.AddComponent<EffectAutoMove>().speed = speed;

            // 使用携程等待
            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }

}
