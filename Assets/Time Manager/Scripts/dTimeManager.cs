using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiamondMind.TimeManager
{
    public class dTimeManager : MonoBehaviour
    {
        public float timeElapsed;
        public float timeScale;
        public bool paused;

        [Header("Slow Motion")]
        public bool isInSlowMotion;
        [Range(0.1f, 0.9f)] public float slowMotionFactor = 0.1f;
        [Tooltip("Speed at which timescale returns to normal")]
        public float slowMotionSpeed = 5f;

        [Header("Fast Forward")]
        public bool isFastForwarding;
        [Range(1.1f, 3f)] public float fastForwardFactor = 2f;
        [Tooltip("Speed at which timescale returns to normal")]
        public float fastForwardSpeed = 5f;

        [Header("Rewind")]
        public bool isRewinding;
        public GameObject[] objectsToRewind;

        Scene _scene;

        private void Start()
        {
            _scene = SceneManager.GetActiveScene();

            foreach(GameObject go in objectsToRewind)
            {
                go.AddComponent<dRewindController>();   // add the rewind component automatically
            }
        }
        private void FixedUpdate()
        {
            timeElapsed = Mathf.Round(Time.fixedUnscaledTime);
        }
        void Update()
        {
            timeScale = Time.timeScale;
            paused = dPauseManager.IsPaused;    // show pause state

            isRewinding = objectsToRewind[0].GetComponent<dRewindController>().canRewind; 

            // reset scene
            if (Input.GetKey(KeyCode.Tab))
            {
                ResetScene();
            }

            // slow motion
            if (Input.GetKey(KeyCode.Space) && !isInSlowMotion && !isFastForwarding)
            {
                DoSlowMotion();
            }
            if(isInSlowMotion)
            {
                if (Time.timeScale > 0.01f && Time.timeScale < 1f)
                {
                    Time.timeScale += (1f / slowMotionSpeed) * Time.unscaledDeltaTime;    // slowly return to normal time
                }
                if (Time.timeScale > 0.9f && Time.timeScale < 1.1f)
                {
                    Time.timeScale = 1f;
                    isInSlowMotion = false;
                }
            }

            // fast forward
            if (Input.GetKey(KeyCode.F) && !isFastForwarding && !isInSlowMotion)
            {
                FastForward();
            }
            if(isFastForwarding)
            {
                if (Time.timeScale > 1f)
                {
                    Time.timeScale -= (1f / fastForwardSpeed) * Time.unscaledDeltaTime;  // slowly return to normal time
                }
                if(Time.timeScale > 0.9f && Time.timeScale < 1.1f)
                {
                    Time.timeScale = 1f;
                    isFastForwarding = false;
                }
            }

            // rewind
            if (Input.GetKeyDown(KeyCode.Return) && !isInSlowMotion && !isFastForwarding)
            {
                foreach (var dRewindController in objectsToRewind)
                {
                    dRewindController.GetComponent<dRewindController>().StartRewind();
                }
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                foreach (var dRewindController in objectsToRewind)
                {
                    dRewindController.GetComponent<dRewindController>().StopRewind();
                }
            }    
        }

        void ResetScene()
        { 
            Time.timeScale = 1f;
            SceneManager.LoadScene(_scene.name);
        }
        public void DoSlowMotion()
        {
            isInSlowMotion = true;

            Time.timeScale = slowMotionFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;   // set fixed update speed
        }
        public void FastForward()
        {
            isFastForwarding = true;
      
            Time.timeScale = fastForwardFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;   // set fixed update speed
        }
    }
}