using UnityEngine;
using System.Collections;

public class CamaraMovement : MonoBehaviour 
{
	public float smooth = 0.5f;
	
	public Transform player;
	private Vector3 relCamerePos;
	private float relCameraPosMag;
	private Vector3 newPos;

	// Use this for initialization
	void Start () 
	{
		relCamerePos  = transform.position - player.position  ;
		relCameraPosMag = relCamerePos .magnitude - 0.5f;

	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 standardPos = player .position + relCamerePos ;//标准位置，即摄像机当前位置
		Vector3 abovePos = player .position  + Vector3.up * relCameraPosMag;//头顶位置
		Vector3 [] checkPoints = new Vector3[6];//设置六个监测点
		
		checkPoints[0]  = standardPos ;//初始位置
		checkPoints [1] = Vector3.Lerp(standardPos , abovePos , 0.10f);//初始位置到头顶位置的20%处
		checkPoints [2] = Vector3.Lerp(standardPos , abovePos , 0.20f);
		checkPoints [3] = Vector3.Lerp(standardPos , abovePos , 0.30f);
		checkPoints[4] = Vector3.Lerp(standardPos , abovePos , 0.40f); 
		checkPoints[5] = Vector3.Lerp(standardPos , abovePos , 0.50f); 
		
		for(int i=0;i<6;i++)
		{
			if(ViewingPosCheck(checkPoints[i]))//循环检测各个监测点是否合适
			{
				break ;
			}
		}
		transform .position  = Vector3.Lerp(transform .position , newPos ,smooth * Time .deltaTime );
		SmoothLookAt();//另写函数解决镜头面向问题
	
	}
	bool ViewingPosCheck(Vector3 checkPos)
	{
		RaycastHit hit;
		Vector3 checkDirection = player.position+new Vector3 (0.0f,0.1f,0.0f)-checkPos;
		if(Physics .Raycast (checkPos ,checkDirection , out hit , relCameraPosMag))//在监测点向玩家发射射线
		{
			if(hit.transform  != player )//若先射中的不是玩家，即有物体挡在镜头前
			{
				return false ;//返回false
			}
		}
		newPos = checkPos;//若先射中玩家或者没有射中任何东西，则设置该位置为摄像机新的位置
		return true ;
	}
	
	void SmoothLookAt()
	{
		Vector3 relPlayerPos = player .position  -  transform .position ;//获取镜头面向方向
		Quaternion  lookRotation = Quaternion .LookRotation (relPlayerPos , Vector3.up );//用lookRotation函数得到要旋转的角度
		transform .rotation = Quaternion .Lerp (transform .rotation ,lookRotation , smooth * Time .deltaTime );//插值函数实现旋转
		
	}
}
