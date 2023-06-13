using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.Text;

/// <summary>
/// 현재 뜨는 오류는 반복구간쪽에서 remove로 인해 end를 하지 못하면서 오류가 나고 있다     
/// </summary>

public class SoundTool : EditorWindow
{
    public int uiWidthLarge = 400;
    public int uiWidthMiddle = 300;
    public int uiWidthSmall = 200;

    private int selection = 0;
    private Vector2 SP1 = Vector2.zero;
    private Vector2 SP2 = Vector2.zero;
    private AudioClip soundSource;
    private static SoundData soundData;

    [MenuItem("Tools/SoundTool")]
    static void Init()
    {
        soundData = CreateInstance<SoundData>();
        soundData.Load();

        SoundTool window = GetWindow<SoundTool>(false, "Sound Tool");
        window.Show();
    }

    private void OnGUI()
    {
        if (soundData == null) return;

        EditorGUILayout.BeginVertical();
        {
            UnityObject source = soundSource;
            ToolHelper.ToolTopLayer(soundData, ref selection, ref source, uiWidthMiddle);
            soundSource = (AudioClip)source;

            EditorGUILayout.BeginHorizontal();
            {
                ToolHelper.ToolMiddleLayerTypeList(ref SP1, soundData, ref selection, ref source, uiWidthMiddle);
                soundSource = (AudioClip)source;

                EditorGUILayout.BeginVertical();
                {
                    SP2 = EditorGUILayout.BeginScrollView(SP2);
                    {
                        if(soundData.GetDataCount() > 0)
                        {
                            SoundClip sound = soundData.clips[selection] as SoundClip;

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.LabelField("ID : ", selection.ToString(), GUILayout.Width(uiWidthLarge));
                                sound.ClipName = EditorGUILayout.TextField("이름 : ", sound.ClipName, GUILayout.Width(uiWidthLarge));
                                sound.soundType = (SoundType)EditorGUILayout.EnumPopup("soundType : ", sound.soundType, GUILayout.Width(uiWidthLarge));
                                sound.MaxVolume = EditorGUILayout.FloatField("maxVolum : ", sound.MaxVolume, GUILayout.Width(uiWidthLarge));
                                sound.isLoop = EditorGUILayout.Toggle("IsLoop : ", sound.isLoop, GUILayout.Width(uiWidthLarge));
                                EditorGUILayout.Separator();


                                if (soundSource == null && sound.ClipName != string.Empty)
                                {
                                    soundSource = Resources.Load(sound.ClipFilePath + sound.ClipFileName) as AudioClip;

                                }
                                soundSource = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", soundSource, typeof(AudioClip), false, GUILayout.Width(uiWidthLarge));

                                if (soundSource != null)
                                {
                                    sound.ClipFilePath = ToolHelper.GetPath(soundSource);
                                    sound.ClipFileName = soundSource.name;
                                    sound.pitch = EditorGUILayout.Slider("Pitch", sound.pitch, -3.0f, 3.0f, GUILayout.Width(uiWidthLarge));
                                    sound.dopplerLevel = EditorGUILayout.Slider("Doppler", sound.dopplerLevel, 0.0f, 5.0f, GUILayout.Width(uiWidthLarge));
                                    sound.rolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("volume Rolloff", sound.rolloffMode, GUILayout.Width(uiWidthLarge));
                                    sound.minDistance = EditorGUILayout.FloatField("min Distance", sound.minDistance, GUILayout.Width(uiWidthLarge));
                                    sound.maxDistance = EditorGUILayout.FloatField("max Distance", sound.maxDistance, GUILayout.Width(uiWidthLarge));
                                    sound.spartialBlend = EditorGUILayout.Slider("PanLevel", sound.spartialBlend, 0.0f, 1.0f, GUILayout.Width(uiWidthLarge));
                                }
                                else
                                {
                                    sound.ClipFilePath = string.Empty;
                                    sound.ClipFileName = string.Empty;
                                }
                                EditorGUILayout.Separator();

                                if (GUILayout.Button("Add Loop", GUILayout.Width(uiWidthMiddle)))
                                {
                                    sound.AddLoop();
                                }

                                for (int i = 0; i < sound.checkTime.Length; i++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    {
                                        GUILayout.Label("Loop Step" + i, EditorStyles.boldLabel);
                                        if (GUILayout.Button("Remove", GUILayout.Width(uiWidthMiddle)))
                                        {
                                            sound.RemoveLoop(i);
                                            return;
                                        }
                                        sound.checkTime[i] = EditorGUILayout.FloatField("check Time", sound.checkTime[i], GUILayout.Width(uiWidthMiddle));
                                        sound.setTime[i] = EditorGUILayout.FloatField("set Time", sound.setTime[i], GUILayout.Width(uiWidthMiddle));

                                    }
                                    EditorGUILayout.EndVertical();
                                }

                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();
        //
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Reload"))
            {
                soundData = CreateInstance<SoundData>();
                soundData.Load();
                selection = 0;
                soundSource = null;
            }
            if (GUILayout.Button("Save"))
            {
                soundData.Save();
                CreateEnumStructure();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    public void CreateEnumStructure()
    {
        string enumName = "SoundList";
        StringBuilder builder = new();
        for (int i = 0; i < soundData.clips.Length; i++)
        {
            if (!soundData.clips[i].ClipName.ToLower().Contains("none"))
            {
                builder.AppendLine($"     {soundData.clips[i].ClipName} = {i.ToString()},");
            }
        }
        ToolHelper.CreateEnumStructure(enumName, builder);
    }

}
