using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;

namespace WebVella.Pulsar.Components
{
	public partial class WvpDropdownDivider : WvpBase
	{

		#region << Parameters >>
		[CascadingParameter] private WvpDropdown Parent { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>
		//All names should start with _
		#endregion

		#region << Lifecycle methods >>
		protected override void OnInitialized()
		{
			if (Parent == null)
				throw new ArgumentNullException(nameof(Parent), "WvpDropdownDivider must exist within a WvpDropdown");

			base.OnInitialized();
		}
		#endregion

		#region << Private methods >>
		//All names should start with _

		#endregion

		#region << Ui handlers >>
		//All names should start with _

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}