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
	public partial class WvpInputCheckboxList<TItem> : WvpInputBase
	{

		#region << Parameters >>

		[Parameter] public bool IsVertical { get; set; } = false;

		[Parameter] public RenderFragment<TItem> WvpInputCheckboxListOption { get; set; }

		[Parameter] public IEnumerable<TItem> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

		[Parameter] public IEnumerable<TItem> Value { get; set; }

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private IEnumerable<TItem> _options;

		private IEnumerable<TItem> _originalValue;

		private List<TItem> _value;

		private bool _isDataTouched = true;

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnInitializedAsync()
		{
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");


			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = Value.ToList();
			}

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

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
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
		}

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}