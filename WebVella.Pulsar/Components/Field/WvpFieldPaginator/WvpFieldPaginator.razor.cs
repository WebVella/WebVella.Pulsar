using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;


namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldPaginator : WvpFieldBase, IDisposable 
	{

		#region << Parameters >>
		[Parameter] public bool PagingEnabled { get; set; } = false;
		[Parameter] public int PageSize { get; set; } = 10;
		[Parameter] public List<int> PageSizeOptions { get; set; } = new List<int>() { 5, 10, 25, 50, 100 };
		[Parameter] public int TotalCount { get; set; } = 0;
		[Parameter] public EventCallback<ChangeEventArgs> PageChanged { get; set; }
		[Parameter] public EventCallback<ChangeEventArgs> PageSizeChanged { get; set; }

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private bool _editEnabled = false;

		private Guid _originalFieldId;

		private object _originalValue;
		

		private int _defaultValue = 1;

		private int _originalPageSize;

		private int _pageSize;
		
		private int? _value = null;

		private int _recordsStart = 1;

		private int _recordsEnd = 1;

		private bool _isFirsPage;

		private bool _isLastPage;

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsInt(Value);
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-paginator-" + FieldId;
			}

			if (_originalPageSize != PageSize)
			{
				_originalPageSize = PageSize;
				_pageSize = PageSize;
			}

			if (!_value.HasValue)
				_value = 0;
						
			_recordsStart = _value.Value + 1;

			var endRec = _value.Value + _pageSize;
			_recordsEnd = endRec > TotalCount ? TotalCount : endRec;
			_isFirsPage = _recordsStart == 1;
			_isLastPage = _recordsEnd == TotalCount;

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI handlers >>
		private void _onOptionInputEvent(ChangeEventArgs e)
		{
			_pageSize = int.Parse((string)e.Value);

			if (PageSizeChanged.HasDelegate)
				PageSizeChanged.InvokeAsync(new ChangeEventArgs() { Value= _pageSize });

			StateHasChanged();
		}

		private void _previousPageHandler()
		{
			if (TotalCount > 0)
				_isLastPage = false;
			else
				_isLastPage = true;

			if (TotalCount > 0 && _value > _pageSize)
				_value-= _pageSize;
			else 
			{ 
				_value = 0;
				_isFirsPage = true;
			}

			if (PageChanged.HasDelegate)
				PageChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
			
			StateHasChanged();
		}

		private void _nextPageHandler() 
		{
			if (TotalCount > 0)
				_isFirsPage = false;
			else
				_isFirsPage = true;

			if (TotalCount > 0)
			{
				if (_value < TotalCount - _pageSize)
					_value += _pageSize;
				else if (_value < TotalCount)
				{
					_value = TotalCount - _value;
					_isLastPage = true;
				}
				else
				{
					_value = TotalCount;
					_isLastPage = true;
				}
			}
			else
			{
				_value = 0;
				_isLastPage = true;
			}
			
			if (PageChanged.HasDelegate)
				PageChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });

			StateHasChanged();
		}
		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}