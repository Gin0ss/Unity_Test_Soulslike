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
    Attack_Combo aC;
    Player_Anim pA;

    #endregion

    GameObject camControl;

    [SerializeField]
    float inputTimer;

    int attackInput;

    [SerializeField]
    bool isAlive = true, isInteracting, isDodging, isAttacking, canAttack, performingLightAttackL, performingLightAttackR, performingHeavyAttackL, performingHeavyAttackR;

    #endregion

    void Start()
    {
        camControl = GameObject.Find("_Camera_Control");

        cC = camControl.GetComponent<Camera_Control>();
        pI = GetComponent<Player_Input>();
        pA = GetComponent<Player_Anim>();
        pC = GetComponent<Player_Controller>();
        aC = GetComponent<Attack_Combo>();

    }

    public void AllowCombo() { canAttack = true; isAttacking = false; }

    public void DisableCombo() { canAttack = false; isAttacking = true; }

    public void GameLoop(float delta)
    {
        Vector3 moveDelta = new (pI.inputMove.x, 0, pI.inputMove.y);
        Vector3 cameraDelta = new (pI.inputCamera.x, pI.inputCamera.y, 0);

        #region Input Check

        pC.isWalking = pI.inputWalkToggle;
        pC.isSprinting = pI.inputSprint;

        if (pI.inputLightAttackL) { attackInput = 0; }
        else if (pI.inputLightAttackR) { attackInput = 1; }
        else if (pI.inputHeavyAttackL) { attackInput = 2; }
        else if (pI.inputHeavyAttackR) { attackInput = 3; }
        isAttacking = pI.inputLightAttackL || pI.inputLightAttackR || pI.inputHeavyAttackL || pI.inputHeavyAttackR;

        isDodging = pI.inputDodge;

        ResetReleaseInputs();

        #endregion 

        if (!isInteracting)
        {
            inputTimer = 0;
            isInteracting = isAttacking || isDodging;

            if (isAttacking) 
            {
                Debug.Log(attackInput);
                aC.Attack(moveDelta, attackInput, delta);
                DisableCombo();

            }
            if (isDodging) { pC.Dodge(moveDelta, delta); }

            pC.Move(moveDelta, delta);

        }
        else 
        { 
            inputTimer += delta;
            aC.ResetComboTimer();
            isInteracting = pA.anim.GetBool("isInteracting"); //When true sync when isInteracting is set to false in anim state

        }

        if (canAttack) aC.IncrementComboTimer(delta);

        cC.FollowTarget(transform.position, delta);
        cC.LookAtTarget(delta);
        cC.OrbitTarget(cameraDelta, delta);

        pA.UpdateAnimValue("isInteracting", isInteracting);

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
