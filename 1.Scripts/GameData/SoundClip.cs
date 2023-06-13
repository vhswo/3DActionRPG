using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    None = -1,
    BGM,
    EFFECT,
    UI
}

public class SoundClip : BaseClip
{
    public SoundType soundType = SoundType.None;
    public float MaxVolume = 1.0f;
    public bool isLoop =  false;
    public float[] checkTime = new float[0];
    public float[] setTime = new float[0];

    private AudioClip clip = null;
    public int currentLoop = 0;
    public float pitch = 1.0f;
    public float dopplerLevel = 1.0f;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic; //���尡 ���̵� �Ǵ� �ӵ��� ��Ÿ���� �Ÿ������� ���� �Ҹ��� �۾�����
    public float minDistance = 10000.0f;
    public float maxDistance = 50000.0f;
    public float spartialBlend = 1.0f;  //3D ������ ������ҽ��� ��ġ�� ȿ���� ���� 1�̸� �̹�

    Interpolate.Function Interpolate_Func;
    public float fadeTime1 = 0.0f;
    public float fadeTime2 = 0.0f;
    public bool isFadeIn = false;
    public bool isFadeOut = false;

    public override void PreLoad()
    {
        ClipFileFullPath = ClipFilePath + ClipFileName;
        if (ClipFileFullPath != string.Empty && (clip == null || (clip != null && clip.name != ClipName)))
        {
            clip = Resources.Load(ClipFileFullPath) as AudioClip;
        }
    }

    public void AddLoop()
    {
        checkTime = ArrayHelper.helperAdd(0.0f, checkTime);
        setTime = ArrayHelper.helperAdd(0.0f, setTime);
    }

    public void RemoveLoop(int index)
    {
        checkTime = ArrayHelper.helperRemove(index, checkTime);
        setTime = ArrayHelper.helperRemove(index, setTime);
    }

    public AudioClip GetClip()
    {
        if(clip == null)
        {
            PreLoad();
        }

        if(clip == null && ClipFileName != string.Empty)
        {
            Debug.LogWarning($"����ġ ���� ���� Ȯ�� : {ClipFileName}");
            return null;
        }

        return clip;
    }

    public bool HasLoop()
    {
        return checkTime.Length > 0;
    }

    public void NextLoop()
    {
        currentLoop++;
        if(currentLoop >= checkTime.Length)
        {
            currentLoop = 0;
        }
    }

    public void CheckLoop(AudioSource source)
    {
        if(HasLoop() && source.time >= checkTime[currentLoop])
        {
            source.time = setTime[currentLoop];
            NextLoop();
        }
    }

    public void FadeIn(float time, Interpolate.EaseType easeType)
    {
        isFadeOut = false;
        fadeTime1 = 0.0f;
        fadeTime2 = time;
        Interpolate_Func = Interpolate.Ease(easeType);
        isFadeIn = true;
    }

    public void FadeOut(float time, Interpolate.EaseType easeType)
    {
        isFadeIn = false;
        fadeTime1 = 0.0f;
        fadeTime2 = time;
        Interpolate_Func = Interpolate.Ease(easeType);
        isFadeOut = true;
    }

    public void DoFade(float time, AudioSource source)
    {
        int Fade = isFadeIn ? 1 : isFadeOut ? 2 : 0;

        switch(Fade)
        {
            case 1:
                fadeTime1 += time;
                source.volume = Interpolate.Ease(Interpolate_Func, 0, MaxVolume, fadeTime1, fadeTime2);
                if (fadeTime1 >= fadeTime2)
                {
                    isFadeIn = false;
                }
                break;
            case 2:
                fadeTime1 += time;
                source.volume = Interpolate.Ease(Interpolate_Func, MaxVolume, 0 - MaxVolume, fadeTime1, fadeTime2);
                if (fadeTime1 >= fadeTime2)
                {
                    isFadeOut = false;
                    source.Stop();
                }
                break;
            default:
                break;
        }

    }

}
