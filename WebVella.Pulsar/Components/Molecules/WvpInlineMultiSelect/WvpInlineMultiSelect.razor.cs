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
using System.Diagnostics;
using System.Linq;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInlineMultiSelect<TItem> : WvpInlineBase, IAsyncDisposable
	{

		#region << Parameters >>

		[Parameter] public bool IsFilterable { get; set; } = false;

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public RenderFragment<TItem> WvpDisplayMultiSelectField { get; set; }

		[Parameter] public RenderFragment<TItem> WvpInputMultiSelectField { get; set; }

		[Parameter] public RenderFragment<TItem> WvpInputMultiSelectOption { get; set; }

		[Parameter] public IEnumerable<TItem> Options { get; set; }

		[Parameter] public IEnumerable<TItem> Value { get; set; }

		[Parameter] public bool EndIsReached { get; set; } = false;

		#endregion

		#region << Callbacks >>

		[Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; } //Fires when user presses enter or input looses focus

		[Parameter] public EventCallback FetchMoreRows { get; set; } 
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private string _inputElementId = "wvp-" + Guid.NewGuid();

		private DotNetObjectReference<WvpInlineMultiSelect<TItem>> _objectReference;

		private IEnumerable<TItem> _originalValue;

		private List<TItem> _value;

		private bool? scheduledEnableEditChange = null;

		private bool? scheduledApplyChange = null;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, Id, "OnEscapeKey");
			}
		}

		public async ValueTask DisposeAsync()
		{
			await new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);

			_objectReference?.Dispose();
		}

		protected override async Task OnInitializedAsync()
		{
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"input-group-{sizeSuffix}");

			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All,TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full };
				jsonSettings.Converters.Insert(0, new PrimitiveJsonConverter());
				var valueJson = JsonConvert.SerializeObject(Value, Formatting.None, jsonSettings);
				_value = JsonConvert.DeserializeObject<IEnumerable<TItem>>(valueJson, jsonSettings).ToList();
			}


			await base.OnParametersSetAsync();
		}
		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

		private async Task _onInputHandler(ChangeEventArgs e)
		{
			await OnInput.InvokeAsync(e);
			//await InvokeAsync(StateHasChanged);
		}

		private async Task _onValueChangeHandler(ChangeEventArgs e)
		{
			_value = ((IEnumerable<TItem>)e.Value).ToList();
			//await InvokeAsync(StateHasChanged);
		}


		private async Task _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{
			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				await Task.Delay(5);
				await new JsService(JSRuntime).FocusElementBySelector("#" + _inputElementId);
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				//Apply Change
				if (applyChange)
				{
					if (JsonConvert.SerializeObject(_value) != JsonConvert.SerializeObject(_originalValue))
					{
						//Update Function should be called
						await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All,TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full };
					jsonSettings.Converters.Insert(0, new PrimitiveJsonConverter());
					_value = JsonConvert.DeserializeObject<IEnumerable<TItem>>(JsonConvert.SerializeObject(_originalValue, Formatting.None, jsonSettings), jsonSettings).ToList();
				}
				_editEnabled = false;
			}
			//await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnEscapeKey()
		{
			await Task.Delay(1);
			if (_editEnabled)
			{
				scheduledEnableEditChange = false;
				scheduledApplyChange = false;
				//OnInput will be called after this because of the blur and it will execute the toggle
			}
		}
		#endregion
	}
}