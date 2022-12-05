using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiamondMind.TimeManager
{
    public class dPauseManager : MonoBehaviour
    {
        public static bool IsPaused = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        void Pause()
        {
            Time.timeScale = 0f;
            IsPaused = true;
        }
        void Resume()
        {
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }

}
