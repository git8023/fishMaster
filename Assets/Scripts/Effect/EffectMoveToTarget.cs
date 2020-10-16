using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动到目的地
/// </summary>
public class EffectMoveToTarget : MonoBehaviour
{

    [Header("目的地")]
    public GameObject destGo;

    [Header("速度")]
    public float speed = 10;

    private void Awake()
    {
        destGo = GameObject.Find("GoldCollect");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destGo.transform.position, speed * Time.deltaTime);
    }
}
