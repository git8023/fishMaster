using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鱼属性
/// </summary>
public class FishAttribute : MonoBehaviour
{
    [Header("生命值")]
    public int hp;

    [Header("价值金币")]
    public int gold;

    [Header("经验")]
    public int exp;

    [Header("一波最大数量")]
    public int maxNumOfRound;

    [Header("速度")]
    public float maxSpeed;

    [Header("尸体")]
    public GameObject diePrefab;

    [Header("金币预制体")]
    public GameObject goldPrefab;

    [Header("捕中概率(0~100)")]
    public int hitProbability;

    private void Awake()
    {
        // 金币为生命值 80%
        // gold = (int)(hp * 0.8);

        // 经验为生命值 65%
        // exp = (int)(hp * 0.65);

        exp = gold;
    }

    /// <summary>
    /// 通过概率计算是否可以被捕获
    /// </summary>
    /// <param name="bulletTypeIndex">子弹类型下标</param>
    public void HitByProbability(int bulletTypeIndex)
    {
        // 计算目标子弹和当前鱼的增量概率
        int offsetProb = GameControl.Instance.CalcHitProb(bulletTypeIndex, gold);
        int targetProb = Math.Max(0, hitProbability + offsetProb);
        int prob = UnityEngine.Random.Range(1, 101);
        if (prob < targetProb)
            CaptureSuccess();
    }

    /// <summary>
    /// 捕鱼成功
    /// </summary>
    private void CaptureSuccess()
    {
        // 增加经验和金币
        LevelManager.Instance.AddByFish(this);

        // 播放死亡动画
        GameObject die = Instantiate(diePrefab);
        die.transform.SetParent(transform.parent, false);
        die.transform.position = transform.position;
        die.transform.rotation = transform.rotation;

        // 产生金币
        GameObject gold = Instantiate(goldPrefab);
        gold.transform.SetParent(transform.parent, false);
        gold.transform.position = transform.position;
        gold.transform.rotation = transform.rotation;

        // 播放特效
        EffectPlayer effectPlayer = GetComponent<EffectPlayer>();
        if (null != effectPlayer)
            effectPlayer.Play();

        // 销毁鱼
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bounds"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 【已过期】
    /// 处理伤害值
    /// </summary>
    /// <param name="demage">伤害</param>
    public void TakeDemage(int demage)
    {
        hp -= demage;
        if (0 >= hp)
            CaptureSuccess();
    }

}
