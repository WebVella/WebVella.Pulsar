using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using WebVella.Pulsar.Services;
using System.Diagnostics;

namespace WebVella.Pulsar.Components
{
	public partial class WvpDropdown : WvpBase, IAsyncDisposable
	{

		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public bool? IsMenuVisible { get; set; } = null;
		[Parameter] public WvpDropDownDirection Mode { get; set; } = WvpDropDownDirection.DropDown;

		#endregion

		#region << Callbacks >>

		[Parameter] public EventCallback<bool> StatusChange { get; set; }

		#endregion

		#region << Private properties >>
		private DotNetObjectReference<WvpDropdown> _objectReference;

		private string _class = "";

		internal bool _isMenuVisible = false;

		private WvpDropDownDirection _mode = WvpDropDownDirection.DropDown;
		#endregion

		#region << Store properties >>
		internal bool StoreIsMenuVisible { get { return _isMenuVisible; } }

		internal WvpDropDownDirection StoreMode { get { return _mode; } }

		internal DotNetObjectReference<WvpDropdown> StoreObjectReference { get { return _objectReference; } }
		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_objectReference = DotNetObjectReference.Create(this);
				await JsService.AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, Id, "OnFocusOut");
				await JsService.AddOutsideClickEventListener($"#{Id}", _objectReference, Id, "OnFocusOut");
			}
			await base.OnAfterRenderAsync(firstRender);
		}

		protected override async Task OnParametersSetAsync()
		{
			if (IsMenuVisible != null)
				_isMenuVisible = IsMenuVisible.Value;
			_mode = Mode;
			var classList = new List<string> { "dropdown  wvp-dropdown" };
			if (!String.IsNullOrWhiteSpace(Class))
				classList.Add(Class);
			classList.Add(_mode.ToDescriptionString());
			_class = String.Join(" ", classList);
			await base.OnParametersSetAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await JsService.RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, Id);
			await JsService.RemoveOutsideClickEventListener($"#{Id}", Id);
			_objectReference?.Dispose();
		}
		#endregion

		#region << Private methods >>

		#endregion

		#region << Store methods >>
		internal Task StoreShowMenu()
		{
			// used to prevent toggle on multiple calls
			if (_isMenuVisible)
				return Task.CompletedTask;

			_isMenuVisible = true;
			StatusChange.InvokeAsync(_isMenuVisible);
			InvokeAsync(StateHasChanged);
			return Task.CompletedTask;
		}

		internal Task StoreHideMenu()
		{
			// used to prevent toggle on multiple calls
			if (!_isMenuVisible)
				return Task.CompletedTask;

			_isMenuVisible = false;
			StatusChange.InvokeAsync(_isMenuVisible);
			InvokeAsync(StateHasChanged);
			return Task.CompletedTask;
		}

		internal Task StoreToggleMenu()
		{
			_isMenuVisible = !_isMenuVisible;
			StatusChange.InvokeAsync(_isMenuVisible);
			InvokeAsync(StateHasChanged);
			return Task.CompletedTask;
		}

		#endregion

		#region << Ui handlers >>
		//All names should start with _
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnFocusOut()
		{
			if (StoreIsMenuVisible)
			{
				await StoreHideMenu();
				await InvokeAsync(StateHasChanged);
			}
		}
		#endregion
	}
}