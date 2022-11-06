using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ReCap.Builder
{
    public enum PublishTarget
    {
		Windows,
		Linux,
		MacOS
	}
	
	
	public class PublishParams
	{
		public PublishTarget? Target = null;
		public bool SelfContained = true;
		public bool SingleFile = false;
		public bool EnableTrimming = false;
		public string Configuration = null;
		public string ExtraArgs = null;

		public PublishParams()
		{

		}

		public static bool TryGetFromCLI(string[] args, out PublishParams param, out Exception exception)
		{
			exception = null;
			try
			{
				param = new PublishParams();
				int argCount = args.Length;
				int extraOptionsStart = -1;
				//string extraOptionsArg = args.FirstOrDefault(x => )
				//if ()
				for (int i = 0; i < argCount; i++)
				{
					int remainingArgs = argCount - (i + 1);
					string arg = args[i];
					if (PublishArgs.UnEscape(arg) == PublishArgs.EXTRA_OPTIONS)
					{
						extraOptionsStart = i;
						break;
					}
					else if (remainingArgs > 0)
					{
						string nextArg = PublishArgs.UnEscape(args[i + 1]);
						if (string.IsNullOrEmpty(nextArg) || string.IsNullOrWhiteSpace(nextArg))
							continue;
						
						if (PublishArgs.IsArg(arg, PublishArgs.PUB_TARGET))
						{
							if (Enum.TryParse<PublishTarget>(nextArg, out PublishTarget target))
							{
								param.Target = target;
							}
							else
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid publish target. Valid options are as follows:{PublishArgs.PLATFORM_VALUES}.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.SELF_CONTAINED))
						{
							if (!bool.TryParse(nextArg, out param.SelfContained))
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid boolean.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.TRIM))
						{
							if (!bool.TryParse(nextArg, out param.EnableTrimming))
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid boolean.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.SINGLE_FILE))
						{
							if (!bool.TryParse(nextArg, out param.SingleFile))
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid boolean.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.CONFIGURATION))
						{
							param.Configuration = nextArg;
						}
						i++;
					}
				}
				if (extraOptionsStart >= 0)
				{
					string dotnetExtraArgs = null;
					var extraArgs = args.Skip(extraOptionsStart).ToArray();
					if (extraArgs.Length > 0)
					{
						var first = extraArgs.First();
						if (PublishArgs.UnEscape(first) != PublishArgs.EXTRA_OPTIONS)
						{
							dotnetExtraArgs = first;
							for (int i = 1; i < extraArgs.Length; i++)
							{
								string extraArg = extraArgs[i];
								dotnetExtraArgs += $" {extraArg}";
								/*if (extraArg == PublishArgs.EXTRA_OPTIONS)
								{
									extraArgs = extraArgs.Skip(i).ToArray();
									break;
								}
								else
								{
									dotnetExtraArgs
								}*/
							}
						}
					}

					if (dotnetExtraArgs != null)
						param.ExtraArgs = dotnetExtraArgs;
				}

				//PublishHub(param);
				return true;
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			
			param = null;
			return false;
		}

		public static bool TryGetParamsForMode(string mode, PublishTarget? target, out PublishParams param)
		{
			if (string.IsNullOrEmpty(mode) || string.IsNullOrWhiteSpace(mode))
			{
				goto invalid;
			}

			param = new PublishParams()
			{
				ExtraArgs = string.Empty
			};
			bool localOnly = false;
			
			if (mode.StartsWith(PublishArgs.MODE_DEV_PREFIX))
			{
				mode = mode.Substring(PublishArgs.MODE_DEV_PREFIX.Length);
				
				if (target != null)
					param.SelfContained = true;
				/*else
					param.SelfContained = false;*/
				
				if (mode == PublishArgs.MODE_DEV_DEMO_SUFFIX)
				{
					localOnly = true;
				}
				else
					goto invalid;
			}
			else if (mode.StartsWith(PublishArgs.MODE_PUBLIC_PREFIX))
			{
				param.Configuration = "Release";
				param.SelfContained = true;
				param.SingleFile = true;
				param.EnableTrimming = true;


				mode = mode.Substring(PublishArgs.MODE_PUBLIC_PREFIX.Length);
				if (mode == PublishArgs.MODE_PUBLIC_LOCAL_SUFFIX)
				{
					localOnly = true;
				}
				else if (mode == PublishArgs.MODE_PUBLIC_ONLINE_SUFFIX)
				{
					Console.WriteLine("Online-phase builds are not yet a thing, sorry.");
					Program.Exit(ExitReason.Failure);
				}
				else
					goto invalid;
			}
			else
				goto invalid;


			
			param.ExtraArgs += $" -p:LOCAL_ONLY={localOnly}";
			
			if (string.IsNullOrEmpty(param.ExtraArgs) || string.IsNullOrWhiteSpace(param.ExtraArgs))
				param.ExtraArgs = null;
			else
				param.ExtraArgs = param.ExtraArgs.TrimStart(' ');
			

			return true;


			invalid:
				param = null;
				return false;
		}
	}
}
