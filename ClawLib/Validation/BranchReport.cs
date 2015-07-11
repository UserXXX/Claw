using System;
using System.Collections.Generic;

namespace Claw.Validation
{
	/// <summary>
	/// A validation report branching into multiple reports.
	/// </summary>
	public class BranchReport : ValidationReport
	{
		private readonly IEnumerable<ValidationReport> reports;
		
		public BranchReport(IEnumerable<ValidationReport> reports)
		{
			this.reports = reports;
		}
		
		public override void AddInfo(string message)
		{
			foreach (var report in reports)
			{
				report.AddInfo(message);
			}
		}
		
		public override void AddWarning(string message)
		{
			foreach (var report in reports)
			{
				report.AddWarning(message);
			}
		}
		
		public override void AddError(string message)
		{
			foreach (var report in reports)
			{
				report.AddError(message);
			}
		}
	}
}
