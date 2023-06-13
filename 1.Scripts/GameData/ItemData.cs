using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemData : BaseData
{
    public string path;
    private string saveName = "Items";

    public void Save()
    {
        path = Application.dataPath + dataDirectory;

        if (clips.Length <=  0) return;

        string array = string.Empty;

        for(int i = 0; i < clips.Length; i++)
        {
            string tmp = JsonUtility.ToJson(clips[i]);
            array = helperJson(tmp, array);
        }

        File.WriteAllText(path + saveName, array);
    }

    public void Load()
    {
        path = Application.dataPath + dataDirectory;

        List<string> array = helperJsonLoad(path + saveName);

        if (array == null || array.Count <= 0) return;

        clips = new ItemClip[array.Count];


        for(int i = 0; i < array.Count; i++)
        {
            clips[i] = JsonUtility.FromJson<ItemClip>(array[i]);
        }

        foreach (ItemClip item in clips)
        {
            item.PreLoad();
        }
    }

    public override int AddDate(string newName)
    {
        if(clips == null)
        {
            clips = new ItemClip[] { new ItemClip() };
            clips[0].ClipName = newName;
        }
        else
        {
            clips = ArrayHelper.helperAdd(new ItemClip(), clips);
            clips[clips.Length - 1].ClipName = newName;
        }

        return GetDataCount();
    }

    public override void RemoveData(int index)
    {
        if (clips == null || clips.Length <= 0) return;

        clips = ArrayHelper.helperRemove(index, clips);
    }

    public ItemClip GetCopy(int index)
    {
        if (index >= clips.Length || index < 0) return null;

        string origin = JsonUtility.ToJson(clips[index]);

        ItemClip tmp = JsonUtility.FromJson<ItemClip>(origin);

        tmp.realID = index;
        tmp.PreLoad();

        return tmp;
    }

    public override void Copy(int index)
    {
        clips = ArrayHelper.helperAdd(GetCopy(index), clips);
    }
}
