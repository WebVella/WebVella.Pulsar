using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using WebVella.Pulsar.Services;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputCheckbox : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>

		[Parameter] public bool? Value { get; set; } = false;

		[Parameter] public string Label { get; set; } = "";

		[Parameter] public RenderFragment WvpInputCheckboxLabel { get; set; }

		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private bool? _originalValue = false;

		private bool? _value = false;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();

			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsNullBool(Value);
			}

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

		private async Task _onChangeHandler(ChangeEventArgs e)
		{
			await ValueChanged.InvokeAsync(e);
			await OnInput.InvokeAsync(e);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}