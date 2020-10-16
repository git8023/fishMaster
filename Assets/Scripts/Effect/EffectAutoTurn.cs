using UnityEngine;
using System.Collections;

/// <summary>
/// 在Z轴旋转
/// </summary>
public class EffectAutoTurn : MonoBehaviour
{

    /// <summary>
    /// 角速度
    /// </summary>
    public float anguleSpeed = 0f;

    void Update()
    {
        // 在z轴旋转
        transform.Rotate(Vector3.forward, anguleSpeed * Time.deltaTime);
    }
}