using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fumiko.Systems.Debug
{
    public class LogSystem : MonoBehaviour
    {
        public static LogSystem instance;

        public List<DebugType> debugTypes;

        public bool showDebugMessages = true;

        private HashSet<int> debugStatementHashes = new HashSet<int>();

        private List<DebugStatement> debugStatements = new List<DebugStatement>();

        private bool debugTypeActive = false;

        public void Log(
            object content,
            string identifier = "",
            DebugType debugType = DebugType.NONE,
            ErrorType errorType = ErrorType.INFO,
            ExecutionInterval executionInterval = ExecutionInterval.ONCE,
            bool direct = false
        )
        {
            if (!showDebugMessages)
            {
                return;
            }

            debugTypeActive = false;
            for (int i = 0; i < debugTypes.Count; i++)
            {
                if (debugTypes[i] == debugType)
                {
                    debugTypeActive = true;
                }
            }

            if (!debugTypeActive)
            {
                return;
            }

            DebugStatement debugStatement = new DebugStatement
            {
                content = content,
                time = DateTime.Now,
                debugType = debugType,
                errorType = errorType,
                identifier = identifier,
                interval = executionInterval
            };

            int randomHashCode = 0;

            if (executionInterval != ExecutionInterval.FRAME &&
                executionInterval != ExecutionInterval.FIXED)
            {
                if (string.IsNullOrEmpty(debugStatement.identifier))
                {
                    string generatedHashCode = DateTime.Now + " : " + content;
                    randomHashCode = generatedHashCode.GetHashCode();
                }
            }

            debugStatement.hash = string.IsNullOrEmpty(debugStatement.identifier) ? randomHashCode : debugStatement.identifier.GetHashCode();

            if (direct)
            {
                debugStatement.Log();
            }
            else
            {
                if (debugStatement.hash == 0 || !debugStatementHashes.Contains(debugStatement.hash))
                {
                    debugStatements.Add(debugStatement);

                    if (debugStatement.hash != 0)
                    {
                        debugStatementHashes.Add(debugStatement.hash);
                    }
                }
            }
        }

        private void ClearDebugStatement(DebugStatement statement)
        {
            if (statement.hash != 0)
            {
                debugStatementHashes.Remove(statement.identifier.GetHashCode());
            }

            debugStatements.Remove(statement);
        }

        private void LogList()
        {
            int debugStatementsCount = debugStatements.Count;
            for (int i = 0; i < debugStatements.Count; i++)
            {
                if (debugStatementsCount != debugStatements.Count)
                {
                    debugStatementsCount = debugStatements.Count;
                    i = 0;
                }

                if (debugStatements[i].interval == ExecutionInterval.FRAME)
                {
                    debugStatements[i].Log();
                    ClearDebugStatement(debugStatements[i]);
                }
                else if (debugStatements[i].interval == ExecutionInterval.ONCE)
                {
                    debugStatements[i].Log();
                }
                else if (debugStatements[i].interval == ExecutionInterval.QUARTER_SECOND
                    && DebugIntervalSystem.instance.RunDebugQuarterInterval)
                {
                    debugStatements[i].Log();
                    ClearDebugStatement(debugStatements[i]);
                }
                else if (debugStatements[i].interval == ExecutionInterval.HALF_SECOND
                    && DebugIntervalSystem.instance.RunDebugHalfInterval)
                {
                    debugStatements[i].Log();
                    ClearDebugStatement(debugStatements[i]);
                }
                else if (debugStatements[i].interval == ExecutionInterval.SECOND
                    && DebugIntervalSystem.instance.RunDebugSecondInterval)
                {
                    debugStatements[i].Log();
                    ClearDebugStatement(debugStatements[i]);
                }
                else if (debugStatements[i].interval == ExecutionInterval.FIVE_SECOND
                    && DebugIntervalSystem.instance.RunDebugFiveSecondInterval)
                {
                    debugStatements[i].Log();
                    ClearDebugStatement(debugStatements[i]);
                }
                else if (debugStatements[i].interval == ExecutionInterval.TEN_SECOND
                    && DebugIntervalSystem.instance.RunDebugTenSecondInterval)
                {
                    debugStatements[i].Log();
                    ClearDebugStatement(debugStatements[i]);
                }

                // DEBUG INTERVALS CLEARED HERE PREVIOUSLY
                // ASSURE THAT STATEMENT IS ONLY RUN ONCE?

                // LOG ALL STATEMENTS AFTER A CERTAIN TIME LIMIT?
            }
        }

        public void DuplicateSingletonError()
        {
            Log(
                content: "Duplicate singleton detected! " + this.name,
                debugType: DebugType.CRITICAL,
                errorType: ErrorType.ERROR);
        }

        private void Update()
        {
            LogList();
        }

        private void Awake()
        {
            if (instance != null)
            {
                DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }
    }
}