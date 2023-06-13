using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SoundData : BaseData
{
    private string path;
    private string saveName = "sounds";

    public void Save()
    {
        path = Application.dataPath + dataDirectory;

        if (clips.Length <= 0) return;

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

        clips = new SoundClip[arrayData.Count];

        for (int i = 0; i < arrayData.Count; i++)
        {
            clips[i] = JsonUtility.FromJson<SoundClip>(arrayData[i]);
        }

        foreach (SoundClip clip in clips)
        {
            clip.PreLoad();
        }
    }

    public override int AddDate(string newName)
    {
        if(clips == null)
        {
            clips = new SoundClip[] { new SoundClip() };
            clips[0].ClipName = newName;
        }
        else
        {
            clips = ArrayHelper.helperAdd(new SoundClip(), clips);
            clips[clips.Length - 1].ClipName = newName;
        }

        return GetDataCount();
    }

    public override void RemoveData(int index)
    {
        if (clips == null || clips.Length <= 0) return;

        clips = ArrayHelper.helperRemove(index, clips);
    }

    public SoundClip GetCopy(int index)
    {
        if (index >= clips.Length || index < 0) return null;

        string origin = JsonUtility.ToJson(clips[index]);

        SoundClip tmp = JsonUtility.FromJson<SoundClip>(origin);

        tmp.realID = index;

        tmp.PreLoad();

        return tmp;
    }

    public override void Copy(int index)
    {

        clips = ArrayHelper.helperAdd(GetCopy(index), clips);
    }

}
