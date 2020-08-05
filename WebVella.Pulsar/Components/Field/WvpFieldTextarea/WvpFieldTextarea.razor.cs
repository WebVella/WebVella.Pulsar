using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	public partial class WvpFieldTextarea : WvpFieldBase, IDisposable
	{

		#region << Parameters >>

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";
		private DotNetObjectReference<WvpFieldTextarea> _objectReference;
		private bool _editEnabled = false;

		private bool _focusOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private string _value = "";

		public List<KeyValuePair<string, string>> _validationErrors = new List<KeyValuePair<string, string>>();

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
				_value = FieldValueService.InitAsString(Value);
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				if(!String.IsNullOrWhiteSpace(Id))
					_domElementId = Id;
				else
					_domElementId = "wvp-field-text-" + FieldId;
			}
			if(ValidationErrors.Count > 0)
				_validationErrors = ValidationErrors.FindAll(x=> x.Key.ToLowerInvariant() == Name.ToLowerInvariant());


			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		private void _onInputChangeKeypressHandler(ChangeEventArgs e)
		{
			_value = FieldValueService.InitAsString(e.Value);
			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(e);
			}
		}

		private void _onKeypressHandler(KeyboardEventArgs e)
		{
			if (e.Key == "Enter")
			{
				_toggleInlineEditClickHandler(false, true);
			}
		}

		//private void _onKeyUpEvent()
		//{
		//	if (Mode == FieldMode.Form)
		//	{
		//		ValueChanged.InvokeAsync(_value);
		//	}
		//	else
		//	{
		//		_dropdownOptions = new List<KeyValuePair<string, string>>();
		//		if (_value.Length > 2)
		//		{
		//			for (int i = 0; i < 5; i++)
		//			{
		//				_dropdownOptions.Add(new KeyValuePair<string, string>(i.ToString(), _value));
		//			}
		//		}
		//		//StateHasChanged();
		//	}
		//}

		//private void OnFocusOutEvent()
		//{
		//	if (Mode == FieldMode.Form)
		//		ValueChanged.InvokeAsync(_value);
		//}

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
				var originalValue = FieldValueService.InitAsString(Value);
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