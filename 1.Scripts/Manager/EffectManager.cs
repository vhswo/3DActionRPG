using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingtonMono<EffectManager>
{
    static public EffectData effectData;
    Dictionary<int, ObjectPool<GameObject>> effects = new ();
    public int CreateClip = 5; // �ѹ��� ������ ��
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
    /// effect Ǯ���� index��°���� CreateClip ��ŭ �߰�
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
               Debug.LogWarning("�������� ����ֽ��ϴ�. Tools���� effect�� �߰����ּ���");
           }
        }
    }

    /// <summary>
    /// ������ƮǮ���� �߰�
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