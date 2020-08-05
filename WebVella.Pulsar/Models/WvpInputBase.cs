using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components.Forms;

namespace WebVella.Pulsar.Models
{
	public abstract class WvpInputBase : WvpBase
	{

		[CascadingParameter] protected EditContext EditContext { get; set; } = default!;

		[CascadingParameter] protected FieldIdentifier FieldIdentifier { get; set; }

		[Parameter] public string Name { get; set; } = "";

		[Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; } //Fires on each user input

		[Parameter] public bool Required { get; set; } = false;

		[Parameter] public WvpSize Size { get; set; } = WvpSize.Normal;

		[Parameter] public string Title { get; set; } = "";

		[Parameter] public EventCallback<ChangeEventArgs> ValueChanged { get; set; } //Fires when user presses enter or input looses focus

	}

}

