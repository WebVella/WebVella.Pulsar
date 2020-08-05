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
	public partial class WvpFieldRadioList : WvpFieldBase, IDisposable 
	{

		#region << Parameters >>
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";
		private DotNetObjectReference<WvpFieldRadioList> _objectReference;
		private bool _editEnabled = false;

		private string _value = "";

		private Guid _originalFieldId;

		private object _originalValue;

		bool _allColumnsWithEmptyLabels = true;
		bool _allRowsWithEmptyLabels = true;
			
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
				if (Value != null)
				{
					if (Value is string)
						_value = (string)Value;
				}

			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-radio-list-" + FieldId;
			}

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI handlers >>
		private void _onInputEvent(ChangeEventArgs e, WvpSelectOption row)
		{
			string _row = row.Value;
			bool isChecked = ((string)e.Value)=="on";

			if (_value == null)
				_value = "";
			
			if (isChecked)
				_value = _row;

			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
			}

			StateHasChanged();
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
				var originalValue = (string)Value;
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