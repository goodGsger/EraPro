using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Framework
{
    public class CmdResult
    {
        public int code;
        public string result;

        public CmdResult(int code, string result)
        {
            this.code = code;
            this.result = result;
        }
    }

    public static class CmdUtil
    {
        public static CmdResult ProcessCommand(string command, string argument = "")
        {
            ProcessStartInfo start = new ProcessStartInfo(command);
            start.Arguments = argument;
            start.CreateNoWindow = false;
            start.ErrorDialog = true;
            start.UseShellExecute = false;

            if (start.UseShellExecute)
            {
                start.RedirectStandardOutput = false;
                start.RedirectStandardError = false;
                start.RedirectStandardInput = false;
            }
            else
            {
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.RedirectStandardInput = true;
                start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
                start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            }

            Process p = Process.Start(start);
            string result = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            int exitCode = p.ExitCode;
            p.Close();

            return new CmdResult(exitCode, result);
        }
    }

}
