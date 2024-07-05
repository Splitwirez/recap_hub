using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ReCap.Hub.Data;

namespace ReCap.Hub
{
    partial class Program
    {
        static bool _exceptionShown = false;
        static void ShowException(Exception exception)
        {
            if (!_exceptionShown)
            {
                _exceptionShown = true;

                string errorTitle = "An error has occurred";
                string followingInformation = "along with the following information:\n"
                    + "    - Your operating system\n"
                    + "    - The version of Darkspore you were using at the time\n"
                    + "    - How you obtained Darkspore\n"
                    + "    - Where you have Darkspore installed to\n"
                    + "    - Describe whatever was happening at the time, as best you can\n"
                    + "    - Describe what you were doing at the time, as best you can\n"
                    + "    - Other stuff TBD (if you see this line, Splitwirez forgor :skull: )";


                List<Exception> exceptions = new List<Exception>();
                Exception current = exception;
                int count = 0;
                while (current != null)
                {
                    exceptions.Add(current);
                    count++;
                    current = current.InnerException;
                    if ((count > 4) || (current.InnerException == current) || (current.InnerException == null))
                        break;
                }


                try
                {
                    string stackTraceDir = Path.Combine(HubGlobalPaths.CfgPath, "StackTraceDumps");
                    if (!Directory.Exists(stackTraceDir))
                        Directory.CreateDirectory(stackTraceDir);
                    string stackTraceDumpPath = Path.Combine(stackTraceDir, DateTime.UtcNow.ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalMilliseconds + ".txt");


                    string stackTraceDump = string.Empty;
                    for (int i = 0; i < exceptions.Count; i++)
                    {
                        Exception ex = exceptions[i];
                        stackTraceDump += $"======== EXCEPTION [{i}] =>\n\n";
                        stackTraceDump += GetExceptionText(ex, string.Empty);
                        stackTraceDump += "\n\n\n\n\n\n";
                    }
                    File.WriteAllText(stackTraceDumpPath, stackTraceDump);
                    string errorText
                        = "Further information has been saved to the following location:\n"
                        + "    \"stackTraceDumpPath\"\n"
                        + "Please inform the Resurrection Capsule team of this, " + followingInformation
                    ;
                    //HubGlobalPaths
                    try
                    {
                        Process.Start(new ProcessStartInfo(stackTraceDir)
                        {
                            UseShellExecute = true
                        });
                    }
                    catch
                    { }
                }
                catch (Exception literallyHow)
                {
                    errorTitle += " [#]";
                    string errorText = "\n\nPlease report this error to the Resurrection Capsule team, including screenshots of all error messages, " + followingInformation; //along with a description of what you were doing at the time.\n\nThe Resurrection Capsule Hub will exit after the last nested exception has been reported.";
                    
                    //literallyHow
                    for (int i = 0; i < exceptions.Count; i++)
                    {
                        Exception ex = exceptions[i];
                        NMessageBox.Show(IntPtr.Zero, GetExceptionText(ex, errorText), errorTitle.Replace("#", count.ToString()));
                    }
                }
            }
        }

        static string GetExceptionText(Exception current, string errorText)
        {
            return current.GetType() + ": " + current.Message + "\n" + current.Source + "\n" + current.StackTrace + errorText;
        }
    }
}
