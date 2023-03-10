using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_Anim : MonoBehaviour
{
    #region Variables

    public Animator anim;

    public AnimatorOverrideController animOverride;
    public LayerMask layerMaskIK;

    public float footHeightOffset = 0.075f, maxFootHeight = 0.42f, minFootHeight = -2, footForwardOffset = 0.05f, footHeightL, footHeightR, feetLevelThreshold = 0.1f, colliderRadius;
    [SerializeField]
    float animSmooth = 0.64f;

    public bool isFeetLevel = true;

    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();

        colliderRadius = GetComponent<CapsuleCollider>().radius + 0.05f;

    }

    public void PlayAnim(string stateName, float delta)
    {
        anim.CrossFade(stateName, 0.16f, 0, delta);

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

    #region IK

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));

            IKRaycast(AvatarIKGoal.LeftFoot);
            IKRaycast(AvatarIKGoal.RightFoot);

            footHeightL = anim.GetIKPosition(AvatarIKGoal.LeftFoot).y - transform.position.y;
            footHeightR = anim.GetIKPosition(AvatarIKGoal.RightFoot).y - transform.position.y;

            isFeetLevel = footHeightL - footHeightR < feetLevelThreshold && footHeightL - footHeightR > -feetLevelThreshold ? true : false;

        }

    }

    public void IKRaycast(AvatarIKGoal IKGoal)
    {
        Vector3 dir = transform.forward * anim.GetFloat("_speed") * colliderRadius;
        Vector3 rayOrigin = anim.GetIKPosition(IKGoal) + (Vector3.up * maxFootHeight) + (transform.forward * footForwardOffset) + dir;

        RaycastHit hit;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out hit, footHeightOffset + (maxFootHeight - minFootHeight), layerMaskIK))
        {
            if (hit.transform.tag == "Ground")
            {
                Vector3 IKPosition = Vector3.Lerp(anim.GetIKPosition(IKGoal), hit.point, 0.5f);
                IKPosition -= dir + (dir * 0.05f);
                IKPosition.y = hit.point.y + footHeightOffset;

                if (IKPosition.y - transform.position.y > feetLevelThreshold && !isFeetLevel && !anim.GetBool("isMoving"))
                {
                    IKPosition += transform.forward * footForwardOffset;

                }

                anim.SetIKPosition(IKGoal, IKPosition);
                anim.SetIKRotation(IKGoal, Quaternion.LookRotation(transform.forward, hit.normal));

            }
        }
    }

    #endregion

}
