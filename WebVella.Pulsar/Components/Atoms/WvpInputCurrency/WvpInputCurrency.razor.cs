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
	public partial class WvpInputCurrency : WvpInputBase
	{

		#region << Parameters >>

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public decimal? Value { get; set; } = null;

		[Parameter] public double? Max { get; set; } = null;

		[Parameter] public double? Min { get; set; } = null;

		[Parameter] public double Step { get; set; } = 1;


		[Parameter] public WvpCurrencyType Currency { get; set; } = new WvpCurrencyType();

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; } //Fires on each user input
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private int _decimalPlaces = 0;

		private decimal? _originalValue = null;

		private decimal? _value = null;

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnInitializedAsync()
		{
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");

			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsDecimal(Value);
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			if (!String.IsNullOrWhiteSpace(Title))
				AdditionalAttributes["title"] = Title;

			if (Min != null)
				AdditionalAttributes["min"] = Min;

			if (Max != null)
				AdditionalAttributes["max"] = Max;

			AdditionalAttributes["step"] = Step;

			_decimalPlaces = 0;
			if(Step.ToString().IndexOf(".") > 0){
				_decimalPlaces = Step.ToString().Substring(Step.ToString().IndexOf(".") + 1).Length;
			}

			if(_value != null)
				_value = Math.Round(_value.Value,_decimalPlaces);

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
		}

		private async Task _onMouseDownHandler(MouseEventArgs e)
		{
			//Needs to be keydown as keypress is produced only on printable chars (does not work on backspace
			await Task.Delay(5);
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
		}

		private async Task _onBlurHandler(FocusEventArgs e)
		{
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}