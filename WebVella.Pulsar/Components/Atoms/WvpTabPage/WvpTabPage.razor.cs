using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;

namespace WebVella.Pulsar.Components
{
	public partial class WvpTabPage : WvpBase
	{

		#region << Parameters >>
		[CascadingParameter] private WvpTabNav Parent { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string Label { get; set; }
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
				throw new ArgumentNullException(nameof(Parent), "WvpTabPage must exist within a WvpTabNav");

			Parent.StoreAddTabPage(this);
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