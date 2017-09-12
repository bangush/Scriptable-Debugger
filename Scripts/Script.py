MdbgCmd.SetLogFile('C:\\dbg\\Log.log', TestName.Empty, LoggingAction.Overwrite)

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
        MdbgCmd.Step(None, onNext)

def onBreak(locationState):
    MdbgCmd.Step(None, onNext)
    
def testBegin():
    MdbgCmd.TestBegin()
    
MdbgCmd.Go(testBegin, onBreak)
#C:\debugger\Scriptable-Debugger\Scriptable-Debugger\Scripts\Script.py