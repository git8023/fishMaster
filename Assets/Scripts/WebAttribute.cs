using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAttribute : MonoBehaviour
{

    [Header("持续时间")]
    [Tooltip("在指定时间内, 所有进入该区域的鱼都受到一次伤害")]
    public float durationTime = 0.5f;

    //[Header("伤害")]
    //public int demage;

    [Header("子弹类型下标")]
    public int bulletTypeIndex;

    private void Start()
    {
        Destroy(gameObject, durationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // 网碰到鱼, 对鱼造成伤害
        if (collision.CompareTag("Fish"))
        {
            FishAttribute fishAttribute = collision.GetComponent<FishAttribute>();
            //fishAttribute.TakeDemage(bulletTypeIndex);
            fishAttribute.HitByProbability(bulletTypeIndex);
        }
    }

}
