using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_LookAt : MonoBehaviour
{
    #region Variables

    public bool isTargeting = false;

    #endregion

    public void LookAtTarget(Vector3 targetPos, float delta)
    {
        Vector3 playerTargetPos = Game_Manager.defaultLookAtTarget.position;

        if (!isTargeting) targetPos = playerTargetPos;

        transform.LookAt(targetPos);

    }

}
