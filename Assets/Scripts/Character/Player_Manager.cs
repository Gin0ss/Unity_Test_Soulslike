using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_Manager : MonoBehaviour
{
    #region Variables

    #region Scripts

    Player_Input pI;
    Player_Controller pC;
    Camera_Control cC;

    GameObject camControl;

    [SerializeField]
    float inputTimer;

    [SerializeField]
    bool isAlive = true, isInteracting;

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

        #region Input Check

        pC.isWalking = pI.inputWalkToggle;
        pC.isSprinting = pI.inputSprint;

        pC.isDodging = InteractionSwitch(pI.inputDodge, 0.5f);

        ResetReleaseInputs();

        #endregion

        if (!isInteracting)
        {
            pC.Move(moveDelta, delta);

        }
        else { inputTimer += delta; }

        cC.FollowTarget(transform.position, delta);
        cC.LookAtTarget(delta);
        cC.OrbitTarget(cameraDelta, delta);

    }

    bool InteractionSwitch(bool actionBool, float interactionTime)
    {
        if (isInteracting)
        {
            actionBool = false;

        }

        if (inputTimer >= interactionTime)
        {
            isInteracting = false;
            inputTimer = 0;

        }
        else if(actionBool) isInteracting = true;


        return actionBool;

    }

    void ResetReleaseInputs()
    {
        pI.inputLockCam = false;
        pI.inputDodge = false;
        pI.inputJump = false;
        pI.inputLightAttackL = false;
        pI.inputLightAttackR = false;
        pI.inputInteract = false;
        pI.inputPause = false;

    }

}
