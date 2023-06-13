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
    Pauldrons, //�߰�, �����
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
    ShortSword, //�ܵ�
    Sword,      //�⺻��
    LongSword, //���
    TwoHandSword, //ū��
    Spear,      //â
    Bow,        //Ȱ
    Sheild,
    Quiver, //ȭ����
}

public enum stat
{
    None,
    Strength, //��
    Agility, //��ø
    Intellect,  //����
    Stamina,    //���¹̳�
    Damage,
    Amor,
    Healing,    //����  ü�� ������ ä���
    Critical,
    Evasion,    //ȸ��
    AttackSpeed,  //���ݼӵ�
    HP,
    MP,
    MoveSpeed, //�̵��ӵ�
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
    public bool Countable = false; //������ �ִ� ����������
    public bool canSell = false, canBuy = false;
    public bool unique = false; //����Ʈ ��������
    public bool canCreateSometing = false; //�������
    public bool CanEnforce = true; //��ȭ�� ��������

    public int enforce = 0; //��ȭ

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
    /// ���� ������ -1 ��ȯ
    /// ���ϴ� ���¸� (int)stat �� ������ �׿� �´� ���� �������ش�
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
