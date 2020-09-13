using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;

namespace WebVella.Pulsar.Services
{
	//Alpha sorted list of methods

	public class JsService
	{
		protected IJSRuntime JSRuntime { get; }

		private int? _browserUtcOffsetInMinutes= null;

		public JsService(IJSRuntime jsRuntime)
		{
			JSRuntime = jsRuntime;
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
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.addBodyClass",
				 className);
		}

		public async ValueTask<bool> AddCKEditor(string elementId, object dotNetReference, string cultureString)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.addCKEditor",
				 elementId,dotNetReference, cultureString);
		}

		public async ValueTask<bool> AddDocumentEventListener(WvpDomEventType eventType, object component, string listenerId, string methodName)
		{
			var eventName = eventType.ToDescriptionString();
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.addDocumentEventListener",
				 eventName, component, listenerId, methodName);
		}

		public async ValueTask<bool> AddFlatPickrDateTime(string elementId, object inputFileElement, string cultureString)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.addFlatPickrDateTime",
				 elementId, inputFileElement, cultureString);
		}

		public async ValueTask<bool> AddFlatPickrDate(string elementId, object inputFileElement, string cultureString)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.addFlatPickrDate",
				 elementId, inputFileElement, cultureString);
		}

		public async ValueTask<bool> AddOutsideClickEventListener(string elementSelector, object component, string listenerId, string methodName)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.addOutsideClickEventListener",
				 elementSelector, component, listenerId, methodName);
		}

		public async ValueTask<bool> AppStart()
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.appStart");
		}

		public async ValueTask<bool> ClearFlatPickrDate(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.clearFlatPickrDate",
				 elementId);
		}


		public async ValueTask<bool> ClearFlatPickrDateTime(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.clearFlatPickrDateTime",
				 elementId);
		}

		public async ValueTask<bool> FocusElement(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.focusElement",
				 elementId);
		}

		public async ValueTask<bool> FocusElementBySelector(string elementSelector)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.focusElementBySelector",
				 elementSelector);
		}

		public async ValueTask<bool> ScrollToElement(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.scrollToElement",
				 elementId);
		}

		public async ValueTask<bool> SetFlatPickrDateChange(string elementId, string dateTimeString)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.setFlatPickrDateChange",
				 elementId, dateTimeString);
		}

		public async ValueTask<bool> SetFlatPickrDateTimeChange(string elementId, string dateTimeString)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.setFlatPickrDateTimeChange",
				 elementId, dateTimeString);
		}

		public async ValueTask<bool> SetElementHtml(string elementId, string html)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.setElementHtml",
				 elementId, html);
		}

		public async ValueTask<bool> InitFileUpload(object inputFileElement, object component)
		{
			return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.initUploadFile", inputFileElement, component);
		}

		public async ValueTask<List<string>> GetSelectedValues(ElementReference elRef)
		{
			return await JSRuntime.InvokeAsync<List<string>>("WebVellaPulsar.getSelectedValues", elRef);
		}

		public async ValueTask<bool> ReloadPage()
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.reloadPage");
		}

		public async ValueTask<bool> RemoveBodyClass(string className)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeBodyClass",
				 className);
		}

		public async ValueTask<bool> RemoveCKEditor(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeCKEditor",
				 elementId);
		}

		public async ValueTask<bool> RemoveDocumentEventListener(WvpDomEventType eventType, string listenerId)
		{
			var eventName = eventType.ToDescriptionString();
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeDocumentEventListener",
				 eventName, listenerId);
		}

		public async ValueTask<bool> RemoveFlatPickrDate(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeFlatPickrDate",
				 elementId);
		}

		public async ValueTask<bool> RemoveFlatPickrDateTime(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeFlatPickrDateTime",
				 elementId);
		}

		public async ValueTask<bool> RemoveOutsideClickEventListener(string elementSelector, string listenerId)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.removeOutsideClickEventListener",
				 elementSelector, listenerId);
		}

		public async ValueTask<bool> SetCKEditorData(string elementId, string content)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.setCKEditorData",
				 elementId,content);
		}

		public async ValueTask<bool> ScrollElement(ElementReference elRef, int x, int y)
		{
			return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.scrollElement", elRef, x, y);
		}

		public async ValueTask<bool> SimulateClick(ElementReference elRef)
		{
			return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.simulateClick", elRef);
		}

		public async ValueTask<bool> SimulateClickById(string elementId)
		{
			return await JSRuntime.InvokeAsync<bool>("WebVellaPulsar.simulateClickById", elementId);
		}

		public async ValueTask<bool> ShowToast(string title, string message, string type, int duration = 3000)
		{
			return await JSRuntime.InvokeAsync<bool>(
				 "WebVellaPulsar.showToast",
				 title, message, type, duration);
		}

	}
}
