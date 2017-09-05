MdbgCmd.SetLogFile('C:\\dbg\\Log.log', True, True)

ps = MdbgCmd.ListProcesses()

calculator = [p for p in ps if p.ProcessName == 'Calculator']

MdbgCmd.AttachToProcess(calculator[0]);
MdbgCmd.SetBreakPoint("MathEvaluator.cs", 156);

def onNext(locationState):
    if locationState.FileName != "MathEvaluator.cs":
        MdbgCmd.Go(testBegin, onBreak)
    else:
        MdbgCmd.Print()
        MdbgCmd.Next(None, onNext)

def onBreak(locationState):
    MdbgCmd.Print()
    MdbgCmd.Next(None, onNext)
    
def testBegin():
    MdbgCmd.TestBegin()
    
MdbgCmd.Go(testBegin, onBreak)
#C:\debugger\Scriptable-Debugger\Scriptable-Debugger\Scripts\Script.py