using System.Collections;
using UnityEngine;
public static class names
{
	public static readonly string baseLayer_Walk = "Base Layer.walk";
	public static readonly string baseLayer_Idle = "Base Layer.idle";
	public static readonly string baseLayer_BigAttack1 = "Base Layer.bigAttack_revolveSwords";
	public static readonly string baseLayer_NormalAttack1 = "Base Layer.normal_doubleSwordsCut";
	public static readonly string baseLayer_NormalAttack2 = "Base Layer.normal_rightCut";
	public static readonly string baseLayer_Shout = "Base Layer.shout";

	public static readonly string SetLeftTrailEffect = "SetLeftTrailEffect";
	public static readonly string SetRightTrailEffect = "SetRightTrailEffect";
	public static class MethodNames
	{
		public static readonly string PlayEffect = "PlayEffect";
	}
}

public enum Armor
{
    Helmet,
    Shirt,
    Pants,
    Boots,
    Shield,
    Ring,
    Gloves,
}
public enum Quality
{
    low,
    medium,
    high,
}
public enum EffectNames
{
	attackEffect1,
	attackEffect2,
	attackEffect3,
	hurtEffect1,
	hurtEffect2,
	upgradeEffect,
	magicEffect1,
	magicEffect2,
}
public enum PlayerStates
{
	walkingState,
	idleState,
	shoutingState,
	bigAttackingState1,
	normalAttackingState1,
	normalAttackingState2,
	normalAttackingState3,
}

public struct PlayEffectParameters
{
	public Vector3 position;
	public Quaternion rotation;
	public float lastTime;
	public EffectNames eName;
}
