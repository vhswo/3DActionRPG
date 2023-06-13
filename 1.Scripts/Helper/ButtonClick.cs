using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour
{
    public Button yesbtn;
    Button nobtn;
    private void Start()
    {
        yesbtn = GetComponent<Button>();
        nobtn = GetComponent<Button>();

        yesbtn.onClick.AddListener(YesBtn);
    }

    public void YesBtn()
    {
    }
}
