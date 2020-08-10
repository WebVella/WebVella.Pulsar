using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using Microsoft.AspNetCore.Components.Web;

namespace WebVella.Pulsar.Components
{
	public partial class WvpDropdownToggle : WvpBase
	{

		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[CascadingParameter] private WvpDropdown Parent { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>
		#endregion

		#region << Lifecycle methods >>

		protected override void OnInitialized()
		{
			if (Parent == null)
				throw new ArgumentNullException(nameof(Parent), "WvpDropdownMenu must exist within a WvpDropdown");

			base.OnInitialized();
		}
		#endregion

		#region << Private methods >>
		//All names should start with _

		#endregion

		#region << Ui handlers >>
		private void _onClickHandler(MouseEventArgs ev){
			Parent.StoreToggleMenu();
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}