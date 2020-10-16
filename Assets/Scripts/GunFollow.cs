using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 枪口跟随鼠标旋转
public class GunFollow : MonoBehaviour
{
    [Header("UGUI面板")]
    public RectTransform UGUITransform;

    [Header("用于计算的摄像机")]
    public Camera mainCamera;

    void Update()
    {
        // 鼠标在UGUI上的位置
        Vector2 screenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        // 鼠标在世界坐标系上的位置
        Vector3 mousePos;

        // 把鼠标在UGUI中的位置转换为世界坐标系中的位置
        RectTransformUtility.ScreenPointToWorldPointInRectangle(UGUITransform, screenPoint, mainCamera, out mousePos);

        // 计算鼠标位置和当前武器夹角
        // 夹角范围: 0~180
        // 所以需要区分左右
        float angle = Vector3.Angle(Vector3.up, mousePos - transform.position);
        bool atRight = (mousePos.x > transform.position.x);
        if (atRight)
            angle = -angle;

        // GUN托管在UGUI下必须使用 transform.localRotation
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
