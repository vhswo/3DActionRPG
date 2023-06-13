using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region EnumType

public enum ItemType
{
    Helmet,
    Chest,
    Pants,
    Boots,
    Pauldrons, //견갑, 망토등
    Gloves,
    LeftWeapon,
    RightWeapon,
    ring,
    necklace,
    Food,
    None,
}

public enum WeaponType
{
    None,
    ShortSword, //단도
    Sword,      //기본검
    LongSword, //장검
    TwoHandSword, //큰검
    Spear,      //창
    Bow,        //활
    Sheild,
    Quiver, //화살통
}

public enum stat
{
    None,
    Strength, //힘
    Agility, //민첩
    Intellect,  //지능
    Stamina,    //스태미나
    Damage,
    Amor,
    Healing,    //마나  체력 따위를 채우다
    Critical,
    Evasion,    //회피
    AttackSpeed,  //공격속도
    HP,
    MP,
    MoveSpeed, //이동속도
}

#endregion

public class ItemClip : BaseClip
{
    public ItemType itemType = ItemType.None;
    public WeaponType weaponType = WeaponType.None;
    public stat[] CharacterAttribute = new stat[0];
    public float[] value = new float[0];

    public Sprite icon = null;
    public GameObject itemPrefab = null;
    public bool Countable = false; //개수가 있는 아이템인지
    public bool canSell = false, canBuy = false;
    public bool unique = false; //퀘스트 전용인지
    public bool canCreateSometing = false; //조합재료
    public bool CanEnforce = true; //강화가 가능한지

    public int enforce = 0; //강화

    public string IconFileName = string.Empty;
    public string IconFilePath = string.Empty;

    public int sell,buy;

    public override void PreLoad()
    {
        ClipFileFullPath = ClipFilePath + ClipFileName;
        if (ClipFileFullPath != string.Empty && (itemPrefab == null || (itemPrefab != null && itemPrefab.name != ClipName)))
        {
            itemPrefab = Resources.Load(ClipFileFullPath) as GameObject;
        }

        ClipFileFullPath = IconFilePath + IconFileName;
        if (ClipFileFullPath != string.Empty && (icon == null || (icon != null && icon.name != IconFileName)))
        {
            icon = Resources.Load<Sprite>(IconFilePath + IconFileName);
        }
        
    }

    /// <summary>
    /// 값이 없으면 -1 반환
    /// 원하는 상태를 (int)stat 로 보내면 그에 맞는 것을 내보내준다
    /// </summary>
    public float getData(int stat)
    {
        for(int i = 0; i < CharacterAttribute.Length; i++)
        {
            if (CharacterAttribute[i] == CharacterAttribute[stat]) return value[i];
        }

        return -1;
    }
}
