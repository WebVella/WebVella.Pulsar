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

namespace WebVella.Pulsar.Components
{
	public partial class WvpInlinePassword : WvpInlineBase, IDisposable
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
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private string _inputElementId = "wvp-" + Guid.NewGuid();

		private DotNetObjectReference<WvpInlinePassword> _objectReference;

		private string _originalValue = "";

		private string _value = "";

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

		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);

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
				_value = FieldValueService.InitAsString(Value);
			}

			if (!String.IsNullOrWhiteSpace(Pattern))
				AdditionalAttributes["pattern"] = Pattern;

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
				_value = e.Value?.ToString();
			}
		}

		private async Task _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{
			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				await Task.Delay(5);
				new JsService(JSRuntime).FocusElementBySelector("#" + _inputElementId);
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				var originalValue = FieldValueService.InitAsString(Value);
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