using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components.Forms;

namespace WebVella.Pulsar.Models
{
	public abstract class WvpInlineBase : WvpBase
	{
		[Parameter] public string Name { get; set; } = "";

		[Parameter] public bool Required { get; set; } = false;

		[Parameter] public WvpSize Size { get; set; } = WvpSize.Normal;

		[Parameter] public string Title { get; set; } = "";

		[Parameter] public EventCallback<ChangeEventArgs> ValueChanged { get; set; } //Fires when user presses enter or input looses focus

		[Parameter] public string ValueEmptyText { get; set; } = "";

		internal bool _editEnabled = false;
	}

}

