using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Camera_Control : MonoBehaviour
{
    #region Variables

    public Camera_LookAt camLookAt;

    public Transform lookTarget;
    Transform cam, camPivot, camOffset;

    [SerializeField]
    float followSmooth = 0.5f, targetSmooth = 0.5f;
    [SerializeField]
    float mouseSensitivityX = 1, mouseSensitivityY = 0.64f;
    [SerializeField]
    float minVertLook = -50, maxVertLook = 50;
    float x, y, z;

    #endregion

    private void Start()
    {
        cam = Camera.main.transform;
        camLookAt = cam.GetComponent<Camera_LookAt>();
        lookTarget = Game_Manager.defaultLookAtTarget;

        camPivot = transform.Find("_camPivot");
        camOffset = camPivot.Find("_camOffset");

    }
    
    public void UpdateOffset(Vector3 offsetDelta)
    {
        camOffset.position += offsetDelta;

    }

    public void FollowTarget(Vector3 targetPos, float delta)
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, followSmooth * delta * 16);

    }

    public void OrbitTarget(Vector2 rotDir, float delta)
    {
        x += -rotDir.y * delta * mouseSensitivityY;
        x = Mathf.Clamp(x, minVertLook, maxVertLook);
        y += rotDir.x * delta * mouseSensitivityX;
        z = camPivot.localRotation.eulerAngles.z;

        Vector3 rot = new (x, y, z);
        Quaternion camRot = Quaternion.Euler(rot);

        camPivot.localRotation = camRot;

    }

    public void LookAtTarget(float delta)
    {
        Vector3 playerTargetPos = Game_Manager.defaultLookAtTarget.position;
        Vector3 targetPos = lookTarget.position;
        targetPos = Vector3.Lerp(playerTargetPos, targetPos, targetSmooth * delta * 8);

        camLookAt.LookAtTarget(targetPos, delta);

    }

}
