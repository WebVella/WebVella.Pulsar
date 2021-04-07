using Microsoft.AspNetCore.Components;
using System;
using System.Timers;
using System.Diagnostics;
using WebVella.Pulsar.Models;
using System.Threading.Tasks;
using WebVella.Pulsar.Services;
using Microsoft.JSInterop;

namespace WebVella.Pulsar.Components
{
	public partial class WvpModal : WvpBase
	{
		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public bool IsOpen { get; set; } = false;
		[Parameter] public bool IgnoreClickOnBackdrop { get; set; } = false;

		[Parameter] public bool IsDraggable { get; set; } = false;
		[Parameter] public WvpSize Size { get; set; } = WvpSize.Normal;

		[Parameter] public string DialogClass { get; set; } = "";
		#endregion

		#region << Callbacks >>
		[Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
		#endregion

		#region << Private properties >>
		private bool _isOpen { get; set; } = false;
		private string _sizeClass { get; set; } = "";
		private bool _isIgnoreClickOnBackdrop { get; set; } = false;

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnParametersSetAsync()
		{
			if (IgnoreClickOnBackdrop != _isIgnoreClickOnBackdrop)
			{
				_isIgnoreClickOnBackdrop = IgnoreClickOnBackdrop ? true : false;

			}

			if (IsOpen != _isOpen)
			{
				if (IsOpen)
					await _show(false);
				else
					await _hide(false);
			}

			switch (Size)
			{
				case WvpSize.Small:
					_sizeClass = "modal-sm";
					break;
				case WvpSize.Large:
					_sizeClass = "modal-lg";
					break;
				case WvpSize.ExtraLarge:
					_sizeClass = "modal-xl";
					break;
				default:
					_sizeClass = "";
					break;
			}
		}

		#endregion

		#region << Private methods >>
		private async Task _show(bool invokeCallback = true)
		{
			_isOpen = true;
			await InvokeAsync(StateHasChanged);
			if (invokeCallback)
			{
				await IsOpenChanged.InvokeAsync(_isOpen);
			}
			await Task.Delay(5);
			if(IsDraggable)
				await new JsService(JSRuntime).MakeDraggable(Id);
		}
		public async Task _hide(bool invokeCallback = true, bool isBackdrop = false)
		{
			if (isBackdrop && _isIgnoreClickOnBackdrop)
				return;

			_isOpen = false;
			await InvokeAsync(StateHasChanged);

			if (invokeCallback)
			{
				await IsOpenChanged.InvokeAsync(_isOpen);
			}
		}

		#endregion

		#region << UI Handlers >>
		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}