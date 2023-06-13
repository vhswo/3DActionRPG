using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;
using System.Text;
using System.IO;


public class ToolHelper
{
    public static string GetPath(UnityObject p_clip)
    {
        string retString = string.Empty;
        retString = AssetDatabase.GetAssetPath(p_clip);
        string[] path_node = retString.Split('/');
        bool findResource = false;
        for (int i = 0; i < path_node.Length - 1; i++)
        {
            if (findResource == false)
            {
                if (path_node[i] == "Resources")
                {
                    findResource = true;
                    retString = string.Empty;
                }
            }
            else
            {
                retString += path_node[i] + "/";
            }

        }

        return retString;
    }

    public static void CreateEnumStructure(string enumName, StringBuilder data)
    {
        string templateFilePath = "Assets/Editor/EnumTemplate.txt";

        string entittyTemplate = File.ReadAllText(templateFilePath);

        entittyTemplate = entittyTemplate.Replace("$DATA$", data.ToString());
        entittyTemplate = entittyTemplate.Replace("$ENUM$", enumName);
        string folderPath = "Assets/1.Scripts/GameData/";
        if (Directory.Exists(folderPath) == false)
        {
            Directory.CreateDirectory(folderPath);
        }

        string FilePath = folderPath + enumName + ".cs";
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
        File.WriteAllText(FilePath, entittyTemplate);
    }

    public static void ToolTopLayer(BaseData data,ref int selection,ref UnityObject source,ref Sprite icon, int uiWidth)
    {
        EditorGUILayout.BeginHorizontal();
        {
            if(GUILayout.Button("Add",GUILayout.Width(uiWidth)))
            {
                data.AddDate("NewData");
                selection = data.GetDataCount() - 1;
                source = null;
                icon = null;
            }

            if (GUILayout.Button("Copy",GUILayout.Width(uiWidth)))
            {
                data.Copy(selection);
                selection = data.GetDataCount() - 1;
                source = null;
                icon = null;
            }

            if(data.GetDataCount() > 0)
            {
                if(GUILayout.Button("Remove",GUILayout.Width(uiWidth)))
                {
                    source = null;
                    icon = null;
                    data.RemoveData(selection);
                }
            }

            if(selection > data.GetDataCount() - 1)
            {
                selection = data.GetDataCount() - 1;
            }


        }
        EditorGUILayout.EndHorizontal();
    }

    public static void ToolMiddleLayerTypeList(ref Vector2 ScrollPos, BaseData data, ref int selection, ref UnityObject source, ref Sprite icon, int uiWidth)
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("box");
            {
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
                {
                    if(data.GetDataCount() > 0)
                    {
                        int lastSelection = selection;
                        selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
                        if(lastSelection != selection)
                        {
                            source = null;
                            icon = null;
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

    public static void ToolTopLayer(BaseData data, ref int selection, ref UnityObject source, int uiWidth)
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add", GUILayout.Width(uiWidth)))
            {
                data.AddDate("NewData");
                selection = data.GetDataCount() - 1;
                source = null;
            }

            if (GUILayout.Button("Copy", GUILayout.Width(uiWidth)))
            {
                data.Copy(selection);
                selection = data.GetDataCount() - 1;
                source = null;
            }

            if (data.GetDataCount() > 0)
            {
                if (GUILayout.Button("Remove", GUILayout.Width(uiWidth)))
                {
                    source = null;
                    data.RemoveData(selection);
                }
            }

            if (selection > data.GetDataCount() - 1)
            {
                selection = data.GetDataCount() - 1;
            }


        }
        EditorGUILayout.EndHorizontal();
    }

    public static void ToolMiddleLayerTypeList(ref Vector2 ScrollPos, BaseData data, ref int selection, ref UnityObject source, int uiWidth)
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical("box");
            {
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
                {
                    if (data.GetDataCount() > 0)
                    {
                        int lastSelection = selection;
                        selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
                        if (lastSelection != selection)
                        {
                            source = null;
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }
}
