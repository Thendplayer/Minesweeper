using System;
using UnityEngine;

namespace Minesweeper.Utils
{
    public class TouchEventHandler : MonoBehaviour
    {
        public static Action<Vector3> OnPrimaryTouchDetectedEvent;
        public static Action<Vector3> OnSecondaryTouchDetectedEvent;

        private bool pressed = false;
        private bool stationary = false;
        private float pressedTimer;
        
        public static void ClearEvents()
        {
            OnPrimaryTouchDetectedEvent = null;
            OnSecondaryTouchDetectedEvent = null;
        }
        
        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
            
            if (Input.touchCount > 0)
            {
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        pressed = true;
                        stationary = false;
                        pressedTimer = 0;
                        break;
                    
                    case TouchPhase.Ended:
                        if (pressed && !stationary)
                        {
                            OnPrimaryTouchDetectedEvent?.Invoke(Input.touches[0].position);
                        }

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
                    OnSecondaryTouchDetectedEvent?.Invoke(Input.touches[0].position);
                    
                    stationary = true;
                    pressedTimer = 0;
                }
            }
            
#endif

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                OnPrimaryTouchDetectedEvent?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                OnSecondaryTouchDetectedEvent?.Invoke(Input.mousePosition);
            }
#endif
        }
    }
}