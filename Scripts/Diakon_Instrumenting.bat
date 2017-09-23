cd "C:\debugger\daikon-dot-net-front-end-master\daikon-dot-net-front-end-master\SampleProjects\BankAccount\UnitTestProject1\bin\Debug"

CeleriacLauncher.exe --is_property_flags --is_readonly_flags --purity-file=BankAccount_auto.pure --save-program=BankAccount.dll.instr BankAccount.dll

move BankAccount.dll BankAccount.dll.orig
move BankAccount.dll.instr BankAccount.dll

echo Done
pause