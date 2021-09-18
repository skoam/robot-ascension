using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Systems.Debug
{
    public class DebugIntervalSystem : MonoBehaviour
    {
        public static DebugIntervalSystem instance;

        private float currentDebugInterval = 0;

        private float currentDebugIntervalInQuarters = 0;

        private float debugIntervalLength = 10;

        private HashSet<int> debugStatementHashes = new HashSet<int>();

        private List<DebugStatement> debugStatements = new List<DebugStatement>();

        private bool runDebugFiveSecondInterval = false;
        private bool runDebugHalfInterval = false;
        private bool runDebugQuarterInterval = false;
        private bool runDebugSecondInterval = false;
        private bool runDebugTenSecondInterval = false;

        public bool RunDebugFiveSecondInterval
        {
            get
            {
                return runDebugFiveSecondInterval;
            }
        }

        public bool RunDebugHalfInterval
        {
            get
            {
                return runDebugHalfInterval;
            }
        }

        public bool RunDebugQuarterInterval
        {
            get
            {
                return runDebugQuarterInterval;
            }
        }

        public bool RunDebugSecondInterval
        {
            get
            {
                return runDebugSecondInterval;
            }
        }

        public bool RunDebugTenSecondInterval
        {
            get
            {
                return runDebugTenSecondInterval;
            }
        }

        private void CalculateDebugIntervals()
        {
            if (currentDebugInterval >= debugIntervalLength)
            {
                currentDebugInterval = 0;
                currentDebugIntervalInQuarters = 0;
                return;
            }

            ClearDebugIntervals();

            currentDebugInterval += Time.deltaTime;

            if (currentDebugIntervalInQuarters + 0.25f < currentDebugInterval)
            {
                currentDebugIntervalInQuarters += 0.25f;
                runDebugQuarterInterval = true;

                if (currentDebugIntervalInQuarters % 0.5f == 0)
                {
                    runDebugHalfInterval = true;
                }

                if (currentDebugIntervalInQuarters % 1 == 0)
                {
                    runDebugSecondInterval = true;
                }

                if (currentDebugIntervalInQuarters % 5 == 0)
                {
                    runDebugFiveSecondInterval = true;
                }

                if (currentDebugIntervalInQuarters % 10 == 0)
                {
                    runDebugTenSecondInterval = true;
                }
            }
        }

        private void ClearDebugIntervals()
        {
            runDebugQuarterInterval = false;
            runDebugHalfInterval = false;
            runDebugSecondInterval = false;
            runDebugFiveSecondInterval = false;
            runDebugTenSecondInterval = false;
        }

        private void Update()
        {
            CalculateDebugIntervals();
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }
    }
}