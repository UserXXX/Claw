using System;

namespace Claw.Validation
{
	/// <summary>
	/// Abstract base class for validation reports.
	/// </summary>
	public abstract class ValidationReport
	{
		/// <summary>
		/// Adds an info to the validation report.
		/// </summary>
		/// <param name="message">The message to add.</param>
		internal abstract void AddInfo(string message);
		
		/// <summary>
		/// Adds a warning to the validation report.
		/// </summary>
		/// <param name="message">The message to add.</param>
		internal abstract void AddWarning(string message);
		
		/// <summary>
		/// Adds an error to the validation report.
		/// </summary>
		/// <param name="message">The message to add.</param>
		internal abstract void AddError(string message);
	}
}
