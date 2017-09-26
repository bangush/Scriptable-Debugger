cd "C:\debugger\Calculator-master\Calculator-master\Build\Debug"

>output.txt (
java -cp C:\daikon\daikon.jar daikon.Daikon --format csharpcontract .\daikon-output\LoreSoft.MathExpressions.decls ".\daikon-output\LoreSoft.MathExpressions.dtrace"
)

echo Done
pause