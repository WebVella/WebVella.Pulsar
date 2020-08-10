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
	public partial class WvpInputFile : WvpInputBase, IDisposable
	{

		#region << Parameters >>

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public List<WvpFileInfo> Value { get; set; } = null;

		[Parameter] public bool Multiple { get; set; } = false;

		[Parameter] public int MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB

		[Parameter] public string AllowedExtensions { get; set; }

		#endregion

		#region << Callbacks >>
		public async Task UpdateProgressAsync(WvpFileInfo fileInfo)
		{
			var fileIndex = -1;

			if (_value != null)
				_value.FindIndex(x => x.Name == fileInfo.Name);
			if (fileIndex > -1)
			{
				_value[fileIndex] = fileInfo;
				await InvokeAsync(StateHasChanged);
			}

		}

		#endregion

		#region << Private properties >>

		private ElementReference _elementRef;

		private DotNetObjectReference<WvpInputFile> _objectReference;

		private List<string> _cssList = new List<string>();

		private List<WvpFileInfo> _originalValue = null;

		private List<WvpFileInfo> _value = null;
		private List<WvpFileInfo> _errorFiles = new List<WvpFileInfo>();

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				new JsService(JSRuntime).InitFileUpload(Id, _objectReference);
			}
			await base.OnAfterRenderAsync(firstRender);
		}

		void IDisposable.Dispose()
		{
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnParametersSetAsync()
		{
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				if (Value == null)
					_value = new List<WvpFileInfo>();
				else
					_value = JsonConvert.DeserializeObject<List<WvpFileInfo>>(JsonConvert.SerializeObject(Value));

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
		private async Task _removeFile(WvpFileInfo file)
		{
			_value.Remove(file);
			ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
			OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
		}
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task NotifyChange(List<WvpFileInfo> files)
		{
			_errorFiles = new List<WvpFileInfo>();

			//Multi File upload
			if (Multiple)
			{
				var eligibleFiles = new List<WvpFileInfo>();
				if (_value != null)
					eligibleFiles.AddRange(_value);

				foreach (var file in files)
				{
					if (file == null)
					{
						file.Status = "File not found";
						_errorFiles.Add(file);
						await InvokeAsync(StateHasChanged);
						return;
					}
					else if (file.Size > MaxFileSize)
					{
						file.Status = $"That's too big. Max size: {WvpHelpers.GetSizeStringFromSize(MaxFileSize)}";
						_errorFiles.Add(file);
						await InvokeAsync(StateHasChanged);
						return;
					}
					else if (eligibleFiles.Any(x => x.Name == file.Name))
					{
						// If file with the same name is uploaded again, exchange
						file.Status = "Loading...";
						eligibleFiles = eligibleFiles.FindAll(x => x.Name != file.Name).ToList();
					}
					else
					{
						file.Status = "Loading...";
					}

					await file.WriteTempFileAsync(JSRuntime, _elementRef, UpdateProgressAsync);

					eligibleFiles.Add(new WvpFileInfo
					{
						ContentType = file.ContentType,
						Id = file.Id,
						LastModified = file.LastModified,
						Name = file.Name,
						Url = file.Url,
						Size = file.Size,
						ServerTempPath = file.ServerTempPath
					});
				}
				_value = eligibleFiles;
				await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
				await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
			}
			//Single File upload
			else
			{
				if (files.Count > 0)
				{
					var file = files[0];

					if (file == null)
					{
						file.Status = "File not found";
						_errorFiles.Add(file);
						await InvokeAsync(StateHasChanged);
						return;
					}
					else if (file.Size > MaxFileSize)
					{
						file.Status = $"That's too big. Max size: {WvpHelpers.GetSizeStringFromSize(MaxFileSize)}";
						_errorFiles.Add(file);
						await InvokeAsync(StateHasChanged);
						return;
					}
					else
					{
						file.Status = "Loading...";
					}

					await file.WriteTempFileAsync(JSRuntime, _elementRef, UpdateProgressAsync);
					_value = new List<WvpFileInfo>();
					_value.Add(new WvpFileInfo
					{
						ContentType = file.ContentType,
						Id = file.Id,
						LastModified = file.LastModified,
						Name = file.Name,
						Url = file.Url,
						Size = file.Size,
						ServerTempPath = file.ServerTempPath
					});
					await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					await OnInput.InvokeAsync(new ChangeEventArgs { Value = _value });
				}
			}
			await InvokeAsync(StateHasChanged);
		}


		#endregion
	}
}