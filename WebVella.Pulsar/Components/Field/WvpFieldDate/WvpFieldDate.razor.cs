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

//All Dates are expected in UTC, 
//Timezone has no influence on the date
//All Result Dates will be returned as UTC

namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldDate : WvpFieldBase, IDisposable
	{

		#region << Parameters >>

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private DotNetObjectReference<WvpFieldDate> _objectReference;

		private bool _editEnabled = false;

		private bool _focusOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private DateTime? _value = null;
		private string _valueAsString = null;

		private DateTime? _minValue = null;
		private DateTime? _maxValue = null;


		private int? _step = null;

		private Dictionary<string, object> _extraAttributes = new Dictionary<string, object>();

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
				_value = FieldValueService.InitAsDateTime(Value);
				_value = _value?.WvpClearKind(); //We should deal with the date as it is
				_valueAsString = _value?.ToShortDateString();
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-date-" + FieldId;
			}
			_minValue = null;
			if (Min != null)
			{
				_minValue = FieldValueService.InitAsDateTime(Min);
			}
			if (_minValue != null)
				_extraAttributes["min"] = _minValue?.ToString("yyyy-MM-dd");//We should deal with the date as it is

			_maxValue = null;
			if (Max != null)
			{
				_maxValue = FieldValueService.InitAsDateTime(Max);
			}
			if (_maxValue != null)
				_extraAttributes["max"] = _maxValue?.ToString("yyyy-MM-dd");//We should deal with the date as it is

			_step = null;
			if (Step != null)
			{
				_step = FieldValueService.InitAsInt(Step);
			}
			if (_step != null)
				_extraAttributes["step"] = _step;


			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		private void _onInputChangeKeypressHandler(ChangeEventArgs e)
		{
			_value = FieldValueService.InitAsDateTime(e.Value);
			if (Mode == WvpFieldMode.Form)
			{
				//Date Should be converted back to UTC
				if (_value != null)
				{
					_value = DateTime.SpecifyKind(_value.Value, DateTimeKind.Utc);
				}
				e.Value = _value;
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
				var originalValue = FieldValueService.InitAsDateTime(Value);
				//Apply Change
				if (applyChange)
				{
					if (_value != originalValue)
					{
						//Date Should be converted back to UTC
						if (_value != null)
						{
							_value = DateTime.SpecifyKind(_value.Value, DateTimeKind.Utc);
						}
						_valueAsString = _value?.ToShortDateString();
						ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
					_value = _value?.WvpClearKind(); //Convert the date to timezone and clear the kind
					_valueAsString = _value?.ToShortDateString();
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