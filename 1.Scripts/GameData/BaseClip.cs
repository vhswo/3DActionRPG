using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseClip
{
    public string ClipName = string.Empty;
    public string ClipFileName = string.Empty;
    public string ClipFilePath = string.Empty;
    public string ClipFileFullPath = string.Empty;
    public int realID = 0;


    /// <summary>
    /// 사전 로딩 역활, 저장할때 따로 저장하지 않기에 path와 name으로 clip을 찾아온다
    /// </summary>
    public virtual void PreLoad() { }
}
