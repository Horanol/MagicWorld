using UnityEngine;
using System.Collections;

public class testButton : MonoBehaviour
{
    public void OnEnable()
    {
        EasyButton.On_ButtonDown += OnBtnDown;
    }

    public void OnDisable()
    {
        EasyButton.On_ButtonDown -= OnBtnDown;
    }
    void OnBtnDown(string btnName)
    {
        if (btnName == "AttackBtn1")
        {
            //点击工具图标时候切换到normalAttack1状态
            Messenger<PlayerStates>.Broadcast(names.MethodNames.CheckAttackState, PlayerStates.normalAttackingState1);
        }

    }

}
