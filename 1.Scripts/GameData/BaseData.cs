using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// data의 기본 클래스
/// </summary>
public class BaseData : ScriptableObject
{
    public const string dataDirectory = "/9.ResourcesData/Resources/Data/";
                                       
    public BaseClip[] clips = null;

    public int GetDataCount()
    {
        int Count = 0;

        if (clips != null) Count = clips.Length;

        return Count;
    }

    public string[] GetNameList(bool ShowID,string filterWord = "")
    {
        string[] nameList = new string[0];

        if (clips == null) return nameList;

        nameList = new string[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            if(filterWord != "")
            {
                if(clips[i].ClipName.ToLower().Contains(filterWord.ToLower()) == false)
                    continue;
            }

            if(ShowID)
                nameList[i] = $"{i} : {clips[i].ClipName}";
            else 
                nameList[i] = clips[i].ClipName;
        }

        return nameList;
    }

    public string helperJson(string add, string array)
    {
        if (array == string.Empty)
        {
            array = add;
            return array;
        }

        array += "\n" + add;

        return array;
    }

    /// <summary>
    /// 한줄씩 읽어온다
    /// </summary>
    public List<string> helperJsonLoad(string path)
    {
        List<string> data = new();

        if (!File.Exists(path)) return default;
        
        foreach (string line in File.ReadLines(path))
        {
            data.Add(line);
        }

        return data;
    }

    public virtual int AddDate(string newName) { return GetDataCount(); }
    public virtual void RemoveData(int index) { }
    public virtual void Copy(int index) { }
}
