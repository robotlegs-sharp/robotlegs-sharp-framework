namespace robotlegs.bender.extensions.commandCenter.api
{
	public interface ICommandTrigger
	{
		/// <summary>
		/// Invoked when the trigger should be activated.
		/// 
		/// <p>Use this to add event listeners or Signal handlers.</p>
		/// </summary>
		void Activate();

		/// <summary>
		/// Invoked when the trigger should be deactivated.
		/// 
		/// <p>Use this to remove event listeners or Signal handlers.</p>
		/// </summary>
		void Deactivate();
	}
}