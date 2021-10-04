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
	public partial class WvpInlineDate : WvpInlineBase, IAsyncDisposable
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// DateTime in UTC format
		/// </summary>
		[Parameter] public DateTime? Value { get; set; } = null;

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private DotNetObjectReference<WvpInlineDate> _objectReference;

		private List<string> _inputGroupList = new List<string>();

		private DateTime? _originalValue = null;

		private DateTime? _value = null;

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
			await new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);

			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}


		protected override async Task OnInitializedAsync()
		{
			var sizeSuffix = Size.ToDescriptionString();
			if (Required)
			{
				if (!String.IsNullOrWhiteSpace(Class))
					_cssList.Add(Class);

				if (!String.IsNullOrWhiteSpace(sizeSuffix))
					_cssList.Add($"form-control-{sizeSuffix}");
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(Class))
					_inputGroupList.Add(Class);

				if (!String.IsNullOrWhiteSpace(sizeSuffix))
					_inputGroupList.Add($"input-group-{sizeSuffix}");
			}

			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = Value;
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
				_value = (DateTime?)e.Value;
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
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				//Apply Change
				if (applyChange)
				{
					if (_value != Value)
					{
						//Update Function should be called
						await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = _originalValue;
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
				await _toggleInlineEditClickHandler(false, false);
				StateHasChanged();
			}
		}
		#endregion
	}
}