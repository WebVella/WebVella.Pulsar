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

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputDateTime : WvpInputBase, IAsyncDisposable
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// If empty string - user input will be considered in the user's local timezone. If set to specific timezone, user's input will be considered as provided in the said timezone
		/// </summary>
		[Parameter] public string TimezoneName { get; set; } = "";

		/// <summary>
		/// DateTime in UTC format
		/// </summary>
		[Parameter] public DateTime? Value { get; set; } = null;

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private List<string> _inputGroupList = new List<string>();

		private DotNetObjectReference<WvpInputDateTime> _objectReference;

		private DateTime? _originalValue = null;

		private string _value = null;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await JsService.AddFlatPickrDateTime(Id, _objectReference, Culture.TwoLetterISOLanguageName);
			}
			await base.OnAfterRenderAsync(firstRender); //Set the proper Id
		}

		public async ValueTask DisposeAsync()
		{
			await JsService.RemoveFlatPickrDateTime(Id);
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();
	
			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");


			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = Value.WvpConvertDateToBrowserLocal(await JsService.GetBrowserUtcOffsetInMinutes(), TimezoneName)?.ToString("yyyy-MM-ddTHH:mm");
				await Task.Delay(5);
				await JsService.SetFlatPickrDateTimeChange(Id, _value);
			}

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>
		private async Task _clearLinkHandler()
		{
			_value = null;
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			await JsService.ClearFlatPickrDateTime(Id);
			await InvokeAsync(StateHasChanged);
		}
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task NotifyChange(DateTime? date)
		{
			if (date != Value)
			{
				DateTime? newValue = null;
				if (date != null)
				{
					newValue = date.WvpBrowserLocalToDate(await JsService.GetBrowserUtcOffsetInMinutes(), TimezoneName);
				}
				else
				{
					await JsService.ClearFlatPickrDateTime(Id);
				}

				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = newValue });
				await OnInput.InvokeAsync(new ChangeEventArgs { Value = newValue });
			}
		}
		#endregion
	}
}