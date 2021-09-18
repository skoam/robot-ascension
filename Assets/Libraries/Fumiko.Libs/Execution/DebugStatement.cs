using System;

namespace Fumiko.Systems.Debug
{
    public class DebugStatement
    {
        public object content;
        public DebugType debugType;
        public ErrorType errorType;
        public int hash = 0;
        public string identifier = "";
        public ExecutionInterval interval;
        public DateTime time;

        private bool logged;

        public void Log()
        {
            if (logged)
            {
                return;
            }

            logged = true;

            if (errorType == ErrorType.CRITICAL)
            {
                UnityEngine.Debug.LogError("----------------------------------------------------------------");
                UnityEngine.Debug.LogError($"{DateTime.UtcNow} : {hash} : {debugType} : {errorType}");
                UnityEngine.Debug.LogError($"{identifier} : {content}");

                UnityEngine.Debug.Break();
            }
            else if (errorType == ErrorType.ERROR)
            {
                UnityEngine.Debug.LogError("----------------------------------------------------------------");
                UnityEngine.Debug.LogError($"{DateTime.UtcNow} : {hash} : {debugType} : {errorType}");
                UnityEngine.Debug.LogError($"{identifier} : {content}");
            }
            else if (errorType == ErrorType.WARNING)
            {
                UnityEngine.Debug.LogWarning("----------------------------------------------------------------");
                UnityEngine.Debug.LogWarning($"{DateTime.UtcNow} : {hash} : {debugType} : {errorType}");
                UnityEngine.Debug.LogWarning($"{identifier} : {content}");
            }
            else
            {
                UnityEngine.Debug.Log("----------------------------------------------------------------");
                UnityEngine.Debug.Log($"{DateTime.UtcNow} : {hash} : {debugType} : {errorType}");
                UnityEngine.Debug.Log($"{identifier} : {content}");
            }
        }
    }
}