using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;
using System.Text;

public class EffectTool : EditorWindow
{
    public int uiWidthLarge = 400;
    public int uiWidthMiddle = 300;
    public int uiWidthSmall = 200;
    private int selection = 0;
    private Vector2 ScrollA = Vector2.zero;
    private Vector2 ScrollB = Vector2.zero;

    private GameObject effectSource = null;
    static private EffectData effectData;
    [MenuItem("Tools/Effect Tool")]
    static void Init()
    {
        effectData = CreateInstance<EffectData>();
        effectData.Load();

        EffectTool window = GetWindow<EffectTool>(false, "Effect Tool");
        window.Show();
    }

    private void OnGUI()
    {
        if (effectData == null) return;

        EditorGUILayout.BeginVertical();
        {
            UnityObject source = effectSource;
            //TOP
            ToolHelper.ToolTopLayer(effectData, ref selection, ref source, uiWidthSmall);
            effectSource = (GameObject)source;

            EditorGUILayout.BeginHorizontal();
            {
                //middle left
                ToolHelper.ToolMiddleLayerTypeList(ref ScrollA, effectData, ref selection, ref source, uiWidthSmall);
                effectSource = (GameObject)source;

                //middle right
                ScrollB = EditorGUILayout.BeginScrollView(ScrollB);
                {
                    if(effectData.GetDataCount() > 0)
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.Separator();

                            EditorGUILayout.LabelField("ID : ", selection.ToString(), GUILayout.Width(uiWidthMiddle));
                            EffectClip selectClip = effectData.clips[selection] as EffectClip;
                            selectClip.ClipName = EditorGUILayout.TextField("¿Ã∏ß : ", selectClip.ClipName, GUILayout.Width(uiWidthLarge));
                            selectClip.effectType = (EffectType)EditorGUILayout.EnumPopup("¿Ã∆Â∆Æ ≈∏¿‘ : ", selectClip.effectType, GUILayout.Width(uiWidthLarge));
                            EditorGUILayout.Separator();

                            if(effectSource == null && selectClip.ClipFileName != string.Empty)
                            {
                                selectClip.PreLoad();
                                effectSource = Resources.Load(selectClip.ClipFilePath + selectClip.ClipFileName) as GameObject;
                            }
                            effectSource = (GameObject)EditorGUILayout.ObjectField("¿Ã∆Â∆Æ : ", effectSource, typeof(GameObject), false, GUILayout.Width(uiWidthLarge));

                            if(effectSource != null)
                            {
                                selectClip.ClipFilePath = ToolHelper.GetPath(effectSource);
                                selectClip.ClipFileName = effectSource.name;
                            }
                            else
                            {
                                selectClip.ClipFileName = string.Empty;
                                selectClip.ClipFilePath = string.Empty;
                                effectSource = null;
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndScrollView();

            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();

        //Bottom
        EditorGUILayout.BeginHorizontal();
        {
            if(GUILayout.Button("ReloadSetting"))
            {
                effectData = CreateInstance<EffectData>();
                effectData.Load();
                selection = 0;
                effectSource = null;
            }
            
            if(GUILayout.Button("Save"))
            {
                effectData.Save();
                CreateEnumStructure();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public void CreateEnumStructure()
    {
        string enumName = "EffectList";
        StringBuilder builder = new StringBuilder();
        builder.AppendLine();
        for (int i = 0; i < effectData.clips.Length; i++)
        {
            if (effectData.clips[i].ClipName != string.Empty)
            {
                builder.AppendLine($"       { effectData.clips[i].ClipName} = {i} ,");
            }
        }
        ToolHelper.CreateEnumStructure(enumName, builder);
    }
}
