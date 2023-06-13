using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    NONE = -1,
    NORMAL,
}

/// <summary>
/// id, type,prefab,effectname,effecpath 
/// </summary>
public class EffectClip : BaseClip
{
    public GameObject ClipPrefab = null;
    public EffectType effectType = EffectType.NONE;
    public float realrizeTime = 0.0f;

    public override void PreLoad()
    {
        ClipFileFullPath = ClipFilePath + ClipFileName;

        if (ClipFileFullPath != string.Empty && (ClipPrefab == null || (ClipPrefab != null && ClipPrefab.name != ClipName)))
        {
            ClipPrefab = Resources.Load(ClipFileFullPath) as GameObject;
        }
    }


    public IEnumerator Disappear(int index)
    {
        if (realrizeTime <= 0.0f) realrizeTime = 0.5f;
        yield return new WaitForSeconds(realrizeTime);
        EffectManager.instance.AddObjectPooling(index,ClipPrefab);
    }

}
