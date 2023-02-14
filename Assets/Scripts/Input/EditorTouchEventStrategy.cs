namespace Minesweeper.Input 
{
	public class EditorTouchEventStrategy : ITouchEventStrategy
	{
		public void Handle() 
		{
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				TouchEventHandler.OnTouchDetectedEvent?.Invoke(0, UnityEngine.Input.mousePosition);
			}
			else if (UnityEngine.Input.GetMouseButtonDown(1))
			{
				TouchEventHandler.OnTouchDetectedEvent?.Invoke(1, UnityEngine.Input.mousePosition);
			}
		}
	}
}