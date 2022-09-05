using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	public class SwipeZone : MMSwipeZone 
	{
		protected override void Swipe()
		{
			switch (_swipeDirection)
			{
				case MMPossibleSwipeDirections.Down:
					InputManager.Instance.DownButtonDown();
					break;
				case MMPossibleSwipeDirections.Up:
					InputManager.Instance.UpButtonDown();
					break;
				case MMPossibleSwipeDirections.Left:
					InputManager.Instance.LeftButtonDown();
					break;
				case MMPossibleSwipeDirections.Right:
					InputManager.Instance.RightButtonDown();
					break;
			}
		}

		protected override void Press()
		{
			InputManager.Instance.MainActionButtonDown();
		}
			
	}
}
