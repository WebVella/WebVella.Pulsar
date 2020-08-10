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
	public partial class WvpInputDate : WvpInputBase, IDisposable
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// DateTime in UTC format
		/// </summary>
		[Parameter] public DateTime? Value { get; set; } = null;

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; } //Fires on each user input
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private List<string> _inputGroupList = new List<string>();

		private DotNetObjectReference<WvpInputDate> _objectReference;

		private DateTime? _originalValue = null;

		private string _value = null;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				JsService.AddFlatPickrDate(Id, _objectReference, "en");
			}
			await base.OnAfterRenderAsync(firstRender); //Set the proper Id
		}

		void IDisposable.Dispose()
		{
			JsService.RemoveFlatPickrDate(Id);
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
				if (Value == null)
					_value = null;
				else
				{
					_value = Value.Value.ToString("yyyy-MM-dd");
				}
				await Task.Delay(5);
				await JsService.SetFlatPickrDateChange(Id, _value);
			}

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task NotifyChange(DateTime? date)
		{
			if (date != Value)
			{
				if (date == null)
				{
					await JsService.ClearFlatPickrDateTime(Id);
				}
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = date });
				await OnInput.InvokeAsync(new ChangeEventArgs { Value = date });
			}
		}
		#endregion
	}
}