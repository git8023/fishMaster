using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttribute : MonoBehaviour
{

    // [Header("伤害值")]
    // public int damage;

    [Header("速度")]
    public int speed;

    [Header("网预制体")]
    public GameObject webPrefab;

    [Header("子弹类型下标")]
    public int bulletTypeIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 边界
        if (collision.CompareTag("Bounds"))
        {
            Destroy(gameObject);
            return;
        }

        // 击中鱼,生成网
        if (collision.CompareTag("Fish"))
        {
            GameObject web = Instantiate(webPrefab);
            web.GetComponent<WebAttribute>().bulletTypeIndex = bulletTypeIndex;
            web.transform.SetParent(transform.parent, false);
            web.transform.position = transform.position;
            Destroy(gameObject);
            return;
        }
    }

}
