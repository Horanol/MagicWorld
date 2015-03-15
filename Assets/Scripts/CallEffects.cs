using UnityEngine;
using System.Collections;

public class CallEffects : MonoBehaviour
{
    public void CallHitGroundEffect()
    {
        PlayEffectParameters p;
        p.eName = EffectNames.attackEffect1;
        p.position = transform.FindChild("hitGroundPosition").transform.position;
        p.rotation = Quaternion.identity;
        p.lastTime = 1.5f; 
        //调用特效方法 
        Messenger<PlayEffectParameters>.Broadcast(names.MethodNames.PlayEffect, p);      
    }



}
