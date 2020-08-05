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
	public partial class WvpFieldAutonumber : WvpFieldBase, IDisposable
	{

		#region << Parameters >>
		//[Parameter] public string Pattern { get; set; } = "";
		//[Parameter] public object Step { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private DotNetObjectReference<WvpFieldAutonumber> _objectReference;

		//private List<KeyValuePair<string, string>> _dropdownOptions = new List<KeyValuePair<string, string>>();

		private bool _editEnabled = false;

		private bool _focusOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private decimal? _value = null;

		private string _stringValue = "";

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
			_objectReference = DotNetObjectReference.Create(this);
			await base.OnAfterRenderAsync(firstRender);
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsDecimal(Value);

				if (string.IsNullOrEmpty(Pattern))
					_stringValue = _value.ToString();
				else
					_stringValue = string.Format(Pattern, _value);
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-autonumber-" + FieldId;
			}

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		//private void _onInputChangeKeypressHandler(ChangeEventArgs e)
		//{
		//	_value = FieldValueService.InitAsDecimal(e.Value);
		//	if (Mode == WvpFieldMode.Form)
		//	{
		//		ValueChanged.InvokeAsync(e);
		//	}
		//}

		//private void _onKeypressHandler(KeyboardEventArgs e)
		//{
		//	if (e.Key == "Enter")
		//	{
		//		_toggleInlineEditClickHandler(false, true);
		//	}
		//}

		//private void _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		//{

		//	//Show Edit
		//	if (enableEdit && !_editEnabled)
		//	{
		//		_focusOnRender = true;
		//		_editEnabled = true;
		//		new JsService(JSRuntime).AddDocumentEventListener(WpvDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
		//		new JsService(JSRuntime).AddDocumentEventListener(WpvDomEventType.KeydownEnter, _objectReference, FieldId.ToString(), "OnEnterKey");
		//		//StateHasChanged();
		//	}

		//	//Hide edit
		//	if (!enableEdit && _editEnabled)
		//	{
		//		var originalValue = FieldValueService.InitAsDecimal(Value);
		//		//Apply Change
		//		if (applyChange)
		//		{
		//			if (_value != originalValue)
		//			{
		//				//Update Function should be called
		//				ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
		//			}
		//		}
		//		//Abandon change
		//		else
		//		{
		//			_value = originalValue;
		//		}
		//		_editEnabled = false;
		//		new JsService(JSRuntime).RemoveDocumentEventListener(WpvDomEventType.KeydownEscape, FieldId.ToString());
		//		new JsService(JSRuntime).RemoveDocumentEventListener(WpvDomEventType.KeydownEnter, FieldId.ToString());
		//		//StateHasChanged();
		//	}

		//}
		#endregion

		#region << JS Callbacks methods >>
		//[JSInvokable]
		//public async Task OnEnterKey()
		//{
		//	if (_editEnabled)
		//	{
		//		_toggleInlineEditClickHandler(false, true);
		//		StateHasChanged();
		//	}
		//}

		//[JSInvokable]
		//public async Task OnEscapeKey()
		//{
		//	if (_editEnabled)
		//	{
		//		_toggleInlineEditClickHandler(false, false);
		//		StateHasChanged();
		//	}
		//}
		#endregion
	}
}