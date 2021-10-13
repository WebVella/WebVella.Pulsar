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
	public partial class WvpDropdownItem : WvpBase
	{

		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		
		[CascadingParameter] private WvpDropdown Parent { get; set; }

		/// <summary>
		/// Will be passed back on OnClick
		/// </summary>
		[Parameter] public object Value { get; set; }
		[Parameter] public bool Disabled { get; set; } = false;

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
				throw new Exception("WvpDropdownItem must exist within a WvpDropdown");

			base.OnInitialized();
		}

		protected override async Task OnParametersSetAsync()
		{
			var classList = new List<string> { "dropdown-item wvp-dropdown-item", Class };

			_class = String.Join(" ", classList);

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>
		//All names should start with _

		#endregion

		#region << Ui handlers >>
		private async Task _onClickHandler(MouseEventArgs e){
			await OnClick.InvokeAsync(Value);
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}