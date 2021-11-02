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
			if (_browserUtcOffsetInMinutes == null)
			{
				_browserUtcOffsetInMinutes = await JSRuntime.InvokeAsync<int>("WebVellaPulsar.getTimezoneOffset");
			}

			return _browserUtcOffsetInMinutes.Value;
		}

		public async ValueTask<bool> AddBodyClass(string className)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.addBodyClass",
				className);
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

		public async ValueTask<bool> AddCKEditor(string elementId, object dotNetReference, string cultureString)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
			   "WebVellaPulsar.addCKEditor",
			   elementId, dotNetReference, cultureString);
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

		public async ValueTask<bool> InitializeInfiniteScroll(Guid componentId, DotNetObjectReference<WvpInfiniteScroll> objectRef, string observerTargetId, string observerViewportId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.initInfiniteScroll",
				componentId, objectRef, observerTargetId, observerViewportId);
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

		public async ValueTask<bool> DestroyInfiniteScroll(Guid componentId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
					 "WebVellaPulsar.infiniteScrollDestroy",
					 componentId);
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

		public async ValueTask<bool> CheckIfElementIdVisible(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.checkIfElementIdVisible",
				elementId);
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


		public async ValueTask<bool> AddDocumentEventListener(WvpDomEventType eventType, object component, string listenerId, string methodName)
		{
			try
			{
				var eventName = eventType.ToDescriptionString();
				await JSRuntime.InvokeAsync<bool>(
					"WebVellaPulsar.addDocumentEventListener",
					eventName, component, listenerId, methodName);
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

		public async ValueTask<bool> AddFlatPickrDateTime(string elementId, object inputFileElement, string cultureString)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
					 "WebVellaPulsar.addFlatPickrDateTime",
					 elementId, inputFileElement, cultureString);
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

		public async ValueTask<bool> AddFlatPickrDate(string elementId, object inputFileElement, string cultureString)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.addFlatPickrDate",
				elementId, inputFileElement, cultureString);
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

		public async ValueTask<bool> AddOutsideClickEventListener(string elementSelector, object component, string listenerId, string methodName)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.addOutsideClickEventListener",
				elementSelector, component, listenerId, methodName);
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

		public async ValueTask<bool> AppStart()
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.appStart");
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

		public async ValueTask<bool> UpdateAppStartProgress(string progress)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.updateAppStartProgress", progress);
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

		public async ValueTask<bool> ClearFlatPickrDate(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.clearFlatPickrDate",
				elementId);
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


		public async ValueTask<bool> ClearFlatPickrDateTime(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.clearFlatPickrDateTime",
				elementId);
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

		public async ValueTask<bool> FocusElement(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.focusElement",
				elementId);
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

		public async ValueTask<bool> FocusElementBySelector(string elementSelector)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.focusElementBySelector",
				elementSelector);
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

		public async ValueTask<bool> MakeDraggable(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.makeDraggable",
				elementId);
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

		public async ValueTask<bool> RemoveDraggable(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.removeDraggable",
				elementId);
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

		public async ValueTask<bool> BlurElement(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.blurElement",
				elementId);
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

		public async ValueTask<bool> BlurElementBySelector(string elementSelector)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.blurElementBySelector",
				elementSelector);
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

		public async ValueTask<bool> ScrollToElement(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.scrollToElement",
				elementId);
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

		public async ValueTask<bool> SetFlatPickrDateChange(string elementId, string dateTimeString)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setFlatPickrDateChange",
				elementId, dateTimeString);
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

		public async ValueTask<bool> SetFlatPickrDateTimeChange(string elementId, string dateTimeString)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setFlatPickrDateTimeChange",
				elementId, dateTimeString);
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

		public async ValueTask<bool> SetPageMetaTitle(string title)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setPageMetaTitle",
				title);
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

		public async ValueTask<bool> SetElementHtml(string elementId, string html)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setElementHtml",
				elementId, html);
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

		public async ValueTask<bool> InitFileUpload(object inputFileElement, object component)
		{

			try
			{
				await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.initUploadFile", inputFileElement, component);
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

		public async ValueTask<List<string>> GetSelectedValues(ElementReference elRef)
		{
			try
			{
				await JSRuntime.InvokeAsync<List<string>>("WebVellaPulsar.getSelectedValues", elRef);
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
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.reloadPage");
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

		public async ValueTask<bool> RemoveBodyClass(string className)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
						 "WebVellaPulsar.removeBodyClass",
						 className);
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

		public async ValueTask<bool> RemoveCKEditor(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
						 "WebVellaPulsar.removeCKEditor",
						 elementId);
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

		public async ValueTask<bool> RemoveDocumentEventListener(WvpDomEventType eventType, string listenerId)
		{
			var eventName = eventType.ToDescriptionString();
			try
			{
				await JSRuntime.InvokeAsync<bool>(
					 "WebVellaPulsar.removeDocumentEventListener",
					 eventName, listenerId);
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

		public async ValueTask<bool> RemoveFlatPickrDate(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeFlatPickrDate",
				 elementId);
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

		public async ValueTask<bool> RemoveFlatPickrDateTime(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
					 "WebVellaPulsar.removeFlatPickrDateTime",
					 elementId);
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

		public async ValueTask<bool> RemoveOutsideClickEventListener(string elementSelector, string listenerId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
						 "WebVellaPulsar.removeOutsideClickEventListener",
						 elementSelector, listenerId);
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

		public async ValueTask<bool> SetCKEditorData(string elementId, string content)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setCKEditorData",
				elementId, content);
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

		public async ValueTask<bool> ScrollElement(ElementReference elRef, int x, int y)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.scrollElement", elRef, x, y);
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

		public async ValueTask<bool> SimulateClick(ElementReference elRef)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.simulateClick", elRef);
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

		public async ValueTask<bool> SimulateClickById(string elementId)
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.simulateClickById", elementId);
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

		public async ValueTask<bool> ShowToast(string title, string message, string type, int duration = 3000)
		{
			try
			{
				await Task.Delay(0);
#pragma warning disable 4014
				_ = JSRuntime.InvokeAsync<bool>(
					 "WebVellaPulsar.showToast",
					 title, message, type, duration);
#pragma warning restore 4014
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

		public async ValueTask<bool> SetModalOpen()
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setModalOpen");
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

		public async ValueTask<bool> SetModalClose()
		{
			try
			{
				await JSRuntime.InvokeAsync<bool>(
				"WebVellaPulsar.setModalClose");
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

	}
}
