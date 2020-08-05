using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
	public partial class WvpFieldDropdown<ValueItemType> : WvpFieldBase, IDisposable 
	{

		#region << Parameters >>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }
		[Parameter] public RenderFragment FooterTemplate { get; set; }
		[Parameter] public RenderFragment<ValueItemType> ItemTemplate { get; set; }
		[Parameter] public RenderFragment<ValueItemType> SelectedItemTemplate { get; set; }
		[Parameter] public List<ValueItemType> GenericOptions { get; set; } = new List<ValueItemType>();
		[Parameter] public string LabelPropetryName { get; set; }
		[Parameter] public string ValuePropetryName { get; set; }
		[Parameter] public bool FilterEnabled { get; set; }
		[Parameter] public string GroupBy { get; set; }
		[Parameter] public EventCallback<ChangeEventArgs> FilterChanged { get; set; }

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private DotNetObjectReference<WvpFieldDropdown<ValueItemType>> _objectReference;

		private bool _editEnabled = false;

		private ValueItemType _value = default(ValueItemType);

		private Guid _originalFieldId;

		private object _originalValue;

		private List<ValueItemType> _originalOptions = new List<ValueItemType>();
		private List<ValueItemType> _options = new List<ValueItemType>();

		private string _filter = "";

		private int? _activeItemIndex = null;
		private string _dropdownShowClass = "";
		private bool _canRefreshOptions = true;
		private string _groupBy = "";

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
			if (string.IsNullOrWhiteSpace(Placeholder))
				Placeholder = "Choose";

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				if (Value != null)
				{
					if (Value.GetType() != typeof(ValueItemType))
					{
						if (!string.IsNullOrWhiteSpace(ValuePropetryName))
						{
							_value = GenericOptions.FirstOrDefault(o => o.GetType().GetProperty(ValuePropetryName).GetValue(o, null) == Value);
						}
						else
							throw new Exception("Invalid value type");
					}
					else
						_value = FieldValueService.InitAsGeneric<ValueItemType>(Value);
				}
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				if (!string.IsNullOrWhiteSpace(Id))
					_domElementId = Id;
				else
					_domElementId = "wvp-field-dropdown-" + FieldId;
			}

			if (_canRefreshOptions && (_options == null || _options.Count == 0 || JsonConvert.SerializeObject(_originalOptions) != JsonConvert.SerializeObject(GenericOptions)))
			{
				_originalOptions = GenericOptions;
				_options = GenericOptions;
			}

			if(!string.IsNullOrWhiteSpace(GroupBy) && _options != null && _options.Count > 0)
			{
				PropertyInfo prop = typeof(ValueItemType).GetProperty(GroupBy);
				if (prop != null && prop.PropertyType.Name == "String")
					_groupBy = GroupBy;
			}

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI handlers >>
		private void onDropdownClick(MouseEventArgs e)
		{
			_filter = "";
			_dropdownShowClass = "show";
			new JsService(JSRuntime).FocusElement("wvp-filter-text-" + _domElementId);
		}

		private void _filterOptionsHandler(ChangeEventArgs e)
		{
			if(FilterChanged.HasDelegate)
				FilterChanged.InvokeAsync(new ChangeEventArgs() { Value = e.Value });
			_canRefreshOptions = true;
			StateHasChanged();
		}

		private void _selectValueClickHandler(ValueItemType item)
		{
			_filter = "";
			_options = GenericOptions;
			_value = item;
			_dropdownShowClass = "";
			if (Mode == WvpFieldMode.Form && ValueChanged.HasDelegate)
			{
				if (string.IsNullOrWhiteSpace(ValuePropetryName))
					ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
				else
				{
					var propInfo = item.GetType().GetProperty(ValuePropetryName);
					if (propInfo == null)
						throw new Exception($"Property '{ValuePropetryName}' not found");

					ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = propInfo.GetValue(item, null) });
				}
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
				var originalValue = FieldValueService.InitAsGeneric<ValueItemType>(Value);
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

		private void _onKeydownHandler(KeyboardEventArgs e)
		{
			if (e.Key == "Escape")
			{
				_activeItemIndex = null;				
				_dropdownShowClass = "";
				_canRefreshOptions = true;

				if (Mode == WvpFieldMode.InlineEdit && _editEnabled)
				{
					_toggleInlineEditClickHandler(false, false);
					StateHasChanged();
				}
				
				if (Mode == WvpFieldMode.Form)
				{
					_value = default;
					ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
				}
			}

			if (e.Key == "ArrowDown")
			{
				if (_activeItemIndex.HasValue && _activeItemIndex.Value < (_options.Count - 1))
					_activeItemIndex++;
				else
					_activeItemIndex = 0;

				_canRefreshOptions = false;
			}

			if (e.Key == "ArrowUp")
			{
				if (_activeItemIndex.HasValue && _activeItemIndex > 0)
					_activeItemIndex--;

				_canRefreshOptions = false;
			}

			if (_activeItemIndex.HasValue && _options.Count > 0 && _activeItemIndex.Value < _options.Count)
			{
				_value = _options[_activeItemIndex.Value];
				
				object selectedValue;
				if (!string.IsNullOrWhiteSpace(ValuePropetryName))
				{
					selectedValue = _value.GetType().GetProperty(ValuePropetryName).GetValue(_value, null);
				}
				else
					selectedValue = _value;

				if (Mode == WvpFieldMode.Form)
				{
					ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = selectedValue });
				}

				if (Mode == WvpFieldMode.Form)
					new JsService(JSRuntime).ScrollToElement($"{_domElementId}_{_activeItemIndex}");
				else
					new JsService(JSRuntime).ScrollToElement($"{_domElementId}_inline_{_activeItemIndex}");
			}

			if (e.Key == "Enter")
			{
				_activeItemIndex = null;
				_dropdownShowClass = "";
				_canRefreshOptions = true;

				if (Mode == WvpFieldMode.InlineEdit && _editEnabled)
				{
					_toggleInlineEditClickHandler(false, true);
					StateHasChanged();
				}
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