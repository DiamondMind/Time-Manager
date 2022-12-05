using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiamondMind.TimeManager
{  
    public class dRewindController : MonoBehaviour
    {
        bool rewinding;
        public bool canRewind;
        public int recordTime = 5;

        List<dTimePoint> timePoints;

        Rigidbody _rb;
        void Start()
        {           
            timePoints = new List<dTimePoint>();
            _rb = GetComponent<Rigidbody>();
        }
   
        private void FixedUpdate()
        {
            if (rewinding)
                Rewind();
            else
                Record();
        }
        void Rewind()
        {
            if (timePoints.Count > 0) //continue while list isnt empty
            {
                canRewind = true;
                dTimePoint timePoint = timePoints[0];
                transform.position = timePoint.position;
                transform.rotation = timePoint.rotation;
                transform.localScale = timePoint.scale;
                timePoints.RemoveAt(0);
            }
            else
            {
                canRewind = false;
                StopRewind();
            }
           
        }
        void Record()
        {
            // stop recording oldest data after max rewind time
            if (timePoints.Count > Mathf.Round(recordTime * (1 / Time.fixedDeltaTime)))
            {
                timePoints.RemoveAt(timePoints.Count - 1);
            }

            timePoints.Insert(0, new dTimePoint(transform.position, transform.rotation, transform.localScale));
        }
        
        public void StartRewind()
        {
            rewinding = true;
            _rb.isKinematic = true;
        }
        public void StopRewind()
        {
            rewinding = false;
            _rb.isKinematic = false;
        }
    }
}

