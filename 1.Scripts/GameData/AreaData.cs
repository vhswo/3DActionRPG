using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCController : MonoBehaviour
{
    public int realID;
    public AreaData BelongVilage = null;

    private void Awake()
    {
        if(BelongVilage != null)
        {
            BelongVilage.AddPeople(this);
        }
    }

    public void ChangeVilage(AreaData area)
    {
        if(BelongVilage != null)
        {
            BelongVilage.RemovePeople(realID);
        }

        BelongVilage = area;
        realID = area.AddPeople(this) - 1;
    }
}

/// <summary>
/// �⺻Ʋ�� ���� �����ؾ���
/// VilagesArea ������ StartPoint�� �ְ�
/// SoundList�� VilagesArea�� �ڽĵ��� �̸��� �� �뷡�̴�
/// FieldSound�� �׻� ��ũ��Ʈ�� ������ 0��° �ڽ��̾���Ѵ�
/// </summary>
public class AreaData : MonoBehaviour
{
    private static SoundList FieldSound = SoundList.None;
    private string VilageName;
    private string FieldName;
    private BoxCollider VilagesArea = null;
    private Vector3 StartPoint = Vector3.zero;
    private SoundList areaSound = SoundList.None;
    public NPCController[] peoples = new NPCController[0];
    private bool BGMPlay = false;

    private void Start()
    {
        Transform myTransform = transform;
        VilageName = gameObject.name;
        VilagesArea = GetComponent<BoxCollider>();
        StartPoint = myTransform.Find("StartPoint").transform.position;
        areaSound = (SoundList)Enum.Parse(typeof(SoundList), VilageName);
        FieldName = transform.parent.name;
        if (FieldSound == SoundList.None) FieldSound = (SoundList)Enum.Parse(typeof(SoundList), FieldName);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!BGMPlay && areaSound != SoundList.None && other.transform.CompareTag("Player"))
        {
            SoundManager.instance.PlayBGMSound((int)areaSound);
            BGMPlay = true;
        }

        if(other.transform.CompareTag("Enemy"))
        {
            //���ڸ��� ���ư����ϱ�
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (BGMPlay && FieldSound != SoundList.None && other.transform.CompareTag("Player"))
        {
            SoundManager.instance.PlayBGMSound((int)FieldSound);
            BGMPlay = false;
        }

    }

    public int GetPeopleCount()
    {
        if (peoples == null) return 0;

        return peoples.Length;
    }

    public int AddPeople(NPCController people)
    {
        if (peoples == null)
            peoples = new NPCController[] { new NPCController() };
        else
            peoples = ArrayHelper.helperAdd(people, peoples);

        return GetPeopleCount();
    }

    public void RemovePeople(int index)
    {
        peoples = ArrayHelper.helperRemove(index, peoples);
    }
}
