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
	public partial class WvpInputHtml : WvpInputBase, IAsyncDisposable
	{

		#region << Parameters >>

		[Parameter] public string Value { get; set; } = "";

		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; } //Fires on each user input
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private DotNetObjectReference<WvpInputHtml> _objectReference;

		private string _originalValue = "";

		private string _value = "";

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await JsService.AddCKEditor(Id,_objectReference, "en");
			}
			await base.OnAfterRenderAsync(firstRender); //Set the proper Id
		}

		public async ValueTask DisposeAsync()
		{
			await JsService.RemoveCKEditor(Id);
			_objectReference?.Dispose();
		}

		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();

			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				_value = FieldValueService.InitAsString(Value);
				await JsService.SetCKEditorData(Id,_value);
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>
		private async Task _onKeyDownHandler(KeyboardEventArgs e)
		{
			//Needs to be keydown as keypress is produced only on printable chars (does not work on backspace
			await Task.Delay(5);
			if (e.Key == "Tab")
			{
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			}

			await OnKeyDown.InvokeAsync(e);
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onBlurHandler()
		{
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task NotifyChange(string content)
		{
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = content });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = content });

		}
		#endregion
	}
}