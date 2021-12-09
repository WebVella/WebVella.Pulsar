using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebVella.Pulsar.Components;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;

namespace WebVella.Pulsar.Services
{
    //Alpha sorted list of methods

    public class JsService : IAsyncDisposable
    {
        protected IJSRuntime JSRuntime { get; }

        private int? _browserUtcOffsetInMinutes = null;

        public JsService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                //Dispose all listeners and objects
                await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.dispose");
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return;
        }

        public async ValueTask<int> GetBrowserUtcOffsetInMinutes()
        {
            try
            {

                if (_browserUtcOffsetInMinutes == null)
                {
                    _browserUtcOffsetInMinutes = await JSRuntime.InvokeAsync<int>("WebVellaPulsar.getTimezoneOffset");
                }

                return _browserUtcOffsetInMinutes.Value;
            }
            catch (JSDisconnectedException)
            { 
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return 0;
        }

        public async ValueTask<bool> AddBodyClass(string className)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.addBodyClass",
                className);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> AddCKEditor(string elementId, object dotNetReference, string cultureString)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
               "WebVellaPulsar.addCKEditor",
               elementId, dotNetReference, cultureString);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> InitializeInfiniteScroll(Guid componentId, DotNetObjectReference<WvpInfiniteScroll> objectRef, string observerTargetId, string observerViewportId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.initInfiniteScroll",
                componentId, objectRef, observerTargetId, observerViewportId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> DestroyInfiniteScroll(Guid componentId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                     "WebVellaPulsar.infiniteScrollDestroy",
                     componentId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> InitializeObservedItem(Guid componentId, DotNetObjectReference<WvpObservedItem> objectRef, string observerTargetId, string observerViewportId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.observedItemInit",
                componentId, objectRef, observerTargetId, observerViewportId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            catch { }
            return false;
        }

        public async ValueTask<bool> DestroyObservedItem(Guid componentId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                     "WebVellaPulsar.observedItemDestroy",
                     componentId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            catch { }
            return false;
        }

        public async ValueTask<bool> CheckIfElementIdVisible(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.checkIfElementIdVisible",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }


        public async ValueTask<bool> AddDocumentEventListener(WvpDomEventType eventType, object component, string listenerId, string methodName)
        {
            try
            {
                var eventName = eventType.ToDescriptionString();
                return await JSRuntime.InvokeAsync<bool>(
                    "WebVellaPulsar.addDocumentEventListener",
                    eventName, component, listenerId, methodName);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            catch { 
                //This is causing troubles with multiselects    
            }
            return false;
        }

        public async ValueTask<bool> AddFlatPickrDateTime(string elementId, object inputFileElement, string cultureString)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                     "WebVellaPulsar.addFlatPickrDateTime",
                     elementId, inputFileElement, cultureString);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> AddFlatPickrDate(string elementId, object inputFileElement, string cultureString)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.addFlatPickrDate",
                elementId, inputFileElement, cultureString);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> AddOutsideClickEventListener(string elementSelector, object component, string listenerId, string methodName)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.addOutsideClickEventListener",
                elementSelector, component, listenerId, methodName);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            catch { }
            return false;
        }

        public async ValueTask<bool> AppStart()
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.appStart");
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> UpdateAppStartProgress(string progress)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.updateAppStartProgress", progress);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> ClearFlatPickrDate(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.clearFlatPickrDate",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }


        public async ValueTask<bool> ClearFlatPickrDateTime(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.clearFlatPickrDateTime",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> FocusElement(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.focusElement",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> FocusElementBySelector(string elementSelector)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.focusElementBySelector",
                elementSelector);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> MakeDraggable(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.makeDraggable",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveDraggable(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.removeDraggable",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> BlurElement(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.blurElement",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> BlurElementBySelector(string elementSelector)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.blurElementBySelector",
                elementSelector);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> ScrollToElement(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.scrollToElement",
                elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SetFlatPickrDateChange(string elementId, string dateTimeString)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setFlatPickrDateChange",
                elementId, dateTimeString);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SetFlatPickrDateTimeChange(string elementId, string dateTimeString)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setFlatPickrDateTimeChange",
                elementId, dateTimeString);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SetPageMetaTitle(string title)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setPageMetaTitle",
                title);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SetElementHtml(string elementId, string html)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setElementHtml",
                elementId, html);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> InitFileUpload(object inputFileElement, object component)
        {

            try
            {
                return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.initUploadFile", inputFileElement, component);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<List<string>> GetSelectedValues(ElementReference elRef)
        {
            try
            {
                return await JSRuntime.InvokeAsync<List<string>>("WebVellaPulsar.getSelectedValues", elRef);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return new List<string>();
        }

        public async ValueTask<bool> ReloadPage()
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.reloadPage");
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveBodyClass(string className)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                         "WebVellaPulsar.removeBodyClass",
                         className);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveCKEditor(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                         "WebVellaPulsar.removeCKEditor",
                         elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveDocumentEventListener(WvpDomEventType eventType, string listenerId)
        {
            var eventName = eventType.ToDescriptionString();
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                     "WebVellaPulsar.removeDocumentEventListener",
                     eventName, listenerId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveFlatPickrDate(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                 "WebVellaPulsar.removeFlatPickrDate",
                 elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveFlatPickrDateTime(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                     "WebVellaPulsar.removeFlatPickrDateTime",
                     elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> RemoveOutsideClickEventListener(string elementSelector, string listenerId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                         "WebVellaPulsar.removeOutsideClickEventListener",
                         elementSelector, listenerId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SetCKEditorData(string elementId, string content)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setCKEditorData",
                elementId, content);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> ScrollElement(ElementReference elRef, int x, int y)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.scrollElement", elRef, x, y);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SimulateClick(ElementReference elRef)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.simulateClick", elRef);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SimulateClickById(string elementId)
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.simulateClickById", elementId);
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }
#pragma warning disable 1998
        public async ValueTask<bool> ShowToast(string title, string message, string type, int duration = 3000)
        {
            try
            {
#pragma warning disable 4014
                _ = JSRuntime.InvokeAsync<bool>(
                     "WebVellaPulsar.showToast",
                     title, message, type, duration);
#pragma warning restore 4014
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return true;
        }
#pragma warning restore 1998
        public async ValueTask<bool> SetModalOpen()
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setModalOpen");
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

        public async ValueTask<bool> SetModalClose()
        {
            try
            {
                return await JSRuntime.InvokeAsync<bool>(
                "WebVellaPulsar.setModalClose");
            }
            catch (JSDisconnectedException)
            {
            }
            catch (OperationCanceledException) // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations.
                // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
            }
            return false;
        }

    }
}
