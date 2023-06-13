using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityObjcet = UnityEngine.Object;
using System.Text;

public class ItemTool : EditorWindow
{
    public int uiWidthLarge = 400;
    public int uiWidthMiddle = 300;
    public int uiWidthSmall = 200;

    private int select = 0;
    private Vector2 SP1 = Vector2.zero;
    private Vector2 SP2 = Vector2.zero;

    private GameObject itemSource = null;
    private Sprite icon = null;
    static private ItemData itemdata;

    [MenuItem("Tools/Item Tool")]
    static void Init()
    {
        itemdata = CreateInstance<ItemData>();
        itemdata.Load();

        ItemTool window = GetWindow<ItemTool>(false, "Item Tool");
        window.Show();
    }

    private void OnGUI()
    {
        if (itemdata == null) return;

        EditorGUILayout.BeginVertical();
        {
            UnityObjcet source = itemSource;
            Sprite Icon = icon;
            ToolHelper.ToolTopLayer(itemdata, ref select, ref source, ref Icon, uiWidthSmall);

            itemSource = (GameObject)source;
            icon = Icon;

            EditorGUILayout.BeginHorizontal();
            {
                ToolHelper.ToolMiddleLayerTypeList(ref SP2, itemdata, ref select, ref source, ref Icon, uiWidthSmall);

                itemSource = (GameObject)source;
                icon = Icon;

                SP2 = EditorGUILayout.BeginScrollView(SP2);
                {
                    if(itemdata.GetDataCount() > 0)
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.Separator();

                            EditorGUILayout.LabelField("ID : ", select.ToString(), GUILayout.Width(uiWidthLarge));
                            ItemClip selectClip = itemdata.clips[select] as ItemClip;
                            selectClip.ClipName = EditorGUILayout.TextField("이름 : ", selectClip.ClipName, GUILayout.Width(uiWidthLarge));
                            selectClip.itemType = (ItemType)EditorGUILayout.EnumPopup("아이템 타입 : ", selectClip.itemType, GUILayout.Width(uiWidthLarge));
                            selectClip.canBuy = EditorGUILayout.Toggle("구입가능 : ",selectClip.canBuy,GUILayout.Width(uiWidthMiddle));
                            if (selectClip.canBuy)
                                selectClip.buy = EditorGUILayout.IntField("구입 가격 : ", selectClip.buy, GUILayout.Width(uiWidthMiddle));
                            selectClip.canSell = EditorGUILayout.Toggle("판매가능 : ", selectClip.canSell, GUILayout.Width(uiWidthMiddle));
                            if(selectClip.canSell)
                                selectClip.sell = EditorGUILayout.IntField("판매 가격 : ", selectClip.sell, GUILayout.Width(uiWidthMiddle));
                            selectClip.canCreateSometing = EditorGUILayout.Toggle("조합가능 : ", selectClip.canCreateSometing, GUILayout.Width(uiWidthMiddle));
                            selectClip.Countable = EditorGUILayout.Toggle("셀수있음 : ", selectClip.Countable, GUILayout.Width(uiWidthMiddle));
                            selectClip.CanEnforce = EditorGUILayout.Toggle("강화가능 : ", selectClip.CanEnforce, GUILayout.Width(uiWidthMiddle));

                            if(selectClip.CanEnforce)
                                selectClip.enforce = EditorGUILayout.IntField("강화 : ", selectClip.enforce, GUILayout.Width(uiWidthLarge));

                            EditorGUILayout.Separator();

                            if(itemSource == null && selectClip.ClipFileName != string.Empty )
                            {
                                selectClip.PreLoad();
                                itemSource = Resources.Load(selectClip.ClipFilePath + selectClip.ClipFileName) as GameObject;
                            }

                            if(icon == null && selectClip.IconFileName != string.Empty)
                            {
                                selectClip.PreLoad();

                                icon = Resources.Load<Sprite>(selectClip.IconFilePath + selectClip.IconFileName);
                            }

                            icon = (Sprite)EditorGUILayout.ObjectField("아이콘 : ", icon, typeof(Sprite), false, GUILayout.Width(uiWidthLarge));

                            itemSource = (GameObject)EditorGUILayout.ObjectField("아이템 : ", itemSource, typeof(GameObject), false, GUILayout.Width(uiWidthLarge));


                            if (itemSource != null)
                            {
                                selectClip.ClipFilePath = ToolHelper.GetPath(itemSource);
                                selectClip.ClipFileName = itemSource.name;
                            }
                            else
                            {
                                selectClip.ClipFileName = string.Empty;
                                selectClip.ClipFilePath = string.Empty;
                                itemSource = null;

                            }
                            if(icon != null)
                            {
                                selectClip.IconFilePath = ToolHelper.GetPath(icon);
                                selectClip.IconFileName = icon.name;
                            }
                            else
                            {
                                selectClip.IconFilePath = string.Empty;
                                selectClip.IconFileName = string.Empty;
                                icon = null;
                            }


                            EditorGUILayout.Separator();

                            for(int i = 0; i < selectClip.CharacterAttribute.Length; i++)
                            {
                                selectClip.CharacterAttribute[i] = (stat)EditorGUILayout.EnumPopup("추가할 스텟 : ", selectClip.CharacterAttribute[i], GUILayout.Width(uiWidthLarge));
                                if(selectClip.CharacterAttribute[i] != stat.None)
                                {
                                    selectClip.value[i] = EditorGUILayout.FloatField("수치 : ", selectClip.value[i], GUILayout.Width(uiWidthMiddle));
                                }
                            }

                            if(GUILayout.Button("스텟추가"))
                            {
                                AddStat(selectClip);
                            }

                            if(GUILayout.Button("제거"))
                            {
                                Remove(selectClip, selectClip.CharacterAttribute.Length - 1);
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

        EditorGUILayout.BeginHorizontal();
        {
            if(GUILayout.Button("ReloadSetting"))
            {
                itemdata = CreateInstance<ItemData>();
                itemdata.Load();
                select = 0;
                itemSource = null;
                icon = null;
            }

            if(GUILayout.Button("Save"))
            {
                itemdata.Save();
                CreateEnumStructure();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddStat(ItemClip clip)
    {
        clip.CharacterAttribute = ArrayHelper.helperAdd(stat.None, clip.CharacterAttribute);
        clip.value = ArrayHelper.helperAdd(0.0f, clip.value);
    }

    void Remove(ItemClip clip,int index)
    {
        clip.CharacterAttribute = ArrayHelper.helperRemove(index,clip.CharacterAttribute);
        clip.value = ArrayHelper.helperRemove(index, clip.value);
    }




    public void CreateEnumStructure()
    {
        string enumName = "ItemList";
        StringBuilder builder = new StringBuilder();
        builder.AppendLine();
        for (int i = 0; i < itemdata.clips.Length; i++)
        {
            if (itemdata.clips[i].ClipName != string.Empty)
            {
                builder.AppendLine($"       { itemdata.clips[i].ClipName} = {i} ,");
            }
        }
        ToolHelper.CreateEnumStructure(enumName, builder);
    }
}
