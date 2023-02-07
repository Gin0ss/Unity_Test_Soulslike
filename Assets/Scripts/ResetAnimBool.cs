using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimBool : StateMachineBehaviour
{
    public string[] targetBool;
    public bool[] boolValue;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < targetBool.Length; i++)
        {
            animator.SetBool(targetBool[i], boolValue[i]);
        }
    }
}
