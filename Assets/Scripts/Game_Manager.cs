using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    #region Variables

    public static Player_Manager pM;
    public static Camera cam;

    public static Transform player;
    public static Transform defaultLookAtTarget;

    #endregion

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultLookAtTarget = player.Find("_Look_Target");
        pM = player.GetComponent<Player_Manager>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;

        pM.GameLoop(deltaTime);

    }
}
