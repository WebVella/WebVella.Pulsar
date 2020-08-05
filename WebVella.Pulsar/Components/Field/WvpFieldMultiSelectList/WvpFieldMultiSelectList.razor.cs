using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;


namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldMultiSelectList : WvpFieldBase, IDisposable 
	{

		#region << Parameters >>

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";
		private DotNetObjectReference<WvpFieldMultiSelectList> _objectReference;
		private bool _editEnabled = false;

		private List<string> _value = new List<string>();

		private List<WvpSelectOption> _filteredOptions = new List<WvpSelectOption>();

		private Guid _originalFieldId;

		private object _originalValue;

		private string _selectedValue = "";

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domElementId}", FieldId.ToString());
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}

		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsListString(Value);

				_filteredOptions = Options.FindAll(x => !_value.Contains(x.Value)).ToList();
				if (_filteredOptions.Count > 0)
					_selectedValue = _filteredOptions[0].Value;
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-multiselect-list-" + FieldId;
			}

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI handlers >>
		//private void _onInputEvent(ChangeEventArgs e)
		//{
		//	_value = FieldValueService.InitAsListString(e.Value);
		//	if (Mode == WvpFieldMode.Form)
		//	{
		//		ValueChanged.InvokeAsync(e);
		//	}
		//}

		private void _addValueClickHandler()
		{
			if (String.IsNullOrWhiteSpace(_selectedValue))
				throw new Exception("Selected value cannot be null");

			_value.Add(_selectedValue);

			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			}
			if (Mode == WvpFieldMode.InlineEdit)
			{
				_filteredOptions = Options.FindAll(x => !_value.Contains(x.Value)).ToList();
				if (_filteredOptions.Count > 0)
					_selectedValue = _filteredOptions[0].Value;
			}
		}

		private void _removeValueClickHandler(string value)
		{
			if (String.IsNullOrWhiteSpace(value))
				throw new Exception("Value cannot be null");

			_value.Remove(value);
			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			}
			if (Mode == WvpFieldMode.InlineEdit)
			{
				_filteredOptions = Options.FindAll(x => !_value.Contains(x.Value)).ToList();
				if (_filteredOptions.Count > 0)
					_selectedValue = _filteredOptions[0].Value;
			}
		}

		private void _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{

			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
				new JsService(JSRuntime).AddOutsideClickEventListener($"#{_domElementId}", _objectReference, FieldId.ToString(), "OnFocusOutClick");
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				var originalValue = FieldValueService.InitAsListString(Value);
				//Apply Change
				if (applyChange)
				{
					if (JsonConvert.SerializeObject(_value) != JsonConvert.SerializeObject(originalValue))
					{
						//Update Function should be called
						ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
					_filteredOptions = Options.FindAll(x => !_value.Contains(x.Value)).ToList();
					if (_filteredOptions.Count > 0)
						_selectedValue = _filteredOptions[0].Value;
				}
				_editEnabled = false;
				new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
				new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domElementId}", FieldId.ToString());
			}

		}


		#endregion

		#region << JS Callbacks methods >>

		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, false);
				StateHasChanged();
			}
		}

		[JSInvokable]
		public async Task OnFocusOutClick()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, true);
				StateHasChanged();
			}
		}
		#endregion
	}
}