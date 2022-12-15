using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InputManager
{
    public Action<Define.MoveInputType> moveInputAction = null;

    public Action mouseInputAction = null;

    public Action escClickAction = null;

    /* Input Ȯ���ϴ� �Լ� */
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
