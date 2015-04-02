using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Viseo.WiiWars.WiimoteInSpace
{
	public abstract class NotifierBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "[CallerMemberName] needs default parameter.")]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
