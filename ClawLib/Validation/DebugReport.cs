using System;
using System.Diagnostics;

namespace Claw.Validation
{
	/// <summary>
	/// A report printing messages to the Trace. This is useful for debug only.
	/// </summary>
	internal class DebugReport : ValidationReport
	{
		internal override void AddInfo(string message)
		{
			Trace.WriteLine("[DEBUG][VALIDATION][INFO]: " + message);
		}
		
		internal override void AddWarning(string message)
		{
			Trace.WriteLine("[DEBUG][WARNING][INFO]: " + message);
		}
		
		internal override void AddError(string message)
		{
			Trace.WriteLine("[DEBUG][ERROR][INFO]: " + message);
		}
	}
}
