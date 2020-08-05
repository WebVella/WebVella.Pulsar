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
	public partial class WvpInlineCheckboxList<TItem> : WvpInlineBase, IDisposable
	{

		#region << Parameters >>

		[Parameter] public bool IsVertical { get; set; } = false;

		[Parameter] public RenderFragment<TItem> WvpDisplayCheckboxListField { get; set; }

		[Parameter] public RenderFragment<TItem> WvpInputCheckboxListOption { get; set; }

		[Parameter] public IEnumerable<TItem> Options { get { return _options; } set { _options = value; _isDataTouched = true; } }

		[Parameter] public IEnumerable<TItem> Value { get; set; }

		#endregion

		#region << Callbacks >>

		[Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; } //Fires when user presses enter or input looses focus

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private string _inputElementId = "wvp-" + Guid.NewGuid();

		private DotNetObjectReference<WvpInlineCheckboxList<TItem>> _objectReference;

		private IEnumerable<TItem> _options;

		private IEnumerable<TItem> _originalValue;

		private List<TItem> _value;

		private bool? scheduledEnableEditChange = null;

		private bool? scheduledApplyChange = null;

		private bool _isDataTouched = true;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, Id, "OnEscapeKey");
			}
			await base.OnAfterRenderAsync(firstRender);
		}

		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);

			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
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
				_value = JsonConvert.DeserializeObject<IEnumerable<TItem>>(JsonConvert.SerializeObject(Value)).ToList();
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
		}

		private async Task _onValueChangeHandler(ChangeEventArgs e)
		{
			_value = ((IEnumerable<TItem>)e.Value).ToList();
		}


		private async Task _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{
			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				await Task.Delay(5);
				new JsService(JSRuntime).FocusElementBySelector("#" + _inputElementId);
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
					_value = JsonConvert.DeserializeObject<IEnumerable<TItem>>(JsonConvert.SerializeObject(_originalValue)).ToList();
				}
				_editEnabled = false;
			}
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnEscapeKey()
		{
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