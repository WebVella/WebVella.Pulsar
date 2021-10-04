using Microsoft.AspNetCore.Components;
using System;
using System.Timers;
using System.Diagnostics;
using WebVella.Pulsar.Models;
using System.Threading.Tasks;
using WebVella.Pulsar.Services;
using Microsoft.JSInterop;
using Nito.AsyncEx;

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

        private readonly AsyncLock locker = new AsyncLock();
        #endregion

        #region << Lifecycle methods >>

        protected override async Task OnParametersSetAsync()
        {
            if (IgnoreClickOnBackdrop != _isIgnoreClickOnBackdrop)
            {
                _isIgnoreClickOnBackdrop = IgnoreClickOnBackdrop;

            }

            Debug.WriteLine($"new: {IsOpen} | old: {_isOpen} | id: {Id}");
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
            using (await locker.LockAsync())
            {
                _isOpen = true;
                await new JsService(JSRuntime).SetModalOpen();
                await InvokeAsync(StateHasChanged);
                if (invokeCallback)
                {
                    await IsOpenChanged.InvokeAsync(_isOpen);
                }
                await Task.Delay(5);
                if (IsDraggable)
                    await new JsService(JSRuntime).MakeDraggable(Id);
            }
        }
        public async Task _hide(bool invokeCallback = true, bool isBackdrop = false)
        {
            using (await locker.LockAsync())
            {
                if (isBackdrop && _isIgnoreClickOnBackdrop)
                    return;

                _isOpen = false;
                await new JsService(JSRuntime).SetModalClose();
                await InvokeAsync(StateHasChanged);

                if (invokeCallback)
                {
                    await IsOpenChanged.InvokeAsync(_isOpen);
                }
                await Task.Delay(5);
                if (IsDraggable)
                    await new JsService(JSRuntime).RemoveDraggable(Id);
            }
        }

        #endregion

        #region << UI Handlers >>
        #endregion

        #region << JS Callbacks methods >>

        #endregion
    }
}