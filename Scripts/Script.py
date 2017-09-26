MdbgCmd.SetLogFile('C:\\dbg\\Log.log', TestName.Empty, LoggingAction.Overwrite)

ps = MdbgCmd.ListProcesses()
calculator = [p for p in ps if p.ProcessName == 'Calculator']

MdbgCmd.AttachToProcess(calculator[0]);
MdbgCmd.SetBreakPoint("MathEvaluator.cs", 170);

def onNext(locationState):
    if locationState.FileName == "":
        MdbgCmd.StepOut(None, onNext)
    elif locationState.FileName == "MathEvaluator.cs" and locationState.Function == "TryNumber" and locationState.LineNumber > 170:
        MdbgCmd.EndTest(TestResult.No_result)
        MdbgCmd.Go(testBegin, onBreak)
    else:
        MdbgCmd.Step(None, onNext)

def onBreak(locationState):
    MdbgCmd.Step(None, onNext)
    
def testBegin():
    MdbgCmd.TestBegin()
    
MdbgCmd.Go(testBegin, onBreak)
#C:\debugger\Scriptable-Debugger\Scriptable-Debugger\Scripts\Script.py