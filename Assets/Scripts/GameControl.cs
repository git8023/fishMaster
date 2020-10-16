using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 游戏控制器, 控制每发子弹消耗的钱和枪等级
/// </summary>
public class GameControl : MonoBehaviour
{
    public static GameControl Instance { get; private set; }

    [Header("枪列表")]
    public GameObject[] gunGos;

    [Header("子弹价格, 每4个子弹对应一把枪")]
    public int[] bulletCosts = { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    [Header("子弹单价Text")]
    public Text bulletCostText;

    #region 子弹预制体
    public GameObject[] bullet1Prefabs;
    public GameObject[] bullet2Prefabs;
    public GameObject[] bullet3Prefabs;
    public GameObject[] bullet4Prefabs;
    public GameObject[] bullet5Prefabs;
    #endregion

    [Header("子弹容器")]
    public Transform bulletHolderTransform;

    [Header("换枪特效")]
    public GameObject changeGunEffect;

    [Header("开炮特效")]
    public GameObject fireEffect;

    [Header("开炮间隔(秒)")]
    public float fireInterval = 0.2f;

    /// <summary>
    /// 子弹开炮间隔计时器
    /// </summary>
    private float fireTimer = 0;

    /// <summary>
    /// 计算捕获概率增量
    /// </summary>
    /// <param name="bulletTypeIndex">子弹下标</param>
    /// <param name="fishGold">鱼的价值</param>
    /// <returns>增量概率在-100~100之间</returns>
    public int CalcHitProb(int bulletIndex, int fishGold)
    {
        int rightIndex = -1;
        for (int i=0; i<bulletCosts.Length; i++)
        {
            if (bulletCosts[i] > fishGold)
            {
                rightIndex = i;
                break;
            }
        }
        rightIndex = Math.Max(bulletCosts.Length - 1, rightIndex);

        // 子弹价值比鱼价值更大
        // 每相差一个等级, 概率增加1%
        if (bulletIndex >= rightIndex)
            return Math.Max(100, bulletIndex - rightIndex);

        // 子弹价值比鱼价值小
        // 每相差一个等级, 概率降低2%
        return Math.Min(-100, (bulletIndex - rightIndex) * 2);
    }

    /// <summary>
    /// 子弹价格下标
    /// </summary>
    private int costIndex = 0;

    /// <summary>
    /// 枪下标
    /// </summary>
    private int gunIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        ScrollWheelChangeBullet();
        Fire();
    }

    /// <summary>
    /// 开炮
    /// 1. 根据炮台选择子弹组
    /// 2. 根据等级选择使用子弹组中哪个子弹
    /// </summary>
    void Fire()
    {

        fireTimer += Time.deltaTime;

        // 防止UI穿透
        // 解释: 解决点击UI部分也会开火
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // 鼠标左键发射子弹
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            // 子弹开火间隔
            if (fireTimer < fireInterval)
                return;
            fireTimer = 0;

            // 尝试扣除子弹费用
            if (!LevelManager.Instance.TryDeductBulletCost(bulletCosts[costIndex]))
                return;

            GameObject[] bulletsGroup;
            switch (gunIndex)
            {
                case 0: bulletsGroup = bullet1Prefabs; break;
                case 1: bulletsGroup = bullet2Prefabs; break;
                case 2: bulletsGroup = bullet3Prefabs; break;
                case 3: bulletsGroup = bullet4Prefabs; break;
                default: bulletsGroup = bullet5Prefabs; break;
            }

            // 每组子弹都是10个元素
            const int BULLET_LENGTH_IN_ONCE_GROUP = 10;
            int bulletIndex = LevelManager.Instance.level % BULLET_LENGTH_IN_ONCE_GROUP;

            // 创建子弹
            GameObject bullet = Instantiate(bulletsGroup[bulletIndex]);
            bullet.transform.SetParent(bulletHolderTransform, false);
            Transform firePos = gunGos[gunIndex].transform;
            AudioManager.Instance.fireClip.PlayAudio();

            // 开炮特效
            GameObject fireEffe = Instantiate(fireEffect);
            fireEffe.transform.SetParent(firePos, false);
            fireEffe.transform.localPosition = new Vector3(0, 0.75f, 0);
            fireEffe.transform.localRotation = firePos.localRotation;

            // 子弹没有被UGUI托管, 需要设置3d坐标系的 position/rotation 而不是 localPosition/localRotation
            // 子弹创建位置与开火位置保持一致
            bullet.transform.position = firePos.Find("FirePos").position;
            bullet.transform.rotation= firePos.rotation;

            #region 添加脚本
            // 从子弹属性中提取子弹速度
            BulletAttribute bulletAttribute = bullet.GetComponent<BulletAttribute>();
            bulletAttribute.bulletTypeIndex = bulletCosts[costIndex];

            // 子弹Z轴是前进方向
            EffectAutoMove autoMove = bullet.AddComponent<EffectAutoMove>();
            autoMove.dir = Vector3.up; 
            autoMove.speed = bulletAttribute.speed;

            #endregion


        }
    }

    /// <summary>
    /// 鼠标滚轮方式切换子弹
    /// </summary>
    private void ScrollWheelChangeBullet()
    {
        float val = Input.GetAxis("Mouse ScrollWheel");
        if (0 > val) OnBulletDown();
        else if (0 < val) OnBulletUp();
    }

    /// <summary>
    /// 子弹升级
    /// </summary>
    public void OnBulletUp()
    {
        // 通过枪的隐藏显示来控制枪的升级降级
        // 隐藏原来的枪
        gunGos[gunIndex].SetActive(false);

        // 可以从最大枪转换到最小枪
        costIndex++;
        if (bulletCosts.Length <= costIndex)
            costIndex = 0;

        // 显示现在的枪
        gunIndex = costIndex / 4;
        gunGos[gunIndex].SetActive(true);

        // 换枪特效
        Instantiate(changeGunEffect);
        AudioManager.Instance.changeClip.PlayAudio();

        // 刷新单价
        bulletCostText.text = "$" + bulletCosts[costIndex];
    }

    /// <summary>
    /// 子弹降级
    /// </summary>
    public void OnBulletDown()
    {
        // 通过枪的隐藏显示来控制枪的升级降级
        // 隐藏原来的枪
        gunGos[gunIndex].SetActive(false);

        // 可以从最小枪转换到最大枪
        costIndex--;
        if (0 >= costIndex)
            costIndex = bulletCosts.Length - 1;

        // 显示现在的枪
        gunIndex = costIndex / 4;
        gunGos[gunIndex].SetActive(true);

        // 换枪特效
        Instantiate(changeGunEffect);

        // 刷新单价
        bulletCostText.text = "$" + bulletCosts[costIndex];
    }

}
