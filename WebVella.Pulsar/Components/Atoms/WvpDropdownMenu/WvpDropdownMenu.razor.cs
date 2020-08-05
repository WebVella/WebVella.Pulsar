using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using WebVella.Pulsar.Services;

namespace WebVella.Pulsar.Components
{
	public partial class WvpDropdownMenu : WvpBase
	{

		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[CascadingParameter] private WvpDropdown Parent { get; set; }

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>
		private string _class = "";
		#endregion

		#region << Lifecycle methods >>

		protected override void OnInitialized()
		{
			if (Parent == null)
				throw new ArgumentNullException(nameof(Parent), "WvpDropdownMenu must exist within a WvpDropdown");
			base.OnInitialized();
		}

		protected override async Task OnParametersSetAsync()
		{
			var classList = new List<string> { "dropdown-menu  wvp-dropdown-menu", Class };
			if (Parent.StoreIsMenuVisible)
				classList.Add("show");
			if (Parent.Mode == WvpDropDownDirection.DropDownRight)
				classList.Add("dropdown-menu-right");


			_class = String.Join(" ", classList);
			await base.OnParametersSetAsync();
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