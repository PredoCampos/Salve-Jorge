using UnityEngine;
using UnityEditor;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// This class adds names for each LevelMapPathElement next to it on the scene view, for easier setup
	/// </summary>
	[CustomEditor(typeof(LevelManager))]
	[InitializeOnLoad]
	public class LevelManagerEditor : Editor 
	{		
		//protected SceneViewIcon _sceneViewIcon;

		[DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
		static void DrawGameObjectName(LevelManager levelManager, GizmoType gizmoType)
		{   
			GUIStyle style = new GUIStyle();
			Vector3 v3FrontTopLeft;

			if (levelManager.RecycleBounds.size!=Vector3.zero)
			{
		        style.normal.textColor = Color.yellow;		 
				v3FrontTopLeft = new Vector3(levelManager.RecycleBounds.center.x - levelManager.RecycleBounds.extents.x, levelManager.RecycleBounds.center.y + levelManager.RecycleBounds.extents.y + 1, levelManager.RecycleBounds.center.z - levelManager.RecycleBounds.extents.z);  // Front top left corner
				Handles.Label(v3FrontTopLeft, "Level Manager Recycle Bounds", style);
				MMDebug.DrawHandlesBounds(levelManager.RecycleBounds,Color.yellow);
			}

			if (levelManager.DeathBounds.size!=Vector3.zero)
			{
				style.normal.textColor = Color.red;		 
				v3FrontTopLeft = new Vector3(levelManager.DeathBounds.center.x - levelManager.DeathBounds.extents.x, levelManager.DeathBounds.center.y + levelManager.DeathBounds.extents.y + 1, levelManager.DeathBounds.center.z - levelManager.DeathBounds.extents.z);  // Front top left corner
				Handles.Label(v3FrontTopLeft, "Level Manager Death Bounds", style);
				MMDebug.DrawHandlesBounds(levelManager.DeathBounds,Color.red);
			}
		}
	}
}