using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MoreMountains.InfiniteRunnerEngine
{	

	[CustomEditor(typeof(GameManager))]
	[CanEditMultipleObjects]

	/// <summary>
	/// Game manager editor
	/// </summary>

	public class GameManagerEditor : Editor
	{

	    private GameManager _target
	    {
	        get { return (GameManager)target; }
	    }

	    /// <summary>
	    /// When inspecting a Corgi Controller, we add to the regular inspector some labels, useful for debugging
	    /// </summary>
	    public override void OnInspectorGUI()
	    {
	        if (_target != null)
	        {
	            EditorGUILayout.LabelField("Status", _target.Status.ToString());
	        }
	        DrawDefaultInspector();


	    }

	    void OnEnable()
	    {
	        _target.GameManagerInspectorNeedRedraw += this.Repaint;
	    }
	    
	    void OnDisable()
	    {
	        _target.GameManagerInspectorNeedRedraw -= this.Repaint;
	    }
	}
}