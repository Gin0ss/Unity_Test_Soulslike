using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    #region Variables

    #region Scripts

    Player_Input pI;
    Player_Controller pC;
    Camera_Control cC;

    GameObject camControl;

    bool isAlive = true;

    #endregion

    #endregion

    void Start()
    {
        camControl = GameObject.Find("_Camera_Control");

        cC = camControl.GetComponent<Camera_Control>();
        pI = this.GetComponent<Player_Input>();
        pC = this.GetComponent<Player_Controller>();

    }

    public void GameLoop(float delta)
    {
        Vector3 moveDelta = new (pI.inputMove.x, 0, pI.inputMove.y);
        Vector3 cameraDelta = new (pI.inputCamera.x, pI.inputCamera.y, 0);

        pC.Move(moveDelta, delta);

        cC.FollowTarget(transform.position, delta);
        cC.LookAtTarget(delta);
        cC.OrbitTarget(cameraDelta, delta);

    }

}
