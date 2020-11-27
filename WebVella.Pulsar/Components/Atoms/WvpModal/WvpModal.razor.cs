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
	public partial class WvpModal : WvpBase, IDisposable
	{
		#region << Parameters >>
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public bool IsOpen { get; set; } = false;
		[Parameter] public bool IgnoreClickOnBackdrop { get; set; } = false;
		[Parameter] public WvpSize Size { get; set; } = WvpSize.Normal;
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>
		private bool _isOpen { get; set; } = false;
		private string _sizeClass { get; set; } = "";
		private bool _isIgnoreClickOnBackdrop { get; set; } = false;

		private DotNetObjectReference<WvpModal> _objectReference;
		#endregion

		#region << Lifecycle methods >>

		void IDisposable.Dispose()
		{
			Task.Run(async () =>
			{
				await JsService.RemoveOutsideClickEventListener($"#{Id}", Id);
			});
		}

		protected override async Task OnInitializedAsync()
		{
			_objectReference = DotNetObjectReference.Create(this);
			await JsService.AddOutsideClickEventListener($"#{Id}", _objectReference, Id, "OnFocusOut");
			base.OnInitialized();
		}

		protected override async Task OnParametersSetAsync()
		{

			if (IsOpen != _isOpen)
			{
				if (IsOpen)
					await _show();
				else
					await _hide();
			}

			if (IgnoreClickOnBackdrop != _isIgnoreClickOnBackdrop)
			{
				_isIgnoreClickOnBackdrop = IgnoreClickOnBackdrop ? true : false;

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
		private async Task _show()
		{
			await new JsService(JSRuntime).AddBodyClass("modal-open");
			_isOpen = true;
			await InvokeAsync(StateHasChanged);
		}
		public async Task _hide()
		{
			await new JsService(JSRuntime).RemoveBodyClass("modal-open");
			_isOpen = false;
			await InvokeAsync(StateHasChanged);
		}

		#endregion

		#region << UI Handlers >>
		#endregion

		#region << JS Callbacks methods >>
		[JSInvokable]
		public async Task OnFocusOut()
		{
			if (!_isIgnoreClickOnBackdrop)
			{
				await _hide();
			}
		}
		#endregion
	}
}