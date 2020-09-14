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
using System.Linq;
using System.Diagnostics;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputFreeMultiSelect : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public List<string> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// Current Autocomplete value
		/// </summary>
		[Parameter] public List<string> Value { get; set; } = new List<string>();

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private bool _isDataTouched = true;

		private List<string> _originOptions = new List<string>();

		private List<string> _options = new List<string>();

		private string _filter = "";

		private List<string> _originalValue = new List<string>();

		private List<string> _value = new List<string>();

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();

			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"input-group-{sizeSuffix}");

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = Value.ToList();
			}

			if (JsonConvert.SerializeObject(_originOptions) != JsonConvert.SerializeObject(Options))
			{
				_originOptions = Options;
				_options = Options;
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

		private async Task _onFilterInputHandler(ChangeEventArgs e)
		{
			_filter = e.Value?.ToString();
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onValueChangeHandler(ChangeEventArgs e)
		{
			var stringValue = (string)e.Value;
			if (!String.IsNullOrWhiteSpace(stringValue) && !_value.Contains(stringValue))
			{
				_value.Add(stringValue);
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			}

			await Task.Delay(5);
			_filter = "";
			await InvokeAsync(StateHasChanged);
		}

		private async Task _removeValue(string option)
		{
			_value.Remove(option);
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await InvokeAsync(StateHasChanged);
		}



		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}