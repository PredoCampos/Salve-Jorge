using UnityEngine;
using System.Collections;
using UnityEditor;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// A custom editor for the Camera Behavior component
	/// </summary>
	[CustomEditor(typeof(CameraBehavior))]
	public class CameraBehaviorEditor : Editor
	{
	    /// <summary>
	    /// Adds 2 buttons to the camera's inspector that allow for easier setup.
	    /// Just position the camera at the min or max position you'd like it to have ingame, and press the corresponding button.
	    /// It will auto-fill the corresponding variables.
	    /// </summary>

	    public override void OnInspectorGUI()
	    {
	        DrawDefaultInspector();
	        CameraBehavior _cameraBehavior = (CameraBehavior)target;

	        if (GUILayout.Button("Set Current Camera Position as Minimum Zoom"))
	        {
	            _cameraBehavior.MinimumZoom = _cameraBehavior.transform.position;
				_cameraBehavior.MinimumZoomOrthographic = _cameraBehavior.GetComponent<Camera>().orthographicSize;
			}
			if (GUILayout.Button("Set Current Camera Position as Maximum Zoom"))
	        {
				_cameraBehavior.MaximumZoom = _cameraBehavior.transform.position;
				_cameraBehavior.MaximumZoomOrthographic = _cameraBehavior.GetComponent<Camera>().orthographicSize;
			}
	    
	    }
	}
}