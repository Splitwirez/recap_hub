using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReCap.Hub.Data;

namespace ReCap.Hub
{
    public class CommandLine
    {
        static readonly char QUOT = OperatingSystem.IsWindows() ? '"' : '\'';


        public const string OPT_PATCH_EXE = "--patch-darkspore-exe";
        //public const string OPT_UNPATCH_EXE = "--unpatch-darkspore-exe";
        public const string OPT_COPY_PACKAGES = "--copy-packages";
        public const string OPT_NO_GUI = "--no-gui";
        public const string OPT_DEBUG = "--debug";

        public static readonly CommandLine Instance = new CommandLine();



        int _exitCode = 0;
        public int ExitCode
        {
            get => _exitCode;
        }

        bool _showGUI = true;
        public bool ShowGUI
        {
            get => _showGUI;
        }

        bool _showDebugInfo = false;
        public bool ShowDebugInfo
        {
            get => _showDebugInfo;
        }

        public bool MainParseCommandLine(string[] args, out IEnumerable<Exception> exceptions)
        {
            List<Exception> exs = new List<Exception>();
            _exitCode = 0;
            string patchExeSrcPath = null;
            string patchExeDestPath = null;
            Dictionary<string, string> copyPackagePaths = null;
            //Debug.WriteLine($"CLI argument count: {args.Length}");

            int argCount = args.Length;
            if (argCount > 0)
                NMessageBox.DebugShow(IntPtr.Zero, $"argCount: {argCount}", "# of args", 0);

            for (int argsIndex = 0; argsIndex < argCount; argsIndex++)
            {
                int argIndex = argsIndex;

                //Debug.WriteLine($"\t[{argIndex}]: '{arg}'");
                if (CommandLine.TryParseArg_PatchExecutable(ref args, ref argIndex, out string patchExeSrcPathO, out string patchExeDestPathO))
                {
                    patchExeSrcPath = patchExeSrcPathO;
                    patchExeDestPath = patchExeDestPathO;
                    NMessageBox.DebugShow(IntPtr.Zero, $"src: '{patchExeSrcPath}'\ndest: '{patchExeDestPath}'\nargIndex: {argIndex}", "Patch executable", 0);
                    argsIndex = argIndex;
                }
                else if (CommandLine.TryParseArg_CopyPackages(ref args, ref argIndex, out Dictionary<string, string> copyPackagePathsO))
                {
                    copyPackagePaths = copyPackagePathsO;
                    NMessageBox.DebugShow(IntPtr.Zero, $"entry count: {copyPackagePaths.Keys.Count}\nargIndex: {argIndex}", "Copy packages", 0);
                    argsIndex = argIndex;
                }
                else
                {
                    string arg = CommandLine.GetArgFromCLI(args[argIndex]);

                    if (arg == CommandLine.OPT_NO_GUI)
                    {
                        NMessageBox.DebugShow(IntPtr.Zero, "NO GUI", string.Empty, 0);

                        _showGUI = false;
                    }
                    else if (arg == CommandLine.OPT_DEBUG)
                    {
                        _showDebugInfo = true;
                    }
                    else
                        NMessageBox.DebugShow(IntPtr.Zero, $"raw: '{args[argIndex]}'\nprocessed: '{arg}'", $"{nameof(args)}[{argIndex}]", 0);
                }
            }


            if ((patchExeSrcPath != null) && (patchExeDestPath != null))
            {
                try
                {
                    Patcher.PatchExecutable(patchExeSrcPath, patchExeDestPath);
                }
                catch (Exception ex)
                {
                    exs.Add(ex);
                    _exitCode -= 10;
                }
            }

            if (copyPackagePaths != null)
            {
                try
                {
                    Patcher.CopyPackages(copyPackagePaths);

                }
                catch (Exception ex)
                {
                    //TODO: Record exception
                    exs.Add(ex);
                    _exitCode -= 100;
                }
            }
            exceptions = exs;
            return exs.Count <= 0;
        }

        public static bool TryParseArg_PatchExecutable(ref string[] args, ref int optIndex, out string srcPath, out string destPath)
        {
            srcPath = null;
            destPath = null;
            if (!GetArgFromCLI(args[optIndex]).Equals(OPT_PATCH_EXE, StringComparison.OrdinalIgnoreCase))
                return false;

            if ((optIndex + 2) >= args.Length)
                return false;

            srcPath = GetArgFromCLI(args[optIndex + 1]);
            destPath = GetArgFromCLI(args[optIndex + 2]);
            if ((srcPath != null) && (destPath != null))
            {
                optIndex += 2;
                return true;
            }
            return false;
            //int index = optIndex + 1;
        }

        public static List<string> PrepareArg_PatchExecutable(string srcPath, string destPath)
        {
            return new List<string>()
            {
                OPT_PATCH_EXE,
                PrepareArgForCLI(srcPath),
                PrepareArgForCLI(destPath),
            };
        }



        public static bool TryParseArg_CopyPackages(ref string[] args, ref int optIndex, out Dictionary<string, string> paths)
        {
            paths = null;
            if (!GetArgFromCLI(args[optIndex]).Equals(OPT_COPY_PACKAGES, StringComparison.OrdinalIgnoreCase))
                return false;

            int index = optIndex + 1;

            if (index >= args.Length)
                return false;

            if (!int.TryParse(GetArgFromCLI(args[index]), out int copyCount))
                return false;

            index++;
            int pathArgsCount = copyCount * 2;
            if ((index + pathArgsCount) >= args.Length)
                return false;

            paths = new Dictionary<string, string>();

            int addToOptIndex = 0;
            for (int i = 0; i < pathArgsCount; i += 2)
            {
                string srcArg = GetArgFromCLI(args[index + i]);
                string destArg = GetArgFromCLI(args[index + i + 1]);
                paths.Add(srcArg, destArg);
                addToOptIndex = i + 1;
            }
            optIndex += pathArgsCount + 1;
            return true;
        }

        public static List<string> PrepareArg_CopyPackages(Dictionary<string, string> paths)
        {
            List<string> args = new List<string>();
            args.Add(OPT_COPY_PACKAGES);
            args.Add(paths.Keys.Count.ToString());
            foreach (var pair in paths)
            {
                args.Add(PrepareArgForCLI(pair.Key));
                args.Add(PrepareArgForCLI(pair.Value));
            }
            return args;
        }


        static string EscapeArguments(params string[] args)
        {
            //http://csharptest.net/529/how-to-correctly-escape-command-line-arguments-in-c/index.html
            StringBuilder arguments = new StringBuilder();
            Regex invalidChar = new Regex("[\x00\x0a\x0d]");//  these can not be escaped
            Regex needsQuotes = new Regex(@"\s|""");//          contains whitespace or two quote characters
            Regex escapeQuote = new Regex(@"(\\*)(""|$)");//    one or more '\' followed with a quote or end of string
            for (int carg = 0; args != null && carg < args.Length; carg++)
            {
                if (args[carg] == null)
                    throw new ArgumentNullException("args[" + carg + "]");
                if (invalidChar.IsMatch(args[carg]))
                    throw new ArgumentOutOfRangeException("args[" + carg + "]");
                if (args[carg] == String.Empty)
                    arguments.Append("\"\"");
                else if (!needsQuotes.IsMatch(args[carg]))
                    arguments.Append(args[carg]);
                else
                {
                    arguments.Append('"');
                    arguments.Append(escapeQuote.Replace(args[carg], m =>
                    m.Groups[1].Value + m.Groups[1].Value +
                        (m.Groups[2].Value == "\"" ? "\\\"" : "")
                    ));
                    arguments.Append('"');
                }
                if (carg + 1 < args.Length)
                    arguments.Append(' ');
            }
            return arguments.ToString();
        }
        public static string PrepareArgsForCLI(IEnumerable<string> args, bool ensureArgsEscaped = true)
        {
            if (ensureArgsEscaped)
                return EscapeArguments(args.ToArray());


            string retArgs = string.Empty;
            int argCount = args.Count();
            if (argCount <= 0)
                return retArgs;

            string firstArg = args.First();
            if (ensureArgsEscaped)
                firstArg = PrepareArgForCLI(firstArg);
            retArgs += firstArg;

            var afterArgs = args.Skip(1);
            foreach (string arg in afterArgs)
            {
                string prepArg = arg;
                if (ensureArgsEscaped)
                    prepArg = PrepareArgForCLI(prepArg);
                retArgs += " " + prepArg;
            }

            return retArgs;
        }


        static readonly List<char> INVALID_CLI_CHARS = null;

        static CommandLine()
        {
            List<char> chars = new List<char>() { ' ', '/', '\\', ':', '*', '\r', '\n' };
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                if (!chars.Contains(c))
                    chars.Add(c);
            }

            INVALID_CLI_CHARS = chars;
        }


        private CommandLine()
        {

        }

        public static string PrepareArgForCLI(string arg)
        {
            return EscapeArguments(arg);
            
            string retArg = arg;

            if (INVALID_CLI_CHARS.Any(x => retArg.Contains(x)))
            {
                if (!retArg.StartsWith(QUOT))
                    retArg = QUOT + retArg;

                if (!retArg.EndsWith(QUOT))
                    retArg = retArg + QUOT;
            }

            return retArg;
        }

        public static string GetArgFromCLI(string arg)
        {
            string retArg = arg;
            while (retArg.StartsWith(QUOT))
            {
                retArg = retArg.Substring(1);
            }
            while (retArg.EndsWith(QUOT))
            {
                retArg = retArg.Substring(0, retArg.Length - 2);
            }
            return retArg;
        }
    }
}
