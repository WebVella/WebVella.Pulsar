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
using System.Diagnostics;
using System.Globalization;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInlineCurrency : WvpInlineBase, IAsyncDisposable
	{

		#region << Parameters >>

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public decimal? Value { get; set; } = null;

		[Parameter] public double? Max { get; set; } = null;

		[Parameter] public double? Min { get; set; } = null;

		[Parameter] public double Step { get; set; } = 0.01;

		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private string _inputElementId = "wvp-" + Guid.NewGuid();

		private DotNetObjectReference<WvpInlineCurrency> _objectReference;

		private int _decimalPlaces = 0;

		private decimal? _originalValue = null;

		private decimal? _value = null;

		private bool? scheduledEnableEditChange = null;

		private bool? scheduledApplyChange = null;



		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, Id, "OnEscapeKey");
			}
			await base.OnAfterRenderAsync(firstRender);
		}

		public async ValueTask DisposeAsync()
		{
			await new  JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);

			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnInitializedAsync()
		{
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"input-group-{sizeSuffix}");

			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsDecimal(Value);
			}

			_decimalPlaces = 0;
			if(Step.ToString(CultureInfo.InvariantCulture).IndexOf(".") > -1){
				_decimalPlaces = Step.ToString(CultureInfo.InvariantCulture).Substring(Step.ToString(CultureInfo.InvariantCulture).IndexOf(".") + 1).Length;
			}

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>
		private async Task _onKeyDownHandler(KeyboardEventArgs e)
		{
			scheduledEnableEditChange = null;
			scheduledApplyChange = null;

			//Needs to be keydown as keypress is produced only on printable chars (does not work on backspace
			if (e.Key == "Enter" || e.Key == "NumpadEnter" || e.Key == "Tab")
			{
				scheduledEnableEditChange = false;
				scheduledApplyChange = true;
			}
			if (e.Key == "Escape")
			{
				scheduledEnableEditChange = false;
				scheduledApplyChange = false;
			}
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onInputHandler(ChangeEventArgs e)
		{
			//On Input will be executed after Keydown so we need to apply any scheduled changes here
			if (scheduledEnableEditChange != null && scheduledApplyChange != null)
			{
				await _toggleInlineEditClickHandler(scheduledEnableEditChange.Value, scheduledApplyChange.Value);
			}
			else
			{
				//Apply value only if a change is not scheduled
				_value = (decimal?)e.Value;
			}
			await InvokeAsync(StateHasChanged);
		}

		private async Task _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{
			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				await Task.Delay(5);
				await new  JsService(JSRuntime).FocusElementBySelector("#" + _inputElementId);
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				var originalValue = FieldValueService.InitAsDecimal(Value);
				//Apply Change
				if (applyChange)
				{
					if (_value != originalValue)
					{
						//Update Function should be called
						await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
				}
				_editEnabled = false;
			}
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnEscapeKey()
		{
			await Task.Delay(0);
			if (_editEnabled)
			{
				scheduledEnableEditChange = false;
				scheduledApplyChange = false;
				//OnInput will be called after this because of the blur and it will execute the toggle
			}
		}
		#endregion
	}
}