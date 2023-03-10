using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Player_Controller : MonoBehaviour
{
    #region Variables

    Rigidbody rB;
    Player_Anim pA;

    public TextMeshProUGUI moveDirectionText;
    public TextMeshProUGUI camDirectionText;

    public LayerMask layerMask;

    public Transform cam;

    Vector3 gizmoHeightPos;

    [SerializeField]
    float moveMultiplier = 1, smooth = 0.5f, height = 0.2f, heightRaycastMax = 0.5f, heightRaycastMin = -4, climbSpeed = 0.16f, edgePush = 0.1f, dodgeSpeed = 1.6f;
    float prevSpeed;

    public bool isSprinting, isWalking;
    [SerializeField]
    bool isGrounded = false, isMoving = false;

    #endregion

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
        pA = this.GetComponent<Player_Anim>();

        cam = Game_Manager.cam.transform;

        isGrounded = true;

    }

    public void Dodge(Vector3 dir, float delta)
    {
        if (isMoving)
        {
            pA.PlayAnim("Dodge_Roll", delta);

        }
        else pA.PlayAnim("Dodge_Backstep", delta);

    }

    public void Move(Vector3 dir, float delta)
    {
        Vector3 movePos = transform.position;

        float speed = GetSpeed(dir.magnitude);

        isMoving = speed != 0 && height >= pA.minFootHeight;

        if (isMoving) dir = Vector3.Lerp(transform.forward, RelativeDirection(dir, cam.forward * dir.magnitude), smooth);
        else dir = transform.forward;

        RaycastHit heightRaycast = new RaycastHit();
        heightRaycast = GetHeightRaycast(dir);
        height = heightRaycast.point.y - transform.position.y;

        isGrounded = height > heightRaycastMin && height <= heightRaycastMax && heightRaycast.transform.tag == "Ground" ? true : false;

        if (isGrounded)
        {
            speed = Mathf.Lerp(prevSpeed, speed, smooth);

            if (isMoving)
            {
                movePos += dir * speed * delta * 3f;
                if (height >= 0.05f) movePos.y = Mathf.Lerp(movePos.y, movePos.y + height, climbSpeed);

                transform.forward = dir;

            }

        }

        rB.MovePosition(movePos);
        pA.UpdateAnimValue("isMoving", isMoving);
        pA.UpdateAnimValue("_speed", speed, delta);

        Vector3 fallDirection = rB.position + ((Vector3.down + dir).normalized * edgePush);
        if (height < pA.minFootHeight) rB.MovePosition(Vector3.Lerp(rB.position, fallDirection, smooth));

        gizmoHeightPos = heightRaycast.point;

        prevSpeed = speed;

    }

    float GetSpeed(float directionMagnitude)
    {
        float speed = directionMagnitude * moveMultiplier;

        if (isWalking) return 0.5f * directionMagnitude;
        else if (isSprinting) return speed * 1.5f;
        if (speed > 0) return Mathf.Clamp(speed, 0.5f, 2);

        return speed;

    }

    RaycastHit GetHeightRaycast(Vector3 dir)
    {
        Vector3 castOrigin = rB.position + (dir * (pA.colliderRadius - 0.05f)) + (Vector3.up * heightRaycastMax); //Still goes inside collision and detect floor under object

        RaycastHit hit;
        Ray ray = new Ray(castOrigin, Vector3.down);

        Physics.Raycast(ray, out hit, -heightRaycastMin + heightRaycastMax, layerMask);

        return hit;

    }

    public Vector3 RelativeDirection(Vector3 dir, Vector3 relativeDir)
    {
        Transform dirTransform = transform;

        dir.y = 0;
        relativeDir.y = 0;

        Vector3 dirRotation = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
        Vector3 relativeRotation = Quaternion.LookRotation(relativeDir, Vector3.up).eulerAngles;

        dirTransform.eulerAngles = relativeRotation;
        dirTransform.eulerAngles += dirRotation;

        dir = dirTransform.forward;
        dir.y = 0;

        return dir.normalized;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(gizmoHeightPos, new Vector3(0.05f, height, 0.05f));

    }

}
