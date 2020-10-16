using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 等级管理
/// 1. 等级/称号
/// 2. 金币
/// 3. 经验
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("等级数字")]
    public Text levelText;
    public int level = 0;
    private readonly string[] levelNames = { "新手", "入门", "渔夫", "大师", "宗师", "达人" };

    [Header("等级称号")]
    public Text levelNameText;

    [Header("经验")]
    public Slider expSlider;
    private int exp;
    public int Exp
    {
        get
        {
            return exp;
        }
    }

    [Header("金币UI")]
    public Text goldText;

    [Header("金币")]
    public int gold = 500;

    [Header("小奖倒计时")]
    public Text smallCDText;
    [Header("小奖奖金")]
    public int smallCharity = 50;
    private const int SMALL_COUNTDOWN = 60;
    private float smallCD = SMALL_COUNTDOWN;

    [Header("升级奖励金币")]
    public int upgradeGoldAward = 10000;

    [Header("大奖倒计时")]
    public Text bigCDText;
    [Header("大奖奖金")]
    public int bigCharity = 1000;
    private const int BIG_COUNTDOWN = 240;
    private float bigCD = BIG_COUNTDOWN;
    public float BigCoundown
    {
        get
        {
            return bigCD;
        }
    }

    [Header("大奖领取")]
    public Button bigAwardButton;

    [Header("大奖领取特效")]
    public GameObject bigAwardEffect;

    [Header("升级特效")]
    public GameObject levelUpEffect;

    [Header("升级提示")]
    public GameObject levelUpTips;

    // [Header("可用背景")]
    // public Sprite[] backgrounds;
    // private int backgroundIndex = 0;
    // [Header("背景")]
    // public Image backgroundImage;
    [Header("海浪:场景切换特效")]
    public GameObject seaWaveEffect;
    [Header("背景切换等级(倍数)")]
    public int changeBgByLevel = 20;

    /// <summary>
    /// 金币文本颜色
    /// </summary>
    private Color goldTextColor;

    private void Awake()
    {
        Instance = this;
        goldTextColor = goldText.color;

        // 数据恢复
        PlayerPrefsEnum.LEVEL.Get(out level, level);
        PlayerPrefsEnum.EXP.Get(out exp, exp);
        PlayerPrefsEnum.GOLD.Get(out gold, gold);
        PlayerPrefsEnum.BIG_AWARD_COUNTDOWN.Get(out bigCD, bigCD);
    }

    private void Update()
    {
        UpdateUI();
        CharityAward();
        //ChangeBackground();
    }

    /// <summary>
    /// 背景切换
    /// </summary>
    private void ChangeBackground()
    {
        // // 最后一张背景不再切换
        // if (backgroundIndex >= backgrounds.Length - 1)
        //     return;
        //
        // // 每20级切换一次背景
        // if (backgroundIndex < level / changeBgByLevel)
        // {
        //     backgroundIndex = level / changeBgByLevel % backgrounds.Length;
        //     backgroundImage.sprite = backgrounds[backgroundIndex];
        //
        //     // 播放海浪
        //     Instantiate(seeWaveEffect);
        //     AudioManager.Instance.seaWaveClip.PlayAudio();
        // }
    }

    /// <summary>
    /// 慈善奖励
    /// 1. 小奖自动领取
    /// 2. 大奖手动领取
    /// </summary>
    private void CharityAward()
    {
        // 小奖
        smallCD -= Time.deltaTime;
        if (0 > smallCD)
        {
            smallCD = SMALL_COUNTDOWN;
            gold += smallCharity;
        }
        smallCDText.text = " " + (int)(smallCD / 10) + "   " + (int)(smallCD % 10);

        // 大奖
        bigCD -= Time.deltaTime;
        if (0 > bigCD && !bigAwardButton.gameObject.activeSelf)
        {
            bigAwardButton.gameObject.SetActive(true);
            bigCDText.gameObject.SetActive(false);
            bigCD = BIG_COUNTDOWN;
        }
        bigCDText.text = (int)bigCD + "s";
    }

    /// <summary>
    /// 尝试扣除子弹费用, 扣除失败给与提示
    /// </summary>
    /// <param name="bulletCost">子弹费用</param>
    /// <returns>true-扣除成功, false-扣除失败</returns>
    public bool TryDeductBulletCost(int bulletCost)
    {
        if (gold < bulletCost)
        {
            StartCoroutine(GoldNotEnough());
            return false;
        }

        gold -= bulletCost;
        return true;
    }

    /// <summary>
    /// 金币不足提示
    /// </summary>
    /// <returns>携程对象</returns>
    private IEnumerator GoldNotEnough()
    {
        goldText.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        goldText.color = goldTextColor;
        yield return new WaitForSeconds(0.1f);
        goldText.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        goldText.color = goldTextColor;
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        // 等级
        // 计算等级
        // 升级经验 = 1000 + 200 * level
        int upgradeExp;
        while (true)
        {
            upgradeExp = CalcUpgradeExp();
            if (exp < upgradeExp)
                break;

            level++;
            exp -= upgradeExp;
            gold += upgradeGoldAward;

            // 升级提示
            levelUpTips.gameObject.SetActive(true);
            levelUpTips.transform.Find("Text").GetComponent<Text>().text = level.ToString();
            levelUpTips.GetComponent<EffectHideSelf>().delaySecond = 1;

            // 升级特效
            Instantiate(levelUpEffect);
            AudioManager.Instance.levelUpgradeClip.PlayAudio();
        }
        levelText.text = level.ToString();
        levelNameText.text = GetLevelName();

        // 经验滑动条
        expSlider.value = (float)exp / upgradeExp;

        // 金币
        goldText.text = gold.ToString();
    }

    /// <summary>
    /// 获取等级称号: 10级一个称号, 超出后使用最大称号
    /// </summary>
    /// <returns>称号</returns>
    private string GetLevelName()
    {
        int nameIndex = level / 10 % levelNames.Length;
        return levelNames[nameIndex];
    }

    /// <summary>
    /// 计算升级经验
    /// </summary>
    /// <returns>升级经验</returns>
    private int CalcUpgradeExp()
    {
        return 1000 * (1 + level);
    }

    /// <summary>
    /// 领取大救济金
    /// </summary>
    public void OnBigCharityClick()
    {
        gold += bigCharity;
        bigCDText.gameObject.SetActive(true);
        bigAwardButton.gameObject.SetActive(false);
        bigCD = BIG_COUNTDOWN;

        AudioManager.Instance.bigAwardClip.PlayAudio();
        Instantiate(bigAwardEffect);
    }

    /// <summary>
    /// 增加经验和金币
    /// </summary>
    /// <param name="fish">鱼</param>
    public void AddByFish(FishAttribute fish)
    {
        exp += fish.exp;
        gold += fish.gold;
    }
}
