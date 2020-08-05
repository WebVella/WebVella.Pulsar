using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;


namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldHtml : WvpFieldBase, IDisposable
	{

		#region << Parameters >>

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domFieldElementId = ""; //CKEditor is outside of the textarea so we need this for the outside click monitor
		private DotNetObjectReference<WvpFieldHtml> _objectReference;
		private bool _editEnabled = false;

		private bool _focusOnRender = false;

		private bool _loadEditorOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private string _value = "";

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domFieldElementId}", FieldId.ToString());
			new JsService(JSRuntime).RemoveCKEditor(Id);
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if(_loadEditorOnRender){
				_loadEditorOnRender = false;
				new JsService(JSRuntime).AddCKEditor(Id,_objectReference, "en");
			}
			if(firstRender && Mode == WvpFieldMode.Form){
				new JsService(JSRuntime).AddCKEditor(Id,_objectReference, "en");
			}

			await base.OnAfterRenderAsync(firstRender);
		}

		protected override async Task OnParametersSetAsync()
		{

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsHtml(Value);
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domFieldElementId = "wvp-field-" + FieldId;
			}


			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>
		private void _showInlineEdit()
		{
			//_focusOnRender = true;
			_loadEditorOnRender = true;
			//new JsService(JSRuntime).AddCKEditor(_domElementId, "en");
			new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
			new JsService(JSRuntime).AddOutsideClickEventListener($"#{_domFieldElementId}", _objectReference, FieldId.ToString(), "OnFocusOutClick");
			_editEnabled = true;
		}

		private void _hideInlineEdit()
		{
			new JsService(JSRuntime).RemoveCKEditor(Id);
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domFieldElementId}", FieldId.ToString());
			_editEnabled = false;
		}

		private void _applyChange()
		{
			var originalValue = FieldValueService.InitAsString(_originalValue);
			if (_value != originalValue)
				ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
		}

		private void _revertChange()
		{
			var originalValue = FieldValueService.InitAsString(_originalValue);
			_value = originalValue;
		}
		#endregion

		#region << UI Handlers >>

		private void _onChangeEvent(ChangeEventArgs e)
		{

			var newValue = FieldValueService.InitAsHtml(e.Value);
			if (_value != newValue)
			{
				_value = newValue;
				if (Mode == WvpFieldMode.Form)
				{
					ValueChanged.InvokeAsync(e);
				}
				if (Mode == WvpFieldMode.InlineEdit)
				{
					_applyChange();
					_hideInlineEdit();
				}
			}
		}

		private void _showInlineEditClickHandler()
		{
			_showInlineEdit();
		}



		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnFocusOutClick()
		{
			//In the HTML Field only the no change outside click is handled. If there is a change the save will be processed with the onChangeEvent
			if (_editEnabled)
			{
				_hideInlineEdit();
				StateHasChanged();
			}
		}

		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				_revertChange();
				_hideInlineEdit();
				StateHasChanged();
			}
		}
		#endregion
	}
}