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
    /// ���� �ε� ��Ȱ, �����Ҷ� ���� �������� �ʱ⿡ path�� name���� clip�� ã�ƿ´�
    /// </summary>
    public virtual void PreLoad() { }
}
