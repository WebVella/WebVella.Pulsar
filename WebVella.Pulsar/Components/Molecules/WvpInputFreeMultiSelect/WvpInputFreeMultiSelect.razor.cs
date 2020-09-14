using System.Collections.Generic;
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
using System.Diagnostics;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputFreeMultiSelect : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public List<string> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

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
		[Parameter] public List<string> Value { get; set; } = new List<string>();

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private bool _isDataTouched = true;

		private List<string> _options;

		private string _filter = "";

		private List<string> _filteredOptions = new List<string>();

		private List<string> _originalValue = new List<string>();

		private List<string> _value = new List<string>();

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

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = Value.ToList();
			}

			if (_isDataTouched)
				_filteredOptions = _filterData(_filter);

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			await base.OnParametersSetAsync();
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

		#endregion

		#region << Ui handlers >>

		private async Task _onFilterInputHandler(ChangeEventArgs e)
		{
			Debug.WriteLine("_onFilterInputHandler");
			_filter = e.Value?.ToString();
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onValueChangeHandler(ChangeEventArgs e)
		{
			Debug.WriteLine("_onValueChangeHandler");
			var stringValue = (string)e.Value;
			_value.Add(stringValue);
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onAddHandler(ChangeEventArgs e)
		{
			_filter = e.Value?.ToString();
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}