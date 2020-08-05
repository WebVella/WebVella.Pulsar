using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;


namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldNumber : WvpFieldBase, IDisposable
	{

		#region << Parameters >>

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private DotNetObjectReference<WvpFieldNumber> _objectReference;
		
		private bool _editEnabled = false;

		private bool _focusOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private decimal? _value = null;

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEnter, FieldId.ToString());
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (_focusOnRender && _editEnabled)
			{
				var result = new JsService(JSRuntime).FocusElement(_domElementId);
				_focusOnRender = false;
			}
			await base.OnAfterRenderAsync(firstRender);
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsDecimal(Value);
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-number-" + FieldId;
			}

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		private void _onInputChangeKeypressHandler(ChangeEventArgs e)
		{
			string value = (string)e.Value;
			_value = FieldValueService.InitAsDecimal(value.Replace(",", "."));

			if (Mode == WvpFieldMode.Form && ValueChanged.HasDelegate)
			{
				ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
			}
		}

		private void _onKeypressHandler(KeyboardEventArgs e)
		{
			if (e.Key == "Enter")
			{
				_toggleInlineEditClickHandler(false, true);
			}
		}

		private void _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{

			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_focusOnRender = true;
				_editEnabled = true;
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEnter, _objectReference, FieldId.ToString(), "OnEnterKey");
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
						ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
				}
				_editEnabled = false;
				new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
				new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEnter, FieldId.ToString());
			}

		}
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnEnterKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, true);
				StateHasChanged();
			}
		}

		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, false);
				StateHasChanged();
			}
		}
		#endregion
	}
}