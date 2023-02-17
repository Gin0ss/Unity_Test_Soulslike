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

    public Transform cam;

    Vector3 gizmoMoveDir, gizmoCamDir;

    [SerializeField]
    float moveSpeed = 1, smooth = 0.5f;
    float prevSpeed;

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
        Vector3 movePos = rB.position;

        float speed = dir.magnitude * moveSpeed;
        speed = Mathf.Lerp(prevSpeed, speed, smooth);
        isMoving = speed != 0;

        pA.UpdateAnimValue("_speed", speed, delta);
        if (isGrounded) pA.UpdateAnimValue("isMoving", isMoving);

        if (isMoving)
        {
            prevSpeed = speed;
            isMoving = true;
            dir = Vector3.Lerp(transform.forward, RelativeDirection(dir, cam.forward), smooth);
            dir *= speed * delta * 2;
            movePos += dir;

            transform.forward = dir;
            rB.MovePosition(movePos);

        }
        else
        {
            isMoving = false;

        }

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

}
