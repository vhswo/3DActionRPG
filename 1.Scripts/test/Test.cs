using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Inventory inven;
    int i = 0;
    void Update()
    {
        if (Input.GetKeyDown("n"))
        {
            ItemClip tmp = ItemManager.instance.GetItemClip(ItemList.Sword);
            inven.AddItem(tmp, 1);
        }

        if (Input.GetKeyDown("m"))
        {
            SoundManager.instance.SetBGMVolume(i == 0 ? 1 : 0);
        }
        if (Input.GetKeyDown("b"))
        {
            ItemClip tmp = ItemManager.instance.GetItemClip(ItemList.HPPotion);
            inven.AddItem(tmp, 1);
        }
    }
}
