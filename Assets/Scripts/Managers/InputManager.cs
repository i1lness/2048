using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputManager
{
    public static Action<Define.MoveInputType> moveInputAction = null;

    public static Action mouseInputAction = null;

    public static Action escClickAction = null;

    /* Input 확인하는 함수 */
    public void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (moveInputAction != null)
                    moveInputAction.Invoke(Define.MoveInputType.Up);
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                if (moveInputAction != null)
                    moveInputAction.Invoke(Define.MoveInputType.Down);
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                if (moveInputAction != null)
                    moveInputAction.Invoke(Define.MoveInputType.Left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (moveInputAction != null)
                    moveInputAction.Invoke(Define.MoveInputType.Right);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (escClickAction != null)
                    escClickAction.Invoke();
            }
        }
    }
}
