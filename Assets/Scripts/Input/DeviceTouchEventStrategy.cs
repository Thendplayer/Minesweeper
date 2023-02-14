using UnityEngine;

namespace Minesweeper.Input 
{
	public class DeviceTouchEventStrategy : ITouchEventStrategy
	{
		private bool pressed = false;
		private bool stationary = false;
		private float pressedTimer;

		public void Handle()
		{
			if (UnityEngine.Input.touchCount > 0)
			{
				switch (UnityEngine.Input.touches[0].phase)
				{
					case TouchPhase.Began:
						pressed = true;
						stationary = false;
						pressedTimer = 0;
						break;

					case TouchPhase.Ended:
						TouchEventHandler.OnTouchDetectedEvent?.Invoke(0, UnityEngine.Input.mousePosition);

						pressed = stationary = false;
						pressedTimer = 0;
						break;

					case TouchPhase.Canceled:
						pressed = stationary = false;
						pressedTimer = 0;
						break;
				}
			}

			if (pressed && !stationary)
			{
				pressedTimer += Time.deltaTime;
				if (pressedTimer > 0.5f)
				{
					TouchEventHandler.OnTouchDetectedEvent?.Invoke(1, UnityEngine.Input.mousePosition);

					stationary = true;
					pressedTimer = 0;
				}
			}
		}
	}
}