using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
	void Start()
	{
		RunTest();
	}
	public void RunTest()
	{
		RunAddTests();
		RunBroadcastTests();
		RunRemoveTests();
		print("All tests have been done!\n");
		Messenger.Broadcast("testVoid");
	}
	public void TestVoid()
	{
		print(" void Test() is printed\n");
	}
	public void TestFloat(float a)
	{
		print("void TestFloat is printed,the parameter is "+a.ToString()+"\n");
	}
	public float TestFloatReturn(float a)
	{
		float returnValue = 1.0f;
		print("float TestFloat is printed,the parameter is "+a.ToString()+" the return value is "+returnValue.ToString()+"\n");
		return returnValue;
	}
	public void RunAddTests()
	{
		Messenger.AddListener("testVoid",TestVoid);
		Messenger<float>.AddListener("testFloat",TestFloat);
		Messenger<float>.AddListener<float>("testFloatReturn",TestFloatReturn);
		print("three kinds of Listener have added\n");
	}
	public void RunBroadcastTests()
	{
		Messenger.Broadcast("testVoid");
		Messenger<float>.Broadcast("testFloat",10.0f);
		Messenger<float>.Broadcast<float>("testFloatReturn",20.0f,HandlerRuturnValue);
	}
	public void HandlerRuturnValue(float a)
	{
		print("the return value is "+a.ToString()+"\n");
	}
	public void RunRemoveTests()
	{
		Messenger.RemoveListener("testVoid",TestVoid);
		Messenger<float>.RemoveListener("testFloat",TestFloat);
		Messenger<float>.RemoveListener<float>("testFloatReturn",TestFloatReturn);
		print("three kinds of Listener have removed\n");
	}


}
