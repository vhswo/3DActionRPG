using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingtonMono<ItemManager>
{
    public static ItemData itemDatas;
    Dictionary<int, ObjectPool<GameObject>> items = new();

    public static ItemData itemData()
    {
        if(itemDatas == null)
        {
            itemDatas = ScriptableObject.CreateInstance<ItemData>();
            itemDatas.Load();
        }

        return itemDatas;
    }

    private void Start()
    {
        itemData();
    }

    //��üȭ & ����Ǯ�� ����
    public void AddItemObj(int index)
    {
        ItemClip item = itemDatas.GetCopy(index);
        GameObject obj = Instantiate(item.itemPrefab, transform);
        obj.SetActive(false);
        items[index].Add(obj);
    }


    //�ʵ� ��üȭ
    public GameObject GetItem(int index, Vector3 pos)
    {
        GameObject obj = items[index].Use();
        obj.SetActive(true);
        obj.transform.position = pos;
        return obj;
    }

    //�����͸� �ʿ��ҋ�
    public ItemClip GetItemClip(string name)
    {
        for(int i =0; i < items.Count; i++)
        {
            if(itemDatas.clips[i].ClipName == name)
            {
                return itemDatas.clips[i] as ItemClip;
            }
        }
        return null;
    }

    public ItemClip GetItemClip(ItemList type)
    {
        return itemDatas.clips[(int)type] as ItemClip;
    }

    IEnumerator CallOverPooling(float time)
    {
        yield return new WaitForSeconds(time);
    }
/*
    public void DropItem(int index, Vector3 pos)
    {
        GameObject obj = items[index].Use();

        if (obj == null) { }//�������߰�

        obj.SetActive(true);
        obj.transform.SetParent(gameObject.transform.root);
        pos.y = 0.0f;
        obj.transform.position = pos;
    }*/
}
