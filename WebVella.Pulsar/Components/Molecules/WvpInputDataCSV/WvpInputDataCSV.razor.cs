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
using System.Globalization;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputDataCSV<TItem> : WvpInputBase
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>

		[Parameter] public WvpDelimiterType Delimiter { get; set; } = WvpDelimiterType.COMMA;

		[Parameter] public bool HasHeader { get; set; } = false;

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public List<TItem> Rows { get; set; } = new List<TItem>();

		[Parameter] public string Value { get; set; } = "";

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback<ChangeEventArgs> DelimiterChanged { get; set; }
		[Parameter] public EventCallback<ChangeEventArgs> HasHeaderChanged { get; set; }
		[Parameter] public EventCallback<ChangeEventArgs> RowsChanged { get; set; }
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private List<string> _controlCssList = new List<string>();

		private string _originalValue = "";

		private string _value = "";

		private List<TItem> _originalRows = new List<TItem>();

		private List<TItem> _rows = new List<TItem>();

		private WvpDelimiterType _originalDelimiter = WvpDelimiterType.COMMA;

		private WvpDelimiterType _delimiter = WvpDelimiterType.COMMA;

		private bool? _originalHasHeader = null;

		private bool _hasHeader = false;

		private List<WvpDelimiterType> _delimiterTypes = new List<WvpDelimiterType>();

		private string _activeTab = "input"; //"preview"

		#endregion

		#region << Lifecycle methods >>


		protected override async Task OnInitializedAsync()
		{
			await Task.Delay(0);
			var enumList = Enum.GetValues(typeof(WvpDelimiterType)).Cast<WvpDelimiterType>();
			foreach (WvpDelimiterType item in enumList)
			{
				_delimiterTypes.Add(item);
			}
		}

		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();

			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			_cssList.Add($"form-control-datacsv");

			var valueHasChanged = false;
			var rowsHasChanged = false;
			var delimterHasChanged = false;
			var hasHeaderHasChanged = false;

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsString(Value);
				valueHasChanged = true;
			}

			if (JsonConvert.SerializeObject(_originalRows) != JsonConvert.SerializeObject(Rows))
			{
				_originalRows = Rows.ToList();
				_rows = Rows.ToList();
				rowsHasChanged = true;
			}

			if (JsonConvert.SerializeObject(_originalDelimiter) != JsonConvert.SerializeObject(Delimiter))
			{
				_originalDelimiter = Delimiter;
				_delimiter = Delimiter;
				delimterHasChanged = true;
			}

			if (JsonConvert.SerializeObject(_originalHasHeader) != JsonConvert.SerializeObject(HasHeader))
			{
				_originalHasHeader = HasHeader;
				_hasHeader = HasHeader;
				hasHeaderHasChanged = true;
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			if (!String.IsNullOrWhiteSpace(Title))
				AdditionalAttributes["title"] = Title;

			//Value has preference
			if (valueHasChanged)
			{
				if (!String.IsNullOrWhiteSpace(_value))
				{
					_rows = WvpHelpers.GetCsvData<TItem>(_value, _hasHeader,_delimiter,Culture);
				}
				else{
					_rows = new List<TItem>();
				}
			}
			else if(rowsHasChanged || delimterHasChanged || hasHeaderHasChanged){
				if(_rows.Count == 0){
					_value = "";
				}
				else{
					//Specifics here is that header should not be generated
					_value =  WvpHelpers.WriteCsvData<TItem>(_rows, false,_delimiter,Culture);
				}
			}


			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

		private void _tabClickHandler(string tab){
			_activeTab = tab;	
		}

		private async Task _onValueChangeHandler(ChangeEventArgs e)
		{
			var value = (string)e.Value;
			//Generate new Rows
			var rows = WvpHelpers.GetCsvData<TItem>(value, _hasHeader,_delimiter,Culture);
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = value });
			await RowsChanged.InvokeAsync(new ChangeEventArgs { Value = rows });

		}

		private async Task _onHasHeaderChangeHandler(ChangeEventArgs e)
		{
			await HasHeaderChanged.InvokeAsync(new ChangeEventArgs { Value = (bool)e.Value });
		}

		private async Task _onDelimterChangeHandler(ChangeEventArgs e)
		{
			await DelimiterChanged.InvokeAsync(new ChangeEventArgs { Value = (WvpDelimiterType)e.Value });
		}

		//private async Task _onBlurHandler(FocusEventArgs e)
		//{
		//	await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
		//	await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
		//	await InvokeAsync(StateHasChanged);
		//}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}