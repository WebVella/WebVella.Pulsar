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
using System.Diagnostics;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputSelect<TItem> : WvpInputBase
	{

		#region << Parameters >>

		[Parameter] public RenderFragment<TItem> WvpInputSelectField { get; set; }

		[Parameter] public RenderFragment<TItem> WvpInputSelectOption { get; set; }

		[Parameter] public bool IsFilterable { get; set; } = false;

		[Parameter] public IEnumerable<TItem> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public TItem Value { get; set; }

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private IEnumerable<TItem> _options;

		private bool? _isDropdownVisible = null;

		private string _filterElementId = "wvp-filter-" + Guid.NewGuid();

		private string _filter = "";

		private List<TItem> _filteredOptions = new List<TItem>();

		private TItem _originalValue;

		private TItem _value;

		private bool _isDataTouched = true;

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

			Debug.WriteLine("param111 " + JsonConvert.SerializeObject(Value));

			Debug.WriteLine("param222 " + JsonConvert.SerializeObject(_originalValue));

			Debug.WriteLine("param333 " + JsonConvert.SerializeObject(_value));

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = JsonConvert.DeserializeObject<TItem>(JsonConvert.SerializeObject(Value));
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


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
		}

		private async Task _onFilterInputHandler(ChangeEventArgs e)
		{
			_filter = e.Value?.ToString();
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });

		}

		private async Task _onSelectHandler(TItem item)
		{
			_value = item;

			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });

			_isDropdownVisible = false;
			await Task.Delay(5);
			_isDropdownVisible = null;
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}