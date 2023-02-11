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
    Player_Anim pA;

    GameObject camControl;

    #endregion

    #endregion

    void Start()
    {
        camControl = GameObject.Find("_Camera_Control");

        cC = camControl.GetComponent<Camera_Control>();
        pI = this.GetComponent<Player_Input>();
        pC = this.GetComponent<Player_Controller>();
        pA = this.GetComponent<Player_Anim>();

    }

    public void GameLoop(float delta)
    {
        Vector3 moveDelta = new (pI.inputMove.x, 0, pI.inputMove.y);
        Vector3 cameraDelta = new (pI.inputCamera.x, 0, pI.inputCamera.y);

        pC.Move(moveDelta, delta);
        //pC.Rotate(moveDelta, delta);

        cC.FollowTarget(transform.position, delta);
        cC.OrbitTarget(cameraDelta, delta);
        cC.LookAtTarget(delta);

    }

}
