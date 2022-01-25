using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Linq;
using WebVella.Pulsar.Services;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputAutocomplete : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public IEnumerable<string> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

		/// <summary>
		/// Method used to filter the Data based on a search string. By default it will apply internal contains func
		/// </summary>
		[Parameter] public Func<IEnumerable<string>, string, IEnumerable<string>> FilterFunc { get; set; }

		/// <summary>
		/// Minimal text input length for triggering autocomplete
		/// </summary>
		[Parameter] public int MinLength { get; set; } = 1;

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Pattern { get; set; } = "";

		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// Current Autocomplete value
		/// </summary>
		[Parameter] public string Value { get; set; } = "";

		[Parameter] public bool BlurOnSubmit { get; set; } = false;
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private int _activeItemIndex = -1;

		private IEnumerable<string> _options;

		private List<string> _filteredOptions = new List<string>();

		private bool _isDataTouched = true;

		private string _inputId = "input-" + Guid.NewGuid();

		/// <summary>
		/// Field is used to stop the dropdown visibility process when value is submited
		/// </summary>
		private bool _preventNextOnInputDropdownVisibilityCheck = false;

		/// <summary>
		/// Field is used to stop the next submit on Blur when the submit is done already by a Enter key or select
		/// </summary>
		private bool _skipNextBlurSubmit = false;

		private bool _isDropdownVisible = false;

		private bool _isDropdownHovered = false;

		private string _originalValue = "";

		private string _value = null;

		private bool _easySubmit = true;
		#endregion

		#region <<Store properties>>

		#endregion

		#region << Lifecycle methods >>
		protected override void OnInitialized()
		{
			_filteredOptions = _filterData();

			_originalValue = Value;
			_value = FieldValueService.InitAsString(Value);

			if (!String.IsNullOrWhiteSpace(Id))
				_inputId = Id;

		}

		protected override void OnParametersSet()
		{
			if (_originalValue != Value)
			{
				_originalValue = FieldValueService.InitAsString(Value);
				_value = FieldValueService.InitAsString(Value);
				//if (_value?.Length >= MinLength)
				//{
				//	if (_filteredOptions.Count > 0)
				//		_isDropdownVisible = true;
				//	else
				//		_isDropdownVisible = false;
				//}
			}
			if (_isDataTouched)
			{
				_filteredOptions = _filterData();
			}
		}

		#endregion

		#region << Private methods >>
		private List<string> _internalFilter(string search = "")
		{
			if (String.IsNullOrWhiteSpace(search))
				return _options.ToList();

			return _options.ToList().FindAll(x => x.ToLowerInvariant().Contains(search.ToLowerInvariant())).ToList();
		}
		private List<string> _filterData(string search = "")
		{
			if (_options != null)
			{
				_isDataTouched = false;
				if (FilterFunc != null)
					return FilterFunc?.Invoke(_options, search).ToList();
				else
					return _internalFilter(search);
			}
			else
			{
				_isDataTouched = false;
				return new List<string>();
			}
		}

		public async Task Clear()
		{
			_isDropdownVisible = false;
			_isDropdownHovered = false;
			_easySubmit = true;
			_value = null;
			_activeItemIndex = -1;
			await Task.Delay(1);
			//await InvokeAsync(StateHasChanged);
			if (BlurOnSubmit)
				_ = await new JsService(JSRuntime).BlurElement(_inputId);
			else
			{
				_ = await new JsService(JSRuntime).FocusElement(_inputId);
			}
		}

		private void UpdateActiveFilterIndex(int activeItemIndex)
		{
			if (activeItemIndex < 0)
				activeItemIndex = -1;

			if (activeItemIndex > (_filteredOptions.Count - 1))
				activeItemIndex = -1;

			_activeItemIndex = activeItemIndex;
		}


		#endregion

		#region << Store methods >>

		#endregion

		#region << Ui handlers >>

		private async Task _ddMenuHoverChangeHandler(ChangeEventArgs args)
		{
			_isDropdownHovered = (bool)args.Value;
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onKeyDownHandler(KeyboardEventArgs e)
		{
			//if (!_isDropdownVisible)
			//	return;

			// make sure everything is filtered
			if (_isDataTouched)
				_filteredOptions = _filterData();//

			var activeItemIndex = _activeItemIndex;
			if ((e.Code == "Enter" || e.Code == "NumpadEnter" || e.Code == "Tab"))
			{
				var selectedItem = _filteredOptions.ElementAtOrDefault(activeItemIndex);
				if (selectedItem != null)
				{
					_value = selectedItem;
				}
				_preventNextOnInputDropdownVisibilityCheck = true;
				_skipNextBlurSubmit = true;
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
				await Clear(); //boz: should be after the invoke so _value is not cleared
				return;
			}
			else if (e.Code == "Escape")
			{
				await Clear();
				return;
			}
			else if (e.Code == "ArrowUp")
			{
				UpdateActiveFilterIndex(--activeItemIndex);
			}
			else if (e.Code == "ArrowDown")
			{
				UpdateActiveFilterIndex(++activeItemIndex);
			}
			if (activeItemIndex > -1)
			{
				_easySubmit = false;
			}
			else
			{
				_easySubmit = true;
			}
			//await Task.Delay(1);
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onBlurHandler()
		{
			if (_skipNextBlurSubmit)
			{
				_skipNextBlurSubmit = false;
				return;
			}
			if (!_isDropdownHovered && _easySubmit && _activeItemIndex == -1)
			{
				await Task.Delay(1);
				//await InvokeAsync(StateHasChanged);
			}
			if (!_isDropdownHovered)
			{
				_isDropdownVisible = false;
				_value = "";
				await Task.Delay(1);
				//await InvokeAsync(StateHasChanged);
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			}

		}

		private async Task _onInputHandler(ChangeEventArgs args)
		{
			_value = args.Value.ToString();
			if (!_preventNextOnInputDropdownVisibilityCheck)
			{
				if (_value?.Length >= MinLength && _filteredOptions.Any())
					_isDropdownVisible = true;
				else
					_isDropdownVisible = false;
			}
			_preventNextOnInputDropdownVisibilityCheck = false;
			await Task.Delay(1);
			//await InvokeAsync(StateHasChanged);
			await OnInput.InvokeAsync(args);
		}

		private async Task _itemSelected(string itemValue)
		{
			_skipNextBlurSubmit = true;
			await Clear();
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = itemValue });
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}