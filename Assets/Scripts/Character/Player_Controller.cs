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
    float moveMultiplier = 1, smooth = 0.5f, height = 0.2f, heightRaycastMax = 0.5f, heightRaycastMin = -4, climbSpeed = 0.16f, edgePush = 0.05f, dodgeSpeed = 1.6f;
    float prevSpeed;

    public bool isSprinting, isWalking, isDodging;
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

    public void Move(Vector3 dir, float delta)
    {
        RaycastHit heightRaycast = new RaycastHit();

        float speed = dir.magnitude * moveMultiplier;

        if (isWalking) speed = 0.5f * dir.magnitude;
        else if (isSprinting ) speed *= 1.5f;
        if(speed > 0) speed = Mathf.Clamp(speed, 0.5f, 2);

        isMoving = speed != 0 && height >= pA.minFootHeight;

        Vector3 movePos = transform.position;

        if (isGrounded)
        {
            speed = Mathf.Lerp(prevSpeed, speed, smooth);

            if (isDodging)
            {
                rB.velocity = isMoving ? dir * dodgeSpeed * rB.mass : transform.forward * dodgeSpeed * 0.5f * rB.mass;

            }

            if (isMoving)
            {
                dir = Vector3.Lerp(transform.forward, RelativeDirection(dir, cam.forward), smooth);

                movePos += dir * speed * delta * 3f;
                if (height >= 0.05f) movePos.y = Mathf.Lerp(movePos.y, movePos.y + height, climbSpeed);

                transform.forward = dir;

            }
            else { dir = Vector3.zero; }

        }

        rB.MovePosition(movePos);
        pA.UpdateAnimValue("isMoving", isMoving);
        pA.UpdateAnimValue("_speed", speed, delta);

        heightRaycast = GetHeightRaycast(dir);
        height = heightRaycast.point.y - transform.position.y;

        if(height < pA.minFootHeight) rB.MovePosition(Vector3.Lerp(rB.position, rB.position + (Vector3.down + dir).normalized, edgePush));
        else

        gizmoHeightPos = heightRaycast.point;

        isGrounded = height > heightRaycastMin && height <= heightRaycastMax && heightRaycast.transform.tag == "Ground" ? true : false;

        prevSpeed = speed;

    }

    RaycastHit GetHeightRaycast(Vector3 dir)
    {
        Vector3 castOrigin = rB.position + (dir * pA.colliderRadius) + (Vector3.up * heightRaycastMax);

        RaycastHit hit;
        Ray ray = new Ray(castOrigin, Vector3.down);

        Physics.Raycast(ray, out hit, -heightRaycastMin + heightRaycastMax, layerMask);

        return hit;

    }

    public Vector3 RelativeDirection(Vector3 dir, Vector3 relativeDir)
    {
        Transform dirTransform = transform;
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
        Gizmos.DrawCube(gizmoHeightPos, new Vector3(0.2f, height, 0.2f));

    }

}
