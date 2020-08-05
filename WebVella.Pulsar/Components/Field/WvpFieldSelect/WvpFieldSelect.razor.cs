using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class WvpFieldSelect : WvpFieldBase, IDisposable
	{

		#region << Parameters >>

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private Dictionary<string,object> _attributes = new Dictionary<string, object>();
		private DotNetObjectReference<WvpFieldSelect> _objectReference;
		private string _domElementId = "";

		private bool _focusOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private bool _editEnabled = false;

		private string _value = "";

		private List<WvpSelectOption> _options = new List<WvpSelectOption>();

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEnter, FieldId.ToString());
			new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domElementId}", FieldId.ToString());
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
					_domElementId = "wvp-field-select-" + FieldId;
			}
			_attributes = new Dictionary<string, object>();

			if(IsDisabled)
				_attributes["disabled"] = "disabled";

			if(Options.Count > TypeaheadSearchEnabledItemCount)
				_options = Options.Take(TypeaheadSearchEnabledItemCount).ToList();
			else
				_options = Options;

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		private void _onInputEvent(ChangeEventArgs e)
		{
			_value = e.Value as string;
			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(e);
			}

			StateHasChanged();
		}

		private void _filterOptionsHandler(ChangeEventArgs e)
		{
			_options = Options.Where(o => o.Label.ToLowerInvariant().Contains(((string)e.Value).ToLowerInvariant())).ToList();
			
			if(_options.Count > TypeaheadSearchEnabledItemCount)
				_options = _options.Take(TypeaheadSearchEnabledItemCount).ToList();


			StateHasChanged();
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
				new JsService(JSRuntime).AddOutsideClickEventListener($"#{_domElementId}", _objectReference, FieldId.ToString(), "OnFocusOutClick");

				//StateHasChanged();
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
				new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domElementId}", FieldId.ToString());
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