using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack_Combo : MonoBehaviour
{
    #region Variables

    Player_Controller pC;
    Player_Anim pA;

    [SerializeField]
    Collider weaponCollider;

    [SerializeField]
    int currentAttackIndex;

    [SerializeField]
    float comboTimer, maxInputTime = 0.5f;

    [SerializeField]
    bool inCombo;

    #endregion

    private void Awake()
    {
        pA = GetComponent<Player_Anim>();
        pC = GetComponent<Player_Controller>();

    }

    public void IncrementComboTimer(float delta)
    {
        inCombo = comboTimer <= maxInputTime;
        if (inCombo && currentAttackIndex > 0) comboTimer += delta;
        else
        {
            comboTimer = 0;
            currentAttackIndex = 0;

        }

    }
    public void ResetComboTimer() { comboTimer = 0; }
    public void EnableAttackCollider() { if(weaponCollider != null) weaponCollider.enabled = true; }
    public void DisableAttackCollider() { if (weaponCollider != null) weaponCollider.enabled = false; }


    public void Attack(Vector3 dir, int attackInput, float delta)
    {
        string attackState = "Invalid_Action"; //Might be better as a string array or use the animator state machine
        
        switch (attackInput)
        {
            case 0: //Light Attack L
                switch (currentAttackIndex)
                {
                    case 0:
                        attackState = "Light_AttackL_1";
                        currentAttackIndex = inCombo ? 1 : 0;
                        break;
                    case 1:
                        attackState = "Light_AttackL_2";
                        currentAttackIndex = inCombo ? 2 : 0;
                        break;
                    case 2:
                        attackState = "Light_AttackL_3";
                        currentAttackIndex = 0;
                        break;
                }
                break;

            case 1: //Light Attack R
                switch (currentAttackIndex)
                {
                    case 0:
                        attackState = "Light_AttackR_1";
                        currentAttackIndex = inCombo ? 1 : 0;
                        break;
                    case 1:
                        attackState = "Light_AttackR_2";
                        currentAttackIndex = inCombo ? 2 : 0;
                        break;
                    case 2:
                        attackState = "Light_AttackR_3";
                        currentAttackIndex = 0;
                        break;
                }
                break;
            case 2: //Heavy Attack L (Can adjust speed on charge with curves or create charge anim states acting as combo attack states)
                switch (currentAttackIndex)
                {
                    case 0:
                        attackState = "Heavy_AttackL_1";
                        currentAttackIndex = inCombo ? 1 : 0;
                        break;
                    case 1:
                        attackState = "Heavy_AttackL_2";
                        currentAttackIndex = 0;
                        break;
                }
                break;
            case 3: //Heavy Attack L (Could also modify the combo input wait time for charge attacks as they take longer but may still work since the timer start at the point input becomes available)
                switch (currentAttackIndex)
                {
                    case 0:
                        attackState = "Heavy_AttackR_1";
                        currentAttackIndex = inCombo ? 1 : 0;
                        break;
                    case 1:
                        attackState = "Heavy_AttackR_2";
                        currentAttackIndex = 0;
                        break;
                }
                break;

        }

        pA.PlayAnim(attackState, delta);

    }

}
