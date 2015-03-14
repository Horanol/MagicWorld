using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MessengerMode {
	DONT_REQUIRE_LISTENER,
	REQUIRE_LISTENER,
}

static internal class MessengerInternal {
	readonly public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
	static public readonly MessengerMode DEFAULT_MODE = MessengerMode.REQUIRE_LISTENER;

	#region Internal variables
	
	//定义一些CleanUp方法也不会取消监听的事件
	static public List< string > permanentMessages = new List< string > ();
	#endregion
	#region Helper methods
	//Marks a certain message as permanent.
	static public void MarkAsPermanent(string eventType) {
		#if LOG_ALL_MESSAGES
		Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
		#endif	
		permanentMessages.Add( eventType );
	}
	
	
	static public void Cleanup()
	{
		#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
		#endif
		
		List< string > messagesToRemove = new List<string>();
		
		foreach (KeyValuePair<string, Delegate> pair in eventTable) {
			bool wasFound = false;
			
			foreach (string message in permanentMessages) {
				if (pair.Key == message) {
					wasFound = true;
					break;
				}
			}
			
			if (!wasFound)
				messagesToRemove.Add( pair.Key );
		}
		
		foreach (string message in messagesToRemove) {
			eventTable.Remove( message );
		}
	}
	
	static public void PrintEventTable()
	{
		Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");
		
		foreach (KeyValuePair<string, Delegate> pair in eventTable) {
			Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
		}
		
		Debug.Log("\n");
	}
	#endregion

	static public void AddListener(string eventType, Delegate callback) {
		MessengerInternal.OnListenerAdding(eventType, callback);//插入委托之前的准备工作，检测能否插入等
		eventTable[eventType] = Delegate.Combine(eventTable[eventType], callback);
	}
	
	static public void RemoveListener(string eventType, Delegate handler) {
		MessengerInternal.OnListenerRemoving(eventType, handler);	//移除委托之前的准备工作
		eventTable[eventType] = Delegate.Remove(eventTable[eventType], handler);
		MessengerInternal.OnListenerRemoved(eventType);//移除后的工作
	}

	//以数组形式返回委托链表中同一键名下所有方法
	static public T[] GetInvocationList<T>(string eventType) {
		Delegate d;
		if(eventTable.TryGetValue(eventType, out d)) {
			if(d != null) {//该代码把委托链表中eventType键名下的所有方法转换成泛型的数组并返回
				return d.GetInvocationList().Cast<T>().ToArray();
			} else {
				throw MessengerInternal.CreateBroadcastSignatureException(eventType);
			}
		}
		return null;
	}
	//插入前工作，检测键名是否存在，插入类型是否正确
	static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded) {
		if (!eventTable.ContainsKey(eventType)) {
			eventTable.Add(eventType, null);
		}
		
		var d = eventTable[eventType];
		if (d != null && d.GetType() != listenerBeingAdded.GetType()) {
			throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}
	//移除前工作，检测移除键名是否存在以及移除类型是否正确
	static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) {
		if (eventTable.ContainsKey(eventType)) {//若委托链表里有该委托
			var d = eventTable[eventType];//取得该键对应的键值
			
			if (d == null) {
				throw new ListenerException(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventType));
			} else if (d.GetType() != listenerBeingRemoved.GetType()) {
				throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		} else {//委托链表没有要移除的委托，抛出异常
			throw new ListenerException(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventType));
		}
	}
	//移除委托后，判断委托链上该键名下是否还有其他委托，没有则移除该键值对
	static public void OnListenerRemoved(string eventType) {
		if (eventTable[eventType] == null) {
			eventTable.Remove(eventType);
		}
	}
	//广播前的准备工作
	static public void OnBroadcasting(string eventType, MessengerMode mode) {
		if (mode == MessengerMode.REQUIRE_LISTENER && !eventTable.ContainsKey(eventType)) {
			throw new MessengerInternal.BroadcastException(string.Format("Broadcasting message {0} but no listener found.", eventType));
		}
	}
	//广播失败抛出异常
	static public BroadcastException CreateBroadcastSignatureException(string eventType) {
		return new BroadcastException(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventType));
	}
	//自定义广播异常
	public class BroadcastException : Exception {
		public BroadcastException(string msg)
		: base(msg) {
		}
	}
	//自定义监听异常
	public class ListenerException : Exception {
		public ListenerException(string msg)
		: base(msg) {
		}
	}
}

// 接收零个参数
static public class Messenger
{ 
	static public void AddListener(string eventType, Action handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
																//Func<TReturn>是返回一个泛型的无输入参数的委托
	static public void AddListener<TReturn>(string eventType, Func<TReturn> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
															//Action是无输入参数无返回类型的委托
	static public void RemoveListener(string eventType, Action handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}
	
	static public void RemoveListener<TReturn>(string eventType, Func<TReturn> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}

	//这个广播方法输出所有无返回值无输入参数的方法
	static public void Broadcast(string eventType) {
		Broadcast(eventType, MessengerInternal.DEFAULT_MODE);
	}
	//两个参数的重载方法
	static public void Broadcast(string eventType, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);//广播前准备工作
		var invocationList = MessengerInternal.GetInvocationList<Action>(eventType);//获取当前键名下所有方法
		
		foreach(var callback in invocationList)
			callback.Invoke();//遍历所有方法并输出
	}

	//这个广播方法输出所有无输入参数，有一个返回值的方法,Action<TReturn>用来接收返回值
	static public void Broadcast<TReturn>(string eventType, Action<TReturn> returnCall) {
		Broadcast(eventType, returnCall, MessengerInternal.DEFAULT_MODE);
	}
	//三个参数的重载方法										//这里也改一下，函数的输入参数改成接收泛型数组
	static public void Broadcast<TReturn>(string eventType, Action<TReturn> returnCall, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);//广播前准备工作
		var invocationList = MessengerInternal.GetInvocationList<Func<TReturn>>(eventType);//获取该键名下所有方法
		//遍历委托链表里的该键名下每一个方法，将得到的返回值转换成<TReturn>类型，作为键值放到该键名下
		foreach(var result in invocationList.Select(del => del.Invoke()).Cast<TReturn>())
		{
			returnCall.Invoke(result);//遍历取出每一个返回值，作为输入参数传给回调函数
		}
		//这里改一下，把返回值构成的泛型数组直接作为参数传给函数returnCall
 //      returnCall.Invoke(invocationList.Select(del =>del.Invoke()).Cast<TReturn>());

	}
}

// One parameter
static public class Messenger<T> {
	static public void AddListener(string eventType, Action<T> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
	
	static public void AddListener<TReturn>(string eventType, Func<T, TReturn> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
	
	static public void RemoveListener(string eventType, Action<T> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}
	
	static public void RemoveListener<TReturn>(string eventType, Func<T, TReturn> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}

	//这个广播方法输出所有无返回值、有一个输入参数的方法
	static public void Broadcast(string eventType, T arg1) {
		Broadcast(eventType, arg1, MessengerInternal.DEFAULT_MODE);
	}
	//三个参数的重载广播方法
	static public void Broadcast(string eventType, T arg1, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action<T>>(eventType);
		
		foreach(var callback in invocationList)
			callback.Invoke(arg1);
	}

	//这个广播方法输出所有有返回值、有一个输入参数的方法，第三个参数Action<TRturn>用来接受返回值
	static public void Broadcast<TReturn>(string eventType, T arg1, Action<TReturn> returnCall) {
		Broadcast(eventType, arg1, returnCall, MessengerInternal.DEFAULT_MODE);
	}
	//四个参数的重载广播方法
	static public void Broadcast<TReturn>(string eventType, T arg1, Action<TReturn> returnCall, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<T, TReturn>>(eventType);
		//遍历委托链表里的该键名下每一个方法，传入参数arg1，将得到的返回值转换成<TReturn>类型，作为键值放到该键名下

	foreach(var result in invocationList.Select(del => del.Invoke(arg1)).Cast<TReturn>()) {
			returnCall.Invoke(result);//遍历取出每一个返回值，作为输入参数传给回调函数
		}
//		returnCall.Invoke(invocationList.Select(del =>del.Invoke(arg1)).Cast<TReturn>());
	}
}

// Two parameters
static public class Messenger<T, U> { 
	static public void AddListener(string eventType, Action<T, U> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
	
	static public void AddListener<TReturn>(string eventType, Func<T, U, TReturn> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
	
	static public void RemoveListener(string eventType, Action<T, U> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}
	
	static public void RemoveListener<TReturn>(string eventType, Func<T, U, TReturn> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}
	//这个广播方法输出所有无返回值、有两个个输入参数的方法
	static public void Broadcast(string eventType, T arg1, U arg2) {
		Broadcast(eventType, arg1, arg2, MessengerInternal.DEFAULT_MODE);
	}
	//四个参数的重载广播方法
	static public void Broadcast(string eventType, T arg1, U arg2, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action<T, U>>(eventType);
		
		foreach(var callback in invocationList)
			callback.Invoke(arg1, arg2);
	}

	//这个广播方法输出所有有返回值、有两个输入参数的方法，第四个参数Action<TRturn>用来接受返回值
	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, Action<TReturn> returnCall) {
		Broadcast(eventType, arg1, arg2, returnCall, MessengerInternal.DEFAULT_MODE);
	}
	//五个参数的重载广播方法
	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, Action<TReturn> returnCall, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<T, U, TReturn>>(eventType);
		
		foreach(var result in invocationList.Select(del => del.Invoke(arg1, arg2)).Cast<TReturn>()) {
			returnCall.Invoke(result);
		}
	}
}

// Three parameters
static public class Messenger<T, U, V> { 
	static public void AddListener(string eventType, Action<T, U, V> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
	
	static public void AddListener<TReturn>(string eventType, Func<T, U, V, TReturn> handler) {
		MessengerInternal.AddListener(eventType, handler);
	}
	
	static public void RemoveListener(string eventType, Action<T, U, V> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}
	
	static public void RemoveListener<TReturn>(string eventType, Func<T, U, V, TReturn> handler) {
		MessengerInternal.RemoveListener(eventType, handler);
	}
	
	static public void Broadcast(string eventType, T arg1, U arg2, V arg3) {
		Broadcast(eventType, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
	}
	
	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, V arg3, Action<TReturn> returnCall) {
		Broadcast(eventType, arg1, arg2, arg3, returnCall, MessengerInternal.DEFAULT_MODE);
	}
	
	static public void Broadcast(string eventType, T arg1, U arg2, V arg3, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action<T, U, V>>(eventType);
		
		foreach(var callback in invocationList)
			callback.Invoke(arg1, arg2, arg3);
	}
	
	static public void Broadcast<TReturn>(string eventType, T arg1, U arg2, V arg3, Action<TReturn> returnCall, MessengerMode mode) {
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<T, U, V, TReturn>>(eventType);
		
		foreach(var result in invocationList.Select(del => del.Invoke(arg1, arg2, arg3)).Cast<TReturn>()) {
			returnCall.Invoke(result);
		}
	}
}

