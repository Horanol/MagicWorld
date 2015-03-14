using UnityEngine;
using System.Collections;

public class Effects : MonoBehaviour 
{
	public  GameObject attackEffect1;
	public  GameObject attackEffect2;
	public  GameObject attackEffect3;
	public  GameObject hurtEffect1;
	public  GameObject hurtEffect2;
	public  GameObject upgradeEffect;
	public  GameObject magicEffect1;
	public  GameObject magicEffect2;

	public MeleeWeaponTrail Left_Trail;
	public MeleeWeaponTrail Right_Trail;
	//全局静态方法，必须把上面的Effect设为静态的！！
	void OnEnable()
	{
		//在脚本运行期间，添加方法到事件中心
		Messenger<PlayEffectParameters>.AddListener(names.MethodNames.PlayEffect,PlayEffect);
	}
	void OnDisable()
	{
		//在脚本失活期间，从事件中心中取消方法
		Messenger<PlayEffectParameters>.RemoveListener(names.MethodNames.PlayEffect,PlayEffect);
	}
	public void PlayEffect(PlayEffectParameters p)
	{  
		GameObject newEffect = null;
		switch(p.eName)
		{
		case EffectNames.attackEffect1 :
			 newEffect = GameObject.Instantiate (attackEffect1 ,p.position ,p.rotation ) as GameObject ;
			break;
		case EffectNames.attackEffect2:
			newEffect = GameObject.Instantiate (attackEffect2,p.position ,p.rotation) as GameObject ;
			break;
		case EffectNames.attackEffect3:
			newEffect = GameObject.Instantiate (attackEffect3,p.position ,p.rotation)as GameObject ;
			break;
		case EffectNames.hurtEffect1:
			newEffect = GameObject.Instantiate (hurtEffect1,p.position ,p.rotation) as GameObject ;
			break;
		case EffectNames.hurtEffect2:
			newEffect = GameObject.Instantiate (hurtEffect2,p.position ,p.rotation) as GameObject ;
			break;
		case EffectNames.magicEffect1:
			newEffect = GameObject.Instantiate (magicEffect1,p.position ,p.rotation) as GameObject ;
			break;
		case EffectNames.magicEffect2:
			newEffect = GameObject.Instantiate (magicEffect2,p.position ,p.rotation) as GameObject ;
			break;
		}
		Destroy(newEffect ,p.lastTime);
	}
	public void SetLeftTrailEffect(bool value)
	{
		//注意刀光脚本的Emit属性是控制是否显示刀光的，不能直接SetActive整个脚本，这样会使得脚本后面不能运行
		Left_Trail.Emit = value;
	}
	public void SetRightTrailEffect(bool value)
	{
		Right_Trail.Emit = value;
	}
}
