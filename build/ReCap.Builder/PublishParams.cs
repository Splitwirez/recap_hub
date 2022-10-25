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
	}
}
