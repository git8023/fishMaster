using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHideSelf : MonoBehaviour
{

    [Header("延迟时间(秒)")]
    public int delaySecond;

    void Update()
    {
        if (gameObject.activeSelf)
            StartCoroutine(Hide());
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    /// <returns>携程对象</returns>
    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(delaySecond);
        gameObject.SetActive(false);
    }

}
