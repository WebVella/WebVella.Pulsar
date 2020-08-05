using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;


namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldCheckbox : WvpFieldBase, IDisposable
	{

		#region << Parameters >>
		[Parameter] public Dictionary<string, object> Attributes { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private DotNetObjectReference<WvpFieldCheckbox> _objectReference;

		//private List<KeyValuePair<string, string>> _dropdownOptions = new List<KeyValuePair<string, string>>();

		private bool _editEnabled = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private bool _value = false;

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

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsBool(Value);
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-checkbox-" + FieldId;
			}
			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		private void _onInputEvent(ChangeEventArgs e)
		{
			_value = FieldValueService.InitAsBool(e.Value);

			if (Mode == WvpFieldMode.Form)
				ValueChanged.InvokeAsync(e);
		}



		private void _toggleInlineEdit(bool enableEdit, bool applyChange)
		{

			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEnter, _objectReference, FieldId.ToString(), "OnEnterKey");
				//StateHasChanged();
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				var originalValue = FieldValueService.InitAsBool(Value);
				//Apply Change
				if (applyChange)
				{
					if (_value != originalValue)
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
				new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEnter, FieldId.ToString());
				//StateHasChanged();
			}

		}
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnEnterKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEdit(false, true);
				StateHasChanged();
			}
		}

		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEdit(false, false);
				StateHasChanged();
			}
		}
		#endregion
	}
}