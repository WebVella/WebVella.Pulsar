using Microsoft.AspNetCore.Components;
using System;
using System.Timers;
using System.Diagnostics;
using WebVella.Pulsar.Models;
using Microsoft.AspNetCore.Components.Web;

namespace WebVella.Pulsar.Components
{
	public partial class WvpModalHeader : WvpBase
	{
		#region << Parameters >>
		[Parameter] public EventCallback<MouseEventArgs> OnCloseClick { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public string HeadingClass { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>
		#endregion

		#region << Lifecycle methods >>

		#endregion

		#region << Private methods >>

	
		#endregion

		#region << UI Handlers >>

		#endregion

		#region << JS Callbacks methods >>
		#endregion
	}
}