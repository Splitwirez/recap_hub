using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ReCap.Hub.Data
{
    public static class Patcher
    {
        public const string AUTO_LOGIN_PACKAGE_NAME = "0ReCapAutoLogin.package";
        static readonly byte[] SEQ_REPLACE =
        {
            0x80,
            0xBF,
            0x2C,
            0x01,
            0x00,
            0x00,
            0x00,
            0x75
        };

        static readonly byte[] SEQ_REPLACE_WITH =
        {
            0x80,
            0xBF,
            0x2C,
            0x01,
            0x00,
            0x00,
            0x01,
            0x75
        };

        static readonly List<string> SEQ_REPLACE_WITH_LOCALHOOOOOST = new List<string>()
        {
            "config.darkspore.com",
            "gosredirector.ea.com",
            "gosredirector.scert.ea.com",
            "gosredirector.stest.ea.com",
            "gosredirector.online.ea.com",
            "api.darkspore.com",
        };

        static readonly Dictionary<byte[], byte[]> SEQ = new Dictionary<byte[], byte[]>()
        {
            {
                SEQ_REPLACE,
                SEQ_REPLACE_WITH
            }
        };

        static Patcher()
        {
            foreach (string replace in SEQ_REPLACE_WITH_LOCALHOOOOOST)
            {
                byte[] seq = StringToBytes(replace);
                SEQ.Add(seq, GetLocalhooooost(seq));
            }
        }

        static byte[] StringToBytes(string str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
            /*List<byte> ret = new List<byte>();
            foreach (char c in str)
            {
                Convert.ToByte(c)
                char.
                BitConverter.GetBytes(c)
            }*/
        }


        public static int PatchGame(bool exeMissing, string exeSrcPath, string exeDestPath, bool autoLoginPackageMissing, string autoLoginPackageDestPath)
        {
            string autoLoginPackageSrcPath = Path.Combine(HubData.Instance.CfgPath, AUTO_LOGIN_PACKAGE_NAME);
            Dictionary<string, string> autoLoginPackageDict = null;
            if (autoLoginPackageMissing)
            {
                autoLoginPackageDict = new Dictionary<string, string>()
                {
                    {
                        autoLoginPackageSrcPath,
                        autoLoginPackageDestPath
                    }
                };

                if (!File.Exists(autoLoginPackageSrcPath))
                {
                    LocalServer.ExtractResource(LocalServer.AUTO_LOGIN_PACKAGE_RESNAME, autoLoginPackageSrcPath);
                }
            }

            try
            {
                if (exeMissing)
                    PatchExecutable(exeSrcPath, exeDestPath);
                
                if (autoLoginPackageMissing)
                    CopyPackages(autoLoginPackageDict);
                
                return 0;
            }
            catch
            {
                if (!Permissions.IsAdministrator())
                {
                    var argsList = new List<string>();
                        //$"\"{srcPath}\" \"{destPath}\""
                        //string.Empty
                    if (exeMissing)
                    {
                        var exeArgs = CommandLine.PrepareArg_PatchExecutable(exeSrcPath, exeDestPath);
                        foreach (string arg in exeArgs)
                        {
                            argsList.Add(arg);
                        }
                        //CommandLine.PrepareArgsForCLI(CommandLine.PrepareArg_PatchExecutable(srcPath, destPath).Append(CommandLine.OPT_NO_GUI).ToList())
                    }
                    
                    if (autoLoginPackageMissing)
                    {
                        var autoLoginPackageArgs = CommandLine.PrepareArg_CopyPackages(autoLoginPackageDict);
                        foreach (string arg in autoLoginPackageArgs)
                        {
                            argsList.Add(arg);
                        }
                    }
                    if (argsList.Count > 0)
                    {
                        argsList.Insert(0, CommandLine.OPT_NO_GUI);
                    
                        string args = CommandLine.PrepareArgsForCLI(argsList);
                        var process = Permissions.RerunAsAdministrator(args);
                        process.WaitForExit();
                        return process.ExitCode;
                    }
                }
            }
            return -1;
        }


        public static void PatchExecutable(string srcPath, string destPath)
        {
            byte[] exe = File.ReadAllBytes(srcPath);
            File.WriteAllBytes(destPath, exe);
            File.Delete(destPath);

            var searchLength = exe.LongLength; //- SEQ_REPLACE_WITH.Length;

            for (long i = 0; i < searchLength; i++)
            {
                foreach (var seq in SEQ)
                {
                    if (ReplaceByteSeq(ref exe, i, seq.Key, seq.Value))
                        break;
                }
                //ReplaceByteSeq(ref exe, i, SEQ_REPLACE, SEQ_REPLACE_WITH);
                //ReplaceByteSeq
            }

            string writePath =
                destPath
                //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.GetFileName(destPath))
            ;
            Debug.WriteLine($"writePath: '{writePath}'");
            File.WriteAllBytes(writePath, exe);
            Permissions.GrantAccessFile(destPath);
        }

        static bool ReplaceByteSeq(ref byte[] exe, long i, byte[] replaceWhat, byte[] replaceWith)
        {
            if ((exe.LongLength - replaceWith.Length) <= (i - 100))
            {
                Debug.WriteLine("Near the end...");
                return false;
            }

            bool retVal = false;
            int matchLengthThreshold = (replaceWhat.Length - 1);
            
            int matched = 0;
            for (int b = 0; b < replaceWhat.Length; b++)
            {
                if (replaceWhat[b] == exe[i + b])
                {
                    if (matched > 1)
                    {
                        string byteVal =
                            //BitConverter.ToString(exe[i + b])
                            exe[i + b].ToString("X2")
                            ;
                        //Debug.WriteLine($"{i}+{b}:{byteVal} == {nameof(replaceWhat)}[{b}]");
                    }
                    matched++;
                }
                else
                {
                    /*if (matched > 3)
                        Debug.WriteLine($"matched: {matched}");*/
                    break;
                }
            }

            if (matched >= matchLengthThreshold)
            {
                Debug.WriteLine($"Full sequence matched!: {matched}");
                retVal = true;
                for (int b = 0; b < replaceWith.Length; b++)
                {
                    exe[i + b] = replaceWith[b];
                }
            }

            return retVal;
        }

        static readonly byte[] LH_Localh = StringToBytes("127.0.0.");
        static readonly byte[] LH_ooooo = StringToBytes("0");
        static readonly byte[] LH_st = StringToBytes("1");

        static byte[] GetLocalhooooost(byte[] forWhat)
        {
            List<byte> localhooooost = LH_Localh.ToList();
            for (int localh = localhooooost.Count; localh < (forWhat.Length - LH_st.Length); localh += LH_ooooo.Length)
            {
                for (int o = 0; o < LH_ooooo.Length; o++)
                {
                    localhooooost.Add(LH_ooooo[o]);
                }
            }
            for (int st = 0; st < LH_ooooo.Length; st++)
            {
                localhooooost.Add(LH_st[st]);
            }
            return localhooooost.ToArray();
        }

        public static void CopyPackages(Dictionary<string, string> copyPackagePaths)
        {
            if ((copyPackagePaths == null) || (copyPackagePaths.Keys == null) || (copyPackagePaths.Keys.Count <= 0))
                return; //return false;
            
            //int totalSucceeded = 0;
            //int totalFailed = 0;
            foreach (string srcPath in copyPackagePaths.Keys)
            {
                /*try
                {*/
                    string destPath = copyPackagePaths[srcPath];
                    File.Copy(srcPath, destPath, true);
                    Permissions.GrantAccessFile(destPath);
                    //totalSucceeded++;
                /*}
                catch
                {
                    totalFailed++;
                }*/
            }
            
            //return totalSucceeded >= copyPackagePaths.Keys.Count;
            //return totalFailed;
        }
    }
}
