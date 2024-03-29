﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using WebVella.Pulsar.Services;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System.Linq;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputMultiSelect<TItem> : WvpInputBase
	{

		#region << Parameters >>

		[Parameter] public RenderFragment<TItem> WvpInputMultiSelectField { get; set; } = null;

		[Parameter] public RenderFragment<TItem> WvpInputMultiSelectOption { get; set; } = null;

		[Parameter] public bool IsFilterable { get; set; } = false;

		[Parameter] public IEnumerable<TItem> Options { get; set; }

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public IEnumerable<TItem> Value { get; set; }

		[Parameter] public bool EndIsReached { get; set; } = false;

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback FetchMoreRows { get; set; }
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private bool? _isDropdownVisible = null;

		private string _filterElementId = "wvp-filter-" + Guid.NewGuid();

		private string _filter = "";

		private List<TItem> _filteredOptions = new List<TItem>();

		private string _originalValueJson;

		private List<TItem> _value;

		private WvpObservedItem _infiniteScrollRef = null;

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();

			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"input-group-{sizeSuffix}");

			if (_originalValueJson != JsonConvert.SerializeObject(Value))
			{
				_originalValueJson = JsonConvert.SerializeObject(Value);
				_value = Value.ToList();
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			if (!FetchMoreRows.HasDelegate)
				EndIsReached = true;

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>
		public async Task<bool> PerformVisibilityCheck()
		{
			if (_infiniteScrollRef != null)
				return await _infiniteScrollRef.PerformVisibilityCheck();

			return false;
		}

		#endregion

		#region << Ui handlers >>

		private async Task _dropdownToggleCallback(bool isVisible)
		{
			if (isVisible)
			{
				_filter = "";
				await JsService.FocusElementBySelector($"#{_filterElementId}");
				await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
			}
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onFilterInputHandler(ChangeEventArgs e)
		{
			_filter = e.Value?.ToString();
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onSelectHandler(TItem item)
		{
			if (_value.Any(x => JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(item)))
			{
				_value = _value.FindAll(x => JsonConvert.SerializeObject(x) != JsonConvert.SerializeObject(item));
			}
			else
			{
				_value.Add(item);
			}

			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			//await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}