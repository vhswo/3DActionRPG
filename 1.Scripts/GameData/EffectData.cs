using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class EffectData : BaseData
{
    public string path;
    private string saveName = "effect";

    public void Save()
    {
        path = Application.dataPath + dataDirectory;

        if (clips.Length <= 0) return; //세이브할게 없으면 세이브하지 않음

        string array = string.Empty;

        for (int i = 0; i < clips.Length; i++)
        {
            string tmp = JsonUtility.ToJson(clips[i]);

            array = helperJson(tmp, array);
        }

        File.WriteAllText(path + saveName, array);

    }

    public void Load()
    {
        path = Application.dataPath + dataDirectory;

        List<string> arrayData = helperJsonLoad(path + saveName);

        if (arrayData == null || arrayData.Count <= 0) return;

        clips = new EffectClip[arrayData.Count];

        for (int i = 0; i < arrayData.Count; i++)
        {
            clips[i] = JsonUtility.FromJson<EffectClip>(arrayData[i]);
        }
    }

    public EffectClip GetEffect(int index)
    {
        if (index < 0 || index >= clips.Length) return null;

        (clips[index] as EffectClip).PreLoad();

        return clips[index] as EffectClip;
    }

    public override int AddDate(string newName)
    {
        if(clips == null)
        {
            clips = new EffectClip[] { new EffectClip()};
            clips[0].ClipName = newName;
        }
        else
        {
            clips = ArrayHelper.helperAdd(new EffectClip(), clips);
            clips[clips.Length - 1].ClipName = newName;
        }

        return GetDataCount();
    }

    public override void RemoveData(int index)
    {
        clips = ArrayHelper.helperRemove(index,clips);
    }

    public override void Copy(int index)
    {
        if (index < 0 || index >= clips.Length) return;

        string origin = JsonUtility.ToJson(clips[index]);
        EffectClip tmp = JsonUtility.FromJson<EffectClip>(origin);
        tmp.realID = clips.Length;

        clips = ArrayHelper.helperAdd(tmp, clips);
    }


}
