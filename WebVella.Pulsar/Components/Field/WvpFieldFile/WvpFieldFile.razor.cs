using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;

namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldFile : WvpFieldBase, IDisposable
	{

		#region << Parameters >>
		[Parameter] public Dictionary<string, object> Attributes { get; set; }
		//[Parameter] public bool SingleFileOnly { get; set; } = true;//in base class
		//[Parameter] public bool ShowFileList { get; set; } = false;//in base class
		[Parameter] public int MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
		[Parameter] public int MaxMessageSize { get; set; } = 20 * 1024;//<20KB
		[Parameter] public int MaxMessageLength { get; set; } = 3;
		[Parameter] public string AllowedExtensions { get; set; }

		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private string _domElementId = "";
		private string _domFieldElementId = ""; //we need this for the outside click monitor

		private DotNetObjectReference<WvpFieldFile> _objectReference;

		protected ElementReference _inputRef;

		IDisposable _thisReference;

		private bool _editEnabled = false;

		private Guid _originalFieldId;

		private object _originalValue;

		private List<WvpFileInfo> _value = new List<WvpFileInfo>();

		private string _status = "";

		private string _defaultStatus = "Or drop files here";

		private long _progressMax = 100;

		private long _progressValue = 0;
		private bool _jsInitialized = false;
		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domFieldElementId}", FieldId.ToString());

			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				if (Value == null)
					_value = new List<WvpFileInfo>();
				if (Value is List<WvpFileInfo>)
					_value = JsonConvert.DeserializeObject<List<WvpFileInfo>>(JsonConvert.SerializeObject(Value));
				else
					_value = new List<WvpFileInfo>();
			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-file-" + FieldId;
				_domFieldElementId = "wvp-field-" + FieldId;
			}

			if (string.IsNullOrWhiteSpace(_status))
				_status = _defaultStatus;

			await base.OnParametersSetAsync();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!string.IsNullOrWhiteSpace(_inputRef.Id) && !_jsInitialized)
			{
				_thisReference = DotNetObjectReference.Create(this);
				new JsService(JSRuntime).InitFileUpload(_domElementId, _thisReference);
				_jsInitialized = true;
			}
		}

		#endregion

		#region << Private methods >>

		private async Task WriteTempFileAsync(WvpFileInfo file)
		{
			string tmpFilePath = Path.GetTempFileName();
			file.Init(this);
			using (Stream fileStream = File.OpenWrite(tmpFilePath))
			{
				using (Stream stream = new MemoryStream())
				{
					await file.WriteToStreamAsync(stream);

					stream.CopyTo(fileStream);
				}
			}

			file.ServerTempPath = tmpFilePath;
			await InvokeAsync(StateHasChanged);
		}
		#endregion

		#region << UI Handlers >>

		private void _browseBtnClickHandler()
		{
			new JsService(JSRuntime).SimulateClick(_inputRef);
		}

		private async Task _removeValueClickHandlerAsync(WvpFileInfo value)
		{
			_value.Remove(value);
			if (Mode == WvpFieldMode.Form)
				ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });

			await InvokeAsync(StateHasChanged);

		}

		private void _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{

			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
				new JsService(JSRuntime).AddOutsideClickEventListener($"#{_domFieldElementId}", _objectReference, FieldId.ToString(), "OnFocusOutClick");
				_jsInitialized = false;
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				List<WvpFileInfo> originalValue;

				if (Value == null)
					originalValue = new List<WvpFileInfo>();
				if (Value is List<WvpFileInfo>)
					originalValue = Value as List<WvpFileInfo>;
				else
					originalValue = new List<WvpFileInfo>();

				//Apply Change
				if (applyChange)
				{
					if (JsonConvert.SerializeObject(_value, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) != JsonConvert.SerializeObject(originalValue, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }))
					{
						//Update Function should be called
						if (ValueChanged.HasDelegate)
							ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
				}
				_editEnabled = false;
				new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
				new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domFieldElementId}", FieldId.ToString());
			}

		}
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task NotifyChange(List<WvpFileInfo> files)
		{
			foreach (var file in files)
			{
				if (file == null)
				{
					_status = "File not found";
					return;
				}
				else if (file.Size > MaxFileSize)
				{
					_status = $"That's too big. Max size: {WvpHelpers.GetSizeStringFromSize(MaxFileSize)}";
					return;
				}
				else
				{
					_status = "Loading...";
				}
				WriteTempFileAsync(file);
			}

			if (SingleFileOnly)
				_value = new List<WvpFileInfo>();

			_value.AddRange(files);

			if (Mode == WvpFieldMode.Form)
				ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });

			StateHasChanged();
		}

		[JSInvokable]
		public async Task OnFocusOutClick()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, true);
				StateHasChanged();
			}
		}


		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, false);
				StateHasChanged();
			}
		}

		public async Task UpdateProgressAsync(long progressValue, long progressMax, string status)
		{
			await InvokeAsync(() =>
			{
				_progressMax = progressMax;
				_progressValue = progressValue;
				_status = status;
				StateHasChanged();
			});
		}

		public async Task WriteToStreamAsync(WvpFileInfo fileInfo, Stream stream, WvpFieldFile component)
		{
			CancellationToken cancellationToken = CancellationToken.None;

			await Task.Run(async () =>
			{
				await component.UpdateProgressAsync(0, fileInfo.Size, "0%");
				var position = 0;
				long qPosition = 0;

				try
				{
					if (!string.IsNullOrWhiteSpace(fileInfo.ServerTempPath))
					{
						using (FileStream fs = File.OpenRead(fileInfo.ServerTempPath))
						{
							byte[] b = new byte[1024];
							UTF8Encoding temp = new UTF8Encoding(true);
							while (fs.Read(b, 0, b.Length) > 0)
							{
								await stream.WriteAsync(b, cancellationToken);
							}
						}

						File.Delete(fileInfo.ServerTempPath);
						fileInfo.ServerTempPath = string.Empty;
					}
					else
					{

						var q = new Queue<ValueTask<string>>();

						while (position < fileInfo.Size)
						{
							while (q.Count < MaxMessageLength && qPosition < fileInfo.Size)
							{
								cancellationToken.ThrowIfCancellationRequested();
								var taskPosition = qPosition;
								var taskSize = Math.Min(MaxMessageSize, (fileInfo.Size - qPosition));

								var task = JSRuntime.InvokeAsync<string>("WebVellaPulsar.readFileData",
									cancellationToken,
									_inputRef,
									fileInfo.Id, taskPosition, taskSize);
								q.Enqueue(task);
								qPosition += taskSize;
							}

							if (q.Count == 0)
								continue;

							var task2 = q.Dequeue();

							int q1;
							int q2;

							ThreadPool.GetAvailableThreads(out q1, out q2);
							var base64 = await task2.ConfigureAwait(true);
							var buffer2 = Convert.FromBase64String(base64);
							await stream.WriteAsync(buffer2, cancellationToken);
							position += buffer2.Length;
							await component.UpdateProgressAsync(position, fileInfo.Size, $"{(position / fileInfo.Size) * 100}%");
						}
					}

					stream.Seek(0, SeekOrigin.Begin);
				}
				catch (Exception ex)
				{
					int i = 0;
				}
				finally
				{
					await UpdateProgressAsync(0, 100, _defaultStatus);
				}

			});
		}

		#endregion
	}
}