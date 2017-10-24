MdbgCmd.SetLogFile('C:\\dbg\\Log.log', TestName.Empty, LoggingAction.Overwrite)

ps = MdbgCmd.ListProcesses()
calculator = [p for p in ps if p.ProcessName == 'Calculator']

MdbgCmd.AttachToProcess(calculator[0]);
MdbgCmd.SetBreakPoint("CalculatorForm.cs", 88);

variablesToLog = Dictionary[str,int]()
variablesToLog['this._eval._variables.entries'] = 2

def onNext(locationState):
    if locationState.FileName == "":
        MdbgCmd.StepOut(None, onNext)
    elif locationState.FileName == "CalculatorForm.cs" and locationState.LineNumber > 88:
        MdbgCmd.EndTest(TestResult.No_result)
        MdbgCmd.Go(testBegin, onNext)
    else:
        MdbgCmd.Print(debuggerVars=False, canDoFunceval=True, expandDepth=0, variablesToLog=variablesToLog)
        #MdbgCmd.Print(debuggerVars=False, canDoFunceval=True, expandDepth=4)
        MdbgCmd.Step(None, onNext)
    
def testBegin():
    MdbgCmd.TestBegin()
    
MdbgCmd.Go(testBegin, onNext)
#C:\debugger\Scriptable-Debugger\Scriptable-Debugger\Scripts\CodeSliceScript.py