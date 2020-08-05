using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components.Forms;

namespace WebVella.Pulsar.Models
{
	public abstract class WvpDisplayBase : WvpBase
	{
		[Parameter] public WvpSize Size { get; set; } = WvpSize.Normal;

		[Parameter] public string Title { get; set; } = "";

		[Parameter] public string ValueEmptyText { get; set; } = "";
	}

}

