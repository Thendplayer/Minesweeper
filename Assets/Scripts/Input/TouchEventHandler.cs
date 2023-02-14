using System;
using UnityEngine;

namespace Minesweeper.Input
{
    public class TouchEventHandler : MonoBehaviour
    {
        public static Action<int, Vector3> OnTouchDetectedEvent;
        private ITouchEventStrategy inputStrategy;

        public static void ClearEvents()
        {
            OnTouchDetectedEvent = null;
        }

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
            inputStrategy = new DeviceTouchEventStrategy();
#endif

#if UNITY_EDITOR
            inputStrategy = new EditorTouchEventStrategy();
#endif
        }

        private void Update() 
        {
            inputStrategy?.Handle();
        }
    }
}