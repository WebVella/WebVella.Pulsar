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
	public partial class WvpInputAutocomplete<TItem> : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public IEnumerable<TItem> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

		/// <summary>
		/// Method used to get the display field from the data source
		/// </summary>
		[Parameter] public Func<TItem, string> GetTextFunc { get; set; }


		/// <summary>
		/// Method used to get the value field from the data source
		/// </summary>
		[Parameter] public Func<TItem, string> GetValueFunc { get; set; }

		/// <summary>
		/// Method used to filter the Data based on a search string. By default it will apply internal contains func
		/// </summary>
		[Parameter] public Func<IEnumerable<TItem>, string, IEnumerable<TItem>> FilterFunc { get; set; }

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

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private int _activeItemIndex = -1;

		private IEnumerable<TItem> _options;

		private List<TItem> _filteredOptions = new List<TItem>();

		private bool _isDataTouched = true;

		/// <summary>
		/// Field is used to stop the dropdown visibility process when value is submited
		/// </summary>
		private bool _preventNextOnInputDropdownVisibilityCheck = false;

		private bool _isDropdownVisible = false;

		private string _originalValue = "";

		private string _value = null;

		#endregion

		#region <<Store properties>>

		#endregion

		#region << Lifecycle methods >>
		protected override void OnInitialized()
		{
			_filteredOptions = _filterData();

			_originalValue = Value;
			_value = FieldValueService.InitAsString(Value);
			base.OnInitialized();
		}

		protected override void OnParametersSet()
		{
			if(_originalValue != Value)
				_value = FieldValueService.InitAsString(Value);

			if (_isDataTouched)
				_filteredOptions = _filterData();

			
			base.OnParametersSet();
		}

		#endregion

		#region << Private methods >>
		private List<TItem> _internalFilter(string search = "")
		{

			var query = from q in Options
							let text = GetTextFunc.Invoke(q)
							where text.IndexOf(search, 0, System.StringComparison.CurrentCultureIgnoreCase) >= 0
							select q;

			return query.ToList();
		}
		private List<TItem> _filterData(string search = "")
		{
			if (Options != null)
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
				return new List<TItem>();
			}
		}

		public async Task Clear()
		{
			_isDropdownVisible = false;
			_value = null;
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
		}

		private void UpdateActiveFilterIndex(int activeItemIndex)
		{
			if (activeItemIndex < 0)
				activeItemIndex = 0;

			if (activeItemIndex > (_filteredOptions.Count - 1))
				activeItemIndex = _filteredOptions.Count - 1;

			_activeItemIndex = activeItemIndex;
		}


		#endregion

		#region << Store methods >>

		#endregion

		#region << Ui handlers >>
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
					_value = GetValueFunc(selectedItem);
				}
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
				await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
				_isDropdownVisible = false;
				_preventNextOnInputDropdownVisibilityCheck = true;
			}
			else if (e.Code == "Escape")
			{
				await Clear();
			}
			else if (e.Code == "ArrowUp")
			{
				UpdateActiveFilterIndex(--activeItemIndex);
			}
			else if (e.Code == "ArrowDown")
			{
				UpdateActiveFilterIndex(++activeItemIndex);
			}
			await InvokeAsync(StateHasChanged);
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

			await OnInput.InvokeAsync(args);
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onValueChangedHandler(ChangeEventArgs args)
		{
			_isDropdownVisible = false;
			await ValueChanged.InvokeAsync(args);
			await OnInput.InvokeAsync(args);
			await InvokeAsync(StateHasChanged);
		}

		private async Task _itemSelected(string itemValue)
		{
			_isDropdownVisible = false;

			var selectedItem = _filteredOptions.FirstOrDefault(x => GetValueFunc(x) == itemValue);

			_value = selectedItem != null ? GetTextFunc?.Invoke(selectedItem) : string.Empty;

			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}