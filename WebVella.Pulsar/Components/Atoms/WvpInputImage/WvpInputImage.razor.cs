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
using System.IO;
using System.Threading;
using System.Text;
using System.Linq;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInputImage : WvpInputBase, IAsyncDisposable
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public WvpFileInfo Value { get; set; } = null;

		[Parameter] public int MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB

		[Parameter] public string AllowedExtensions { get; set; } = "image/*";

		#endregion

		#region << Callbacks >>
		public async Task UpdateProgressAsync(WvpFileInfo fileInfo)
		{
			_value = fileInfo;
			//await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << Private properties >>

		private ElementReference _elementRef;

		private DotNetObjectReference<WvpInputImage> _objectReference;

		private List<string> _cssList = new List<string>();

		private WvpFileInfo _originalValue = null;

		private WvpFileInfo _value = null;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				_ = await new JsService(JSRuntime).InitFileUpload(Id, _objectReference);
			}
		}

		public async ValueTask DisposeAsync()
		{
			//await Task.Delay(1);
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
				_value = null;
				if (Value != null)
				{
					var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full };
					jsonSettings.Converters.Insert(0, new PrimitiveJsonConverter());
					_value = JsonConvert.DeserializeObject<WvpFileInfo>(JsonConvert.SerializeObject(Value, Formatting.None, jsonSettings), jsonSettings);
				}
			}

			if (!String.IsNullOrWhiteSpace(Name))
				AdditionalAttributes["name"] = Name;

			if (!String.IsNullOrWhiteSpace(Placeholder))
				AdditionalAttributes["placeholder"] = Placeholder;

			if (!String.IsNullOrWhiteSpace(Title))
				AdditionalAttributes["title"] = Title;

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << Ui handlers >>
		private async Task _removeFile()
		{
			_value = null;
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			//await InvokeAsync(StateHasChanged);
		}
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task NotifyChange(List<WvpFileInfo> files)
		{
			_value = null;
			if (files.Count > 0)
				_value = files[0];

			foreach (var file in files)
			{
				if (file == null)
				{
					//file.Status = "File not found";
					//await InvokeAsync(StateHasChanged);
					return;
				}
				else if (file.Size > MaxFileSize)
				{
					file.Status = $"That's too big. Max size: {WvpHelpers.GetSizeStringFromSize(MaxFileSize)}";
					//await InvokeAsync(StateHasChanged);
					return;
				}
				else
				{
					file.Status = "Loading...";
				}

				await file.WriteTempFileAsync(JSRuntime, _elementRef, UpdateProgressAsync);

			}
			await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });

			//await InvokeAsync(StateHasChanged);
		}


		#endregion
	}
}