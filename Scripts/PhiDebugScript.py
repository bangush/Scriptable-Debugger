MdbgCmd.SetLogFile('C:\\dbg\\Log.log', TestName.Ask_for_name, LoggingAction.Overwrite)

ps = MdbgCmd.ListProcesses()
calculator = [p for p in ps if p.ProcessName == 'Calculator']

MdbgCmd.AttachToProcess(calculator[0]);
MdbgCmd.SetBreakPoint("CalculatorForm.cs", 88);

def onNext(locationState):
    if locationState.FileName == "":
        MdbgCmd.StepOut(None, onNext)
    elif locationState.FileName == "CalculatorForm.cs" and locationState.LineNumber > 88:
        MdbgCmd.Go(testBegin, onBreak)
    else:
        MdbgCmd.Print(debuggerVars=False, canDoFunceval=True, expandDepth=3)
        MdbgCmd.Step(None, onNext)

def onBreak(locationState):
    MdbgCmd.Print(debuggerVars=False, canDoFunceval=True, expandDepth=3)
    MdbgCmd.Step(None, onNext)
    
def testBegin():
    MdbgCmd.TestBegin()
    
MdbgCmd.Go(testBegin, onBreak)
#C:\debugger\Scriptable-Debugger\Scriptable-Debugger\Scripts\PhiDebugScript.py