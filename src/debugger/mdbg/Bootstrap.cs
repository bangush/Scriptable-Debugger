//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------
using IronPython.Hosting;
using Microsoft.Samples.Tools.Mdbg;
using Microsoft.Scripting;
using System;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;

// This is declared in the assemblyrefs file
//[assembly:System.Runtime.InteropServices.ComVisible(false)]
#pragma warning disable 618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
#pragma warning restore 618

// Main entry point to the managed debugger.
public class Bootstap
{
    [MTAThread]
    public static int Main(string[] args)
    {
        if (args.Length == 0 || !Regex.IsMatch(args[0].Trim('"'), @"\.py$"))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"Please specify a python script file.");
            Console.ResetColor();
            Console.ReadKey();
            return 1;
        }
        else if (!File.Exists(args[0].Trim('"')))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"The python script file does not exist.");
            Console.ResetColor();
            Console.ReadKey();
            return 2;
        }

        MDbgShell shell = new MDbgShell();
        shell.Start(new string[] { });

        var engine = Python.CreateEngine();
        var scope = engine.CreateScope();
        engine.Execute("import clr", scope);
        engine.Execute($@"clr.AddReferenceToFileAndPath('{AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\")}mdbgTypes.dll')", scope);
        engine.Execute($@"clr.AddReferenceToFileAndPath('{AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\")}mdbgeng.dll')", scope);
        engine.Execute($@"clr.AddReferenceToFileAndPath('{AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\")}mdbgext.dll')", scope);
        engine.Execute($@"clr.AddReferenceToFileAndPath('{AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\")}PhiDebugging.dll')", scope);
        engine.Execute($@"clr.AddReferenceToFileAndPath('{AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", @"\\")}InvariantDetector.dll')", scope);
        engine.Execute(@"from PhiDebugging import *", scope);
        engine.Execute(@"from InvariantDetector import *", scope);
        engine.Execute(@"from Microsoft.Samples.Debugging.MdbgEngine import *", scope);
        engine.Execute(@"from Microsoft.Samples.Tools.Mdbg import *", scope);

        try
        {
            engine.ExecuteFile(args[0].Trim('"'), scope);
        }
        catch (SyntaxErrorException syntaxError)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($@"Syntax error in line: {syntaxError.Line}, column: {syntaxError.Column}");
            Console.WriteLine(syntaxError.Message);
            Console.ResetColor();
        }        
        catch (Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"The script generated an '{ex.GetType().Name}' exception:");
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }

        Console.WriteLine("<------->");
        Console.ReadKey();

        return 0;
    }
}
