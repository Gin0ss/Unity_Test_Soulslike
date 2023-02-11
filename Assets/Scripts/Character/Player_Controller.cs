using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Player_Controller : MonoBehaviour
{
    #region Variables

    public Rigidbody rB;
    public TextMeshProUGUI moveDirectionText;
    public TextMeshProUGUI camDirectionText;

    public Transform cam;

    [SerializeField]
    float speed = 1, camSmooth = 0.5f;

    #endregion

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
        cam = Game_Manager.cam.transform;

    }

    public void Move(Vector3 dir, float delta)
    {
        Vector3 movePos = rB.position;

        if (transform.position - ) speed = dir.magnitude;
        dir = delta * speed * PlayerDirection(dir, delta);

        if (speed != 0)
        {
            movePos += dir;

            transform.forward = dir;
            rB.MovePosition(movePos);

        }

    }

    public Vector3 PlayerDirection(Vector3 dir, float delta)
    {
        float smooth = delta * camSmooth * 16;

        dir = Vector3.Lerp(transform.forward, dir, 0.5f * delta * 16);
        dir = Vector3.Slerp(RelativeCamDirection(dir, delta), dir, smooth);
        dir.y = 0;

        moveDirectionText.text = string.Format("{0} : {1}", dir.x, dir.z);

        return dir.normalized;

    }

    public Vector3 RelativeCamDirection(Vector3 dir, float delta)
    {
        float smooth = delta * camSmooth * 10;

        dir = Vector3.Slerp(cam.forward, dir, 0.5f * delta * 16);
        dir.y = 0;

        camDirectionText.text = string.Format("{0} : {1}", cam.forward.x, cam.forward.z);

        return dir.normalized;

    }

}
