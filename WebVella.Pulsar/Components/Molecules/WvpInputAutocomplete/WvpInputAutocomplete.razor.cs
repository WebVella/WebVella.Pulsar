﻿using System.Collections.Generic;
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
			base.OnInitialized();
		}

		protected override void OnParametersSet()
		{
			if (_originalValue != Value)
			{
				_originalValue = FieldValueService.InitAsString(Value);
				_value = FieldValueService.InitAsString(Value);
			}

			if (_isDataTouched)
				_filteredOptions = _filterData();


			base.OnParametersSet();
		}

		#endregion

		#region << Private methods >>
		private List<string> _internalFilter(string search = "")
		{

			var query = from q in Options
							where q.IndexOf(search, 0, System.StringComparison.CurrentCultureIgnoreCase) >= 0
							select q;

			return query.ToList();
		}
		private List<string> _filterData(string search = "")
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
			await new JsService(JSRuntime).BlurElement(_inputId);
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

		private async Task _ddMenuHoverChangeHandler(ChangeEventArgs args)
		{
			_isDropdownHovered = (bool)args.Value;
		}

		private async Task _onKeyDownHandler(KeyboardEventArgs e)
		{
			if (!_isDropdownVisible)
				return;

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

				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });

				_preventNextOnInputDropdownVisibilityCheck = true;
				await Clear();
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

			await InvokeAsync(StateHasChanged);
		}

		private async Task _onInputHandler(ChangeEventArgs args)
		{
			var value = args.Value.ToString();

			_value = value;
			if (!_preventNextOnInputDropdownVisibilityCheck)
			{
				if (_value?.Length >= MinLength && _filteredOptions.Any())
					_isDropdownVisible = true;
				else
					_isDropdownVisible = false;
			}
			_preventNextOnInputDropdownVisibilityCheck = false;

			Debug.WriteLine("AC: _onInputHandler");
			await OnInput.InvokeAsync(args);
			await InvokeAsync(StateHasChanged);

		}

		private async Task _onValueChangedHandler(ChangeEventArgs args)
		{
			//Value change in this case should be triggered only if the dropdown is not visible to prevent double posting (this method will be executed before _itemSelected)
			if (!_isDropdownHovered)
			{
				await ValueChanged.InvokeAsync(args);
				_isDropdownVisible = false;
			}
			await InvokeAsync(StateHasChanged);
		}

		private async Task _itemSelected(string itemValue)
		{
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = itemValue });
			await Clear();
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}