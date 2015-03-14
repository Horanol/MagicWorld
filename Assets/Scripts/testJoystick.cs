using UnityEngine;
using System.Collections;

public class testJoystick : MonoBehaviour
{

    public void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    public void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }
	  
    void OnJoystickMoveEnd(MovingJoystick move)
    {
        //停止时，角色恢复idle  
        if (move.joystickName == "MoveJoystick")
        {
            Messenger<float, float>.Broadcast("SetDirection", 0.0f, 0.0f);
        }
    }
    //移动摇杆中  
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "MoveJoystick")
        {
            return;
        }

        //获取摇杆中心偏移的坐标  
        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;

         //设置角色的朝向（摇杆偏移量）  
         Messenger<float, float>.Broadcast("SetDirection", joyPositionX, joyPositionY);
    }  
}
