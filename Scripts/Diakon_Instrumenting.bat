cd "C:\debugger\Calculator-master\Calculator-master\Build\Debug"

CeleriacLauncher.exe --is_property_flags --is_readonly_flags --robust-mode --source-language=CSharp --is-enum-flags --omit-var="(System\..*?\..*)" --sample-start=10 --purity-file=LoreSoft.MathExpressions_auto.pure --save-program=LoreSoft.MathExpressions.dll.instr LoreSoft.MathExpressions.dll

move LoreSoft.MathExpressions.dll LoreSoft.MathExpressions.dll.orig
move LoreSoft.MathExpressions.dll.instr LoreSoft.MathExpressions.dll

echo Done
pause