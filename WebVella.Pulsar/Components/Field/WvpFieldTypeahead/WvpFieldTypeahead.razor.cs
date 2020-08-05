using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	public partial class WvpFieldTypeahead : WvpFieldBase, IDisposable
	{

		#region << Parameters >>
		[Parameter] public bool WillLoadMoreResults { get; set; }
		[Parameter] public bool ValueOnlyFromOptions { get; set; } = true;
		[Parameter] public EventCallback<ChangeEventArgs> LoadMoreResults { get; set; }
		[Parameter] public EventCallback<ChangeEventArgs> FilterChanged { get; set; }
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";
		//private DotNetObjectReference<WvpFieldTypeahead> _objectReference;
		private bool _editEnabled = false;

		private bool _focusOnRender = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private string _value = "";

		private string _prefill = "";

		private bool _onEnterRegistred = false;

		private List<string> _options = new List<string>();

		public List<KeyValuePair<string, string>> _validationErrors = new List<KeyValuePair<string, string>>();

		public int? _activeItemIndex = null;

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			//new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			//if (_objectReference != null)
			//{
			//	_objectReference.Dispose();
			//	_objectReference = null;
			//}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (_focusOnRender && _editEnabled)
			{
				var result = new JsService(JSRuntime).FocusElement(_domElementId);
				_focusOnRender = false;
			}

			if (Mode == WvpFieldMode.Form && !_onEnterRegistred)
			{
				//new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEnter, _objectReference, FieldId.ToString(), "OnEnterKey");
				_onEnterRegistred = true;
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
					_domElementId = "wvp-field-typeahead-" + FieldId;
			}
			if(ValidationErrors.Count > 0)
				_validationErrors = ValidationErrors.FindAll(x=> x.Key.ToLowerInvariant() == Name.ToLowerInvariant());
			
			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>
		private async Task _onInputChangeKeypressHandlerAsync(ChangeEventArgs e)
		{
			_value = FieldValueService.InitAsString(e.Value);

			if(FilterChanged.HasDelegate)
				await FilterChanged.InvokeAsync(e);

			if (!string.IsNullOrWhiteSpace(_value) && _value.Length > MinCharLimit)
				_options = StringOptions.Where(s=> s.ToLowerInvariant().Contains(_value.ToLowerInvariant())).ToList();
			else
				_options = new List<string>();

			if (_options.Count > 0)
			{
				_options = _options.OrderBy(s => s.StartsWith(_value) ? 0 : 1).ToList();
				if (_options.First().StartsWith(_value))
					_prefill = _options.First();
			}
			else
				_prefill = "";

			if (!ValueOnlyFromOptions)
			{
				if (Mode == WvpFieldMode.Form)
				{
					ValueChanged.InvokeAsync(e);
				}
			}
		}

		private async Task _onLoadMoreAsync()
		{
			if(LoadMoreResults.HasDelegate)
				await LoadMoreResults.InvokeAsync(new ChangeEventArgs() { Value= _value });

			_onInputChangeKeypressHandlerAsync(new ChangeEventArgs() { Value = _value });
			StateHasChanged();
		}

		private void _onSelectOption(string selectedOption)
		{
			_value = selectedOption;
			_prefill = "";
			_options = new List<string>();

			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
			}
		}

		private void _onKeydownHandler(KeyboardEventArgs e)
		{
			if (e.Key == "Escape") 
			{
				var originalValue = FieldValueService.InitAsString(Value);
				_activeItemIndex = null;
				_value = originalValue;
				_prefill = "";

				if (Mode == WvpFieldMode.InlineEdit && _editEnabled)
				{
					_toggleInlineEditClickHandler(false, false);
					StateHasChanged();
				}
			}
			
			if (e.Key == "ArrowDown")
			{
				if (_activeItemIndex.HasValue && _activeItemIndex.Value < (_options.Count - 1))
					_activeItemIndex++;
				else
					_activeItemIndex = 0;				
			}

			if (e.Key == "ArrowUp")
			{
				if (_activeItemIndex.HasValue && _activeItemIndex > 0)
					_activeItemIndex--;
			}

			if (_activeItemIndex.HasValue && _options.Count > 0 && _activeItemIndex.Value < _options.Count)
			{
				_value = _options[_activeItemIndex.Value];
				_prefill = "";

				if (Mode == WvpFieldMode.Form)
				{
					ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
				}

				if (Mode== WvpFieldMode.Form)
					new JsService(JSRuntime).ScrollToElement($"{_domElementId}_{_activeItemIndex}");
				else
					new JsService(JSRuntime).ScrollToElement($"{_domElementId}_inline_{_activeItemIndex}"); 
			}

			if (e.Key == "Enter")
			{
				_activeItemIndex = null;
				_prefill = "";
				_options = new List<string>();

				if (Mode == WvpFieldMode.InlineEdit && _editEnabled)
				{
					bool applyChange = true;
					if (ValueOnlyFromOptions)
						applyChange = StringOptions.Any(s => s.ToLowerInvariant().Contains(_value.ToLowerInvariant()));

					_toggleInlineEditClickHandler(false, applyChange);
					StateHasChanged();
				}
			}	
			
		}

		private void _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{
			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_focusOnRender = true;
				_editEnabled = true;
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
			}

		}
		#endregion

		#region << JS Callbacks methods >>
		
		//[JSInvokable]
		//public async Task OnEscapeKey()
		//{
		//	if (_editEnabled)
		//	{
		//		_toggleInlineEditClickHandler(false, false);
		//		StateHasChanged();
		//	}

		//	if (Mode == WvpFieldMode.Form)
		//	{
		//		_value = _prefill;
		//		_prefill = "";
		//		_options = new List<string>();
		//		ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });

		//		StateHasChanged();
		//	}
		//}
		#endregion
	}
}