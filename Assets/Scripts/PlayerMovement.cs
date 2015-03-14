using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public Animator anim;
	public CharacterController charControl;
	public float smoothness = 5.5f;
	public float movingSpeed =4f;
	public float jumpSpeed=13.0f;
	public float gravity = 20.0f;
	
	public bool isWalking = false;
	public bool isAttacking = false;
	public bool TrailIsEnabled = false;

	public Vector3 direction =Vector3 .zero;

	public GameObject AttackingEffects;
	void Start()
	{
		anim = GetComponent<Animator>();
		charControl = GetComponent<CharacterController>();
	}
	void Update()
	{
		SetDirection();
		if(direction == Vector3 .zero )
		{
			isWalking = false;
			anim.SetBool("isWalking",false);
		}
		if(isWalking)
		{
			RotateBody();
			MoveTowards();
			anim.SetBool("isWalking",true);
			isAttacking = false;
		}
		else 
		{
			AnimatorStateInfo info = anim .GetCurrentAnimatorStateInfo(0);
			if(info.nameHash == Animator .StringToHash (names .baseLayer_Idle ))
			{
				//isAttacking = false;
				//判断输入是否为空的代码要在idle状态下判断，当播放其他动画的时候不判断
				if(direction!= Vector3 .zero)
				{
					isWalking = true ;
				}
				if(Input .GetKey (KeyCode.R ))
				{
					anim .SetTrigger ("shout");	
				}
				if(Input .GetKey (KeyCode.Z ))
				{
					anim .SetTrigger ("bigAttack1");
					isAttacking = true;
				}
				if(Input .GetKey (KeyCode.X ))
				{
					anim .SetTrigger ("normalAttack1");
					isAttacking = true;
				}
				if(Input .GetKey (KeyCode.C))
				{
					anim .SetTrigger ("normalAttack2");
					isAttacking = true;
				}
			}
		}
		if(isAttacking)
		{
			if(!TrailIsEnabled)
			{
				AttackingEffects.SendMessage (names.SetLeftTrailEffect,true);
				AttackingEffects.SendMessage (names.SetRightTrailEffect,true);
				TrailIsEnabled = true;
			}
		}
		else
		{
			if(TrailIsEnabled)
			{
				AttackingEffects.SendMessage (names.SetLeftTrailEffect,false);
				AttackingEffects.SendMessage (names.SetRightTrailEffect,false);
				TrailIsEnabled = false;
			}
		}
	}
	void MoveTowards()
	{
			Vector3 moveDirection = direction * movingSpeed;
			if(charControl.isGrounded)
			{
				if (Input.GetButton("Jump"))
				{
					moveDirection.y = jumpSpeed;//设置跳起距离	
				}
			}
			moveDirection.y -= gravity * Time.deltaTime;//模拟重力
			charControl.Move(moveDirection * Time.deltaTime);//计算一帧的移动距离并移动
	}
	void RotateBody()
	{
		if(direction != Vector3.zero)//当方向不为空时，旋转
		{
			Quaternion targetRotation = Quaternion.LookRotation (direction);
			transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime*smoothness);
		}
	}
	void SetDirection()
	{
			float h=0.0f,v=0.0f;
		  	if(Input.GetKey(KeyCode.W))
			{
				v=1.0f;
			}
			if(Input.GetKey(KeyCode.S))
			{
				v=-1.0f;
			}
			if(Input.GetKey(KeyCode.A))
			{
				h=-1.0f;
			}
			if(Input.GetKey(KeyCode.D))
			{
				h=1.0f;
			}
			direction = new Vector3(h,0.0f,v);
			direction.Normalize();
	}

}
