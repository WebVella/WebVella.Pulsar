using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Linq;
using WebVella.Pulsar.Services;

namespace WebVella.Pulsar.Components
{
	public partial class WvpInlineAutocomplete : WvpInlineBase, IAsyncDisposable
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public IEnumerable<string> Options { get; set; }

		/// <summary>
		/// Method used to filter the Data based on a search string. By default it will apply internal contains func
		/// </summary>
		[Parameter] public Func<IEnumerable<string>, string, IEnumerable<string>> FilterFunc { get; set; }

		/// <summary>
		/// Minimal text input length for triggering autocomplete
		/// </summary>
		[Parameter] public int MinLength { get; set; } = 1;


		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// Current Autocomplete value
		/// </summary>
		[Parameter] public string Value { get; set; } = "";

		#endregion

		#region << Callbacks >>

		[Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; } //Fires on each user input

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private DotNetObjectReference<WvpInlineAutocomplete> _objectReference;

		private string _originalValue = "";

		private string _value = "";

		#endregion

		#region <<Store properties>>

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await JsService.AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, Id, "OnEscapeKey");
			}
			await base.OnAfterRenderAsync(firstRender);
		}

		public async ValueTask DisposeAsync()
		{
			await JsService .RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);

			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override void OnInitialized()
		{
			if (!String.IsNullOrWhiteSpace(Class))
				_cssList.Add(Class);

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"input-group-{sizeSuffix}");

			_originalValue = Value;
			_value = FieldValueService.InitAsString(Value);
		}

		protected override void OnParametersSet()
		{
			if(_originalValue != Value && !_editEnabled)
				_value = FieldValueService.InitAsString(Value);
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << Store methods >>

		#endregion

		#region << Ui handlers >>

		private async Task _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{
			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				await Task.Delay(5);
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				var originalValue = FieldValueService.InitAsString(Value);
				//Apply Change
				if (applyChange)
				{
					if (_value != originalValue)
					{
						//Update Function should be called
						await ValueChanged.InvokeAsync(new ChangeEventArgs { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
				}
				_editEnabled = false;
			}
			await InvokeAsync(StateHasChanged);
		}

		private async Task _onValueChanged(ChangeEventArgs args){
			_value = FieldValueService.InitAsString(args.Value?.ToString());
			await InvokeAsync(StateHasChanged);
	
		}

		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				await _toggleInlineEditClickHandler(false,false);
			}
		}
		#endregion
	}
}