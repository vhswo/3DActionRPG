using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseAnimation : MonoBehaviour
{
    public Animator myAnimator { get; private set; }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

}

public class PlayerAnimation : BaseAnimation
{
    private void Start()
    {
        myAnimator.SetBool("IsGround", true);
    }
}
