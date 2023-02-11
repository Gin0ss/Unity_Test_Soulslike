using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[System.Serializable]
public class Player_Input : MonoBehaviour
{
    #region Variables

    PlayerInput playerInput;

    #region Input Values

    public Vector2 inputMove;
    public Vector2 inputCamera;

    public bool inputLockCam;
    public bool inputDodge;
    public bool inputSprint;
    public bool inputJump;
    public bool inputLightAttackL;
    public bool inputLightAttackR;
    public bool inputHeavyAttackL;
    public bool inputHeavyAttackR;
    public bool inputInteract;
    public bool inputPause;
    public bool inputWalkToggle;

#endregion

    #region Input Actions

    InputAction actionMove;
    InputAction actionCamera;
    InputAction actionLockCam;
    InputAction actionDodge;
    InputAction actionSprint;
    InputAction actionJump;
    InputAction actionLightAttackL;
    InputAction actionLightAttackR;
    InputAction actionHeavyAttackL;
    InputAction actionHeavyAttackR;
    InputAction actionInteract;
    InputAction actionPause;
    InputAction actionWalkToggle;

    #endregion

    #endregion

    #region Initialisation

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        actionMove = playerInput.actions["Move"];
        actionCamera = playerInput.actions["Camera"];

        actionLockCam = playerInput.actions["Lock_Cam"];
        actionDodge = playerInput.actions["Dodge"];
        actionSprint = playerInput.actions["Sprint"];
        actionJump = playerInput.actions["Jump"];
        actionLightAttackL = playerInput.actions["Light_Attack.L"];
        actionLightAttackR = playerInput.actions["Light_Attack.R"];
        actionHeavyAttackL = playerInput.actions["Heavy_Attack.L"];
        actionHeavyAttackR = playerInput.actions["Heavy_Attack.R"];
        actionInteract = playerInput.actions["Interact"];
        actionPause = playerInput.actions["Pause"];
        actionWalkToggle = playerInput.actions["Walk_Toggle"];
        
    }

    private void OnEnable()
    {
        actionLockCam.performed += context => inputLockCam = true;
        actionDodge.performed += context => inputDodge = true;
        actionSprint.performed += context => inputSprint = true;
        actionJump.performed += context => inputJump = true;
        actionLightAttackL.performed += context => inputLightAttackL = true;
        actionLightAttackR.performed += context => inputLightAttackR = true;
        actionHeavyAttackL.performed += context => inputHeavyAttackL = true;
        actionHeavyAttackR.performed += context => inputHeavyAttackR = true;
        actionInteract.performed += context => inputInteract = true;
        actionPause.performed += context => inputPause = true;
        actionWalkToggle.performed += context => inputWalkToggle = true;

    }

    private void OnDisable()
    {
        actionLockCam.performed -= context => inputLockCam = false;
        actionDodge.performed -= context => inputDodge = false;
        actionSprint.performed -= context => inputSprint = false;
        actionJump.performed -= context => inputJump = false;
        actionLightAttackL.performed -= context => inputLightAttackL = false;
        actionLightAttackR.performed -= context => inputLightAttackR = false;
        actionHeavyAttackL.performed -= context => inputHeavyAttackL = false;
        actionHeavyAttackR.performed -= context => inputHeavyAttackR = false;
        actionInteract.performed -= context => inputInteract = false;
        actionPause.performed -= context => inputPause = false;
        actionWalkToggle.performed -= context => inputWalkToggle = false;

    }

    #endregion

    private void FixedUpdate()
    {
        inputMove = actionMove.ReadValue<Vector2>();
        inputCamera = actionCamera.ReadValue<Vector2>();

    }


}
