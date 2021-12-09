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
	public partial class WvpInputPassword : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Pattern { get; set; } = "";

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public string Value { get; set; } = "";

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; } //Fires on each user input
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private string _originalValue = "";

		private string _value = "";

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
				_value = FieldValueService.InitAsString(Value);
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			if (!String.IsNullOrWhiteSpace(Title))
				AdditionalAttributes["title"] = Title;

			if(!String.IsNullOrWhiteSpace(Pattern))
				AdditionalAttributes["pattern"] = Pattern;

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>
		private async Task _onKeyDownHandler(KeyboardEventArgs e)
		{
			//Needs to be keydown as keypress is produced only on printable chars (does not work on backspace
			await Task.Delay(5);
			if (e.Key == "Enter" || e.Key == "NumpadEnter" || e.Key == "Tab")
			{
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			}

			await OnKeyDown.InvokeAsync(e);
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onBlurHandler()
		{
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			//await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}