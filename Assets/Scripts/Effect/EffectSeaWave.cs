using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSeaWave : MonoBehaviour
{

    /// <summary>
    /// 结束位置
    /// </summary>
    private Vector3 endPos;

    void Start()
    {
        endPos = -transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, 10 * Time.deltaTime);
    }
}
