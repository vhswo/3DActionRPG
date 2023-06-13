using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingtonMono<EffectManager>
{
    static public EffectData effectData;
    Dictionary<int, ObjectPool<GameObject>> effects = new ();
    public int CreateClip = 5; // 한번에 생산할 양
    public Transform PrefabsBox;

    private void Start()
    {
        EffectData();

        if (effectData.clips == null) return;

       for(int i = 0; i < effectData.clips.Length; i++)
       {
           effects.Add(i, new());
           AddEffectObj(i);
       }
    }

    public static EffectData EffectData()
    {
        if (effectData == null)
        {
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.Load();
        }

        return effectData;
    }

    /// <summary>
    /// effect 풀링에 index번째꺼를 CreateClip 만큼 추가
    /// </summary>
    public void AddEffectObj(int index)
    {
        EffectClip clip = effectData.GetEffect(index);

        for (int i = 0; i < CreateClip; i++)
        {
            try
            {
                AddObjectPooling(index, clip.ClipPrefab);
            }
           catch
           {
               Debug.LogWarning("프리팹이 비어있습니다. Tools에서 effect를 추가해주세요");
           }
        }
    }

    /// <summary>
    /// 오브젝트풀링에 추가
    /// </summary>
    public void AddObjectPooling(int index,GameObject clip)
    {
        Transform Box = PrefabsBox;
        if (Box == null) Box = transform.Find("EffectManager/Effects");

        GameObject obj = Instantiate(clip, Box);

        obj.SetActive(false);
        effects[index].Add(obj);
    }


    public GameObject EffectOneShot(int index,Vector3 pos)
    {
        GameObject effect = effects[index].Use();

        if(effect == default)
        {
            AddEffectObj(index);
        }

        effect.transform.position = pos;
        effect.gameObject.SetActive(true);

        EffectClip clip = effectData.GetEffect(index);
        StartCoroutine(clip.Disappear(index));

        return effect;
    }

}