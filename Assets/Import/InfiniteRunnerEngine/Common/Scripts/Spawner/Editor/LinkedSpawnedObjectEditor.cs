using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MoreMountains.InfiniteRunnerEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor (typeof(LinkedSpawnedObject))]
[CanEditMultipleObjects]

public class LinkedSpawnedObjectEditor : Editor 
{
	protected LinkedSpawnedObject _linkedSpawnedObject;
	protected Renderer _renderer;
	protected GUIStyle _style;

	public virtual void OnEnable()
	{
		_linkedSpawnedObject = (LinkedSpawnedObject) target;
		_renderer=_linkedSpawnedObject.GetComponent<Renderer>();
		_style = new GUIStyle();
		_style.normal.textColor = Color.green;	

		if (_linkedSpawnedObject.In==Vector3.zero && _linkedSpawnedObject.Out==Vector3.zero )
		{
			ResetInOutPosition();

		}		
	}

	public virtual void OnSceneGUI()
	{
		if (_linkedSpawnedObject==null)
			return;
		if (_style==null)
			return;

		// we draw the In handle

		/*_style.normal.textColor = Color.green;	 
		Handles.Label(_linkedSpawnedObject.In+(Vector3.down*0.1f)+(Vector3.right*0.1f), _linkedSpawnedObject.gameObject.name+" In",_style);*/

		DrawHandle(_linkedSpawnedObject.gameObject,_linkedSpawnedObject.In,Color.green,_linkedSpawnedObject.gameObject.name+" In");
		DrawHandle(_linkedSpawnedObject.gameObject,_linkedSpawnedObject.Out,Color.red,_linkedSpawnedObject.gameObject.name+" Out");

	}

	protected virtual void DrawHandle(GameObject handleObject, Vector3 handlePosition, Color handleColor,String handleName)
	{
		Vector3 handleCenter = _linkedSpawnedObject.transform.position+handlePosition;

		Handles.color=handleColor;
		Handles.DrawLine(handleCenter+Vector3.up,handleCenter+Vector3.down);
		Handles.DrawLine(handleCenter+Vector3.left,handleCenter+Vector3.right);
		Handles.CircleHandleCap(0,
					handleCenter,
					Quaternion.identity,
	    			0.5f,
					EventType.Repaint);


		_style.normal.textColor = handleColor;	 
		Handles.Label(handleCenter+(Vector3.down*0.1f)+(Vector3.right*0.1f), handleName,_style);
	}

	public override void OnInspectorGUI()
	{
		_linkedSpawnedObject = (LinkedSpawnedObject)target;

		EditorGUILayout.LabelField("In/Out Set up",_linkedSpawnedObject.InOutSetup.ToString());

		DrawDefaultInspector();		

		if (GUILayout.Button("In/Out Reset"))
		{
			ResetInOutPosition();
		}
	}

	protected virtual void ResetInOutPosition()
	{
		_linkedSpawnedObject.In = _linkedSpawnedObject.transform.InverseTransformPoint(_linkedSpawnedObject.GetComponent<Renderer>().bounds.min);
		_linkedSpawnedObject.Out = _linkedSpawnedObject.transform.InverseTransformPoint(_linkedSpawnedObject.GetComponent<Renderer>().bounds.max);
		_linkedSpawnedObject.InOutSetup=true;
	}



}
