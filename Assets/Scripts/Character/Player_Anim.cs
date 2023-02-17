using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Anim : MonoBehaviour
{
    #region Variables

    Animator anim;

    float animSmooth = 0.64f;

    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    public void PlayAnim(string stateName, float delta)
    {
        anim.CrossFade(stateName, delta);

    }
    public void PlayAnim(string stateName, string boolState, bool boolValue, float delta)
    {
        anim.SetBool(boolState, false);
        anim.CrossFade(stateName, delta);

    }

    public void UpdateAnimValue(string valueName, float value)
    {
        anim.SetFloat(valueName, value);

    }
    public void UpdateAnimValue(string valueName, float value, float delta)
    {
        float v = anim.GetFloat(valueName);
        value = Mathf.Lerp(v, value, delta * animSmooth * 16);

        anim.SetFloat(valueName, value);

    }

    public void UpdateAnimValue(string valueName, bool value)
    {
        anim.SetBool(valueName, value);

    }

}
