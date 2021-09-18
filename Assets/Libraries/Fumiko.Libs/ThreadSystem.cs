using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Fumiko.Systems.Debug;

namespace Fumiko.Systems.Threading
{
    public class ThreadSystem : MonoBehaviour
    {
        public static ThreadSystem instance;

        private int currentThreads = 0;

        private int maximumNumberOfThreads;
        private List<AdditionalThread> threadQueue = new List<AdditionalThread>();

        public int MaximumThreads
        {
            get
            {
                return maximumNumberOfThreads;
            }
        }

        public void CalculateMaximumThreads()
        {
            maximumNumberOfThreads = Environment.ProcessorCount;

            LogSystem.instance.Log(
                content: maximumNumberOfThreads,
                identifier: "Maximum Number of Threads:",
                debugType: DebugType.DEBUG,
                direct: true
            );
        }

        public bool RunAsThread(AdditionalThread thread)
        {
            if (currentThreads < maximumNumberOfThreads)
            {
                threadQueue.RemoveAll(x => x.hash == thread.hash);

                currentThreads++;

                LogSystem.instance.Log(
                    content: thread.identifier,
                    identifier: "Starting Thread..",
                    debugType: DebugType.DEBUG,
                    direct: true
                );

                Thread sysThread = new Thread(thread.Run);
                thread.myThread = sysThread;
                sysThread.Start();

                return true;
            }
            else if (!threadQueue.Exists(x => x.hash == thread.hash))
            {
                LogSystem.instance.Log(
                    content: thread.identifier,
                    identifier: "Adding Thread to Queue..",
                    debugType: DebugType.DEBUG,
                    direct: true
                );

                threadQueue.Add(thread);
            }
            else
            {
                LogSystem.instance.Log(
                    content: thread.identifier,
                    identifier: "Thread already in queue.",
                    debugType: DebugType.DEBUG,
                    direct: true
                );
            }

            return false;
        }

        public void ThreadFinished(AdditionalThread call, bool successFull = true, Exception e = null)
        {
            currentThreads = Mathf.Clamp(currentThreads--, 0, currentThreads);

            if (successFull)
            {
                LogSystem.instance.Log(
                    content: currentThreads,
                    identifier: $"Finished Thread ({call.identifier})! Threads Left:",
                    debugType: DebugType.DEBUG,
                    errorType: ErrorType.SUCCESS,
                    direct: true
                );
            }
            else
            {
                LogSystem.instance.Log(
                    content: e.Message,
                    identifier: $"Thread Crashed: ({call.identifier})!:",
                    debugType: DebugType.DEBUG,
                    errorType: ErrorType.ERROR,
                    direct: true
                );
            }
        }

        private void Start()
        {
            CalculateMaximumThreads();
        }

        private void Update()
        {
            if (DebugIntervalSystem.instance.RunDebugQuarterInterval && threadQueue.Count > 0)
            {
                RunAsThread(threadQueue[0]);
            }
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