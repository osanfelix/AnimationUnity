using UnityEngine;
using System.Collections.Generic;

public class KeyFunction : MonoBehaviour
{
	#region STATIC ATTRIBUTES
	public static IList<KeyFunction> keyFunctionList = new List<KeyFunction>();
	#endregion

	
	#region ATTRIBUTES
	public delegate void voidDelegate();
	private voidDelegate _voidFunc = null;
	private KeyCode _key;
	private bool _locker = false;
	#endregion

	
	#region CONSTRUCTOR
	public static KeyFunction createInstance(KeyCode key, voidDelegate voidFunc)
	{
		KeyFunction _instance = new GameObject("KeyFunction_" + keyFunctionList.Count.ToString()).gameObject.AddComponent<KeyFunction>();
		moveToKeyFunctionsGroup(_instance);
		
		_instance.setKey(key);
		_instance.setFunction(voidFunc);

		keyFunctionList.Add(_instance);
		return _instance;
	}
	#endregion

	
	#region DELEGATES
	public void setFunction(voidDelegate voidFunc)
	{
		this._voidFunc = voidFunc;
	}
	
	public void setKey(KeyCode key)
	{
		this._key = key;
	}
	#endregion
	
	#region STATIC METHODS
	public static void moveToKeyFunctionsGroup(KeyFunction keyFunction)
	{
		GameObject folder = GameObject.Find("Key_Functions");			// KeyFunction folder
		if(folder == null)
			folder = new GameObject("Key_Functions");
		keyFunction.transform.parent = folder.transform;
	}
	#endregion

	
	#region BEHAVIOUR METHODS
	void Update()
	{
		if(Input.GetKey(_key))
		{
			if(!_locker)
			{
				_locker = true;
				_voidFunc();
			}
		}
		else
			_locker = false;
	}
	#endregion
}