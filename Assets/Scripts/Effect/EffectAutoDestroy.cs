using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动销毁
/// </summary>
public class EffectAutoDestroy : MonoBehaviour
{

    [Header("延迟时间")]
    public float dalayTime = 1f;

    private void Start()
    {
        Destroy(gameObject, dalayTime);
    }

}

