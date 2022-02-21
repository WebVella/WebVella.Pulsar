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

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputSelect<TItem> : WvpInputBase
	{

		#region << Parameters >>

		[Parameter] public RenderFragment<TItem> WvpInputSelectField { get; set; }

		[Parameter] public RenderFragment<TItem> WvpInputSelectOption { get; set; }

		[Parameter] public bool IsFilterable { get; set; } = false;

		[Parameter] public IEnumerable<TItem> Options { get; set; }

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public TItem Value { get; set; }
		[Parameter] public WvpDropDownDirection Mode { get; set; } = WvpDropDownDirection.DropDown;

		[Parameter] public bool EndIsReached { get; set; } = false;
	

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback FetchMoreRows { get; set; } 
		[Parameter] public EventCallback<string> FreeInputSubmit { get; set; } 
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private bool? _isDropdownVisible = null;

		private string _filterElementId = "wvp-filter-" + Guid.NewGuid();

		private string _filter = "";
		
		private string _filterPlaceholder = "type and select value";

		private List<TItem> _filteredOptions = new List<TItem>();

		private string _originalValueJson = null;

		private TItem _value;

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
				var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All,TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full };
				jsonSettings.Converters.Insert(0, new PrimitiveJsonConverter());
				_value = JsonConvert.DeserializeObject<TItem>(JsonConvert.SerializeObject(Value, Formatting.None, jsonSettings), jsonSettings);
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			if(!FetchMoreRows.HasDelegate)
				EndIsReached = true;

			
			if(FreeInputSubmit.HasDelegate)
				_filterPlaceholder = "type, select or ENTER";
			else
				_filterPlaceholder = "type and select value";
			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>
		public async Task<bool> PerformVisibilityCheck()
		{
			if(_infiniteScrollRef != null)
				return await _infiniteScrollRef.PerformVisibilityCheck();

			return false;
		}

		#endregion

		#region << Ui handlers >>

		private async Task _dropdownToggleCallback(bool isVisible)
		{
			if (Options == null || Options.ToList().Count == 0)
			{
				return;
			}
			if (isVisible)
			{
				_filter = "";
				await JsService.FocusElementBySelector($"#{_filterElementId}");
				await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
				//await InvokeAsync(StateHasChanged);
			}
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onFilterInputHandler(ChangeEventArgs e)
		{
			_filter = e.Value?.ToString();
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _filter });
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _filterSubmit()
		{
			if (FreeInputSubmit.HasDelegate && !String.IsNullOrWhiteSpace(_filter))
			{
				await FreeInputSubmit.InvokeAsync(_filter);
				_filter = null;
				_isDropdownVisible = false;
				await InvokeAsync(StateHasChanged);
				await Task.Delay(1);
				_isDropdownVisible = null;
				await InvokeAsync(StateHasChanged);				
			}
		}

		private async Task _onSelectHandler(TItem item)
		{
			_value = item;

			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });

			_isDropdownVisible = false;
			await InvokeAsync(StateHasChanged);
			await Task.Delay(1);
			_isDropdownVisible = null;
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}