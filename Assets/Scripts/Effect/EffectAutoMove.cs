using UnityEngine;
using System.Collections;

/// <summary>
/// 向指定方向移动
/// </summary>
public class EffectAutoMove : MonoBehaviour
{

    /// <summary>
    /// 速度
    /// </summary>
    public float speed;

    /// <summary>
    /// 移动方向, 默认向X轴方向移动
    /// </summary>
    public Vector3 dir = Vector3.right;

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
