using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using WebVella.Pulsar.Services;
using System;

namespace WebVella.Pulsar.Models
{
	public abstract class WvpBase : ComponentBase
	{
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object> AdditionalAttributes  { get; set; } = new Dictionary<string,object>();

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		[Inject] protected JsService JsService { get; set; }

		[Parameter] public string Id { get; set; } = "wvp-" + Guid.NewGuid();

		[Parameter] public string Class { get; set; } = "";

		[Parameter] public CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;

		[Inject] private IStringLocalizerFactory StringLocalizerFactory { get; set; }

		[Parameter] public string CustomResourcesPath { get; set; }

		[Parameter] public string CustomResourcesLocation { get; set; }

		[Parameter] public EventCallback<object> OnClick { get; set; }

		private IStringLocalizer localizer = null;
		
		protected IStringLocalizer WVT { 
			get
			{
				if (localizer == null)
					localizer = GetStringLocalizer();

				return localizer;
			}
		}

		private IStringLocalizer GetStringLocalizer()
		{
			string resourcesLocation = CustomResourcesLocation;
			if (string.IsNullOrWhiteSpace(resourcesLocation))
				resourcesLocation = GetType().Assembly.GetName().Name;

			string resourcesPath = CustomResourcesPath;
			if (string.IsNullOrWhiteSpace(resourcesPath))
				resourcesPath = GetType().FullName;

			return StringLocalizerFactory.Create(resourcesPath, resourcesLocation);
		}


	}

}

