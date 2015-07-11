using System;
using System.Diagnostics;

namespace Claw.Validation
{
	/// <summary>
	/// A report printing messages to the Trace. This is useful for debug only.
	/// </summary>
	public class DebugReport : ValidationReport
	{
		public override void AddInfo(string message)
		{
			Trace.WriteLine("[DEBUG][VALIDATION][INFO]: " + message);
		}
		
		public override void AddWarning(string message)
		{
            Trace.WriteLine("[DEBUG][VALIDATION][WARNING]: " + message);
		}
		
		public override void AddError(string message)
		{
			Trace.WriteLine("[DEBUG][VALIDATION][ERROR]: " + message);
		}
	}
}
