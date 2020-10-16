using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水波纹
/// </summary>
public class EffectWaterWave : MonoBehaviour
{

    [Header("水波纹贴图")]
    public Texture[] textures;
    private Material material;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        InvokeRepeating(nameof(ChangeWave), 0, 0.1f);
    }

    // Update is called once per frame
    void ChangeWave()
    {
        material.mainTexture = textures[index];
        index = (index + 1) % textures.Length;
    }
}
