using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;


namespace WebVella.Pulsar.Models
{
	public abstract class WvpFieldBase : WvpBase
	{

		[Parameter]  public string CurrencyCode { get; set; } = "USD";

		[Parameter] public string Description { get; set; } = "";

		[Parameter] public Guid FieldId { get; set; } = Guid.NewGuid();

		[Parameter] public bool IsDisabled { get; set; }

		[Parameter] public bool IsReadonly { get; set; }

		[Parameter] public string LabelHelpText { get; set; } = "";

		[Parameter] public WvpFieldLabelMode LabelMode { get; set; } = WvpFieldLabelMode.Stacked;

		[Parameter] public string LabelText { get; set; } = "";

		[Parameter] public string LabelWarningText { get; set; } = "";

		[Parameter] public string LabelErrorText { get; set; } = "";

		[Parameter] public string LabelCheckedText { get; set; } = "selected";

		[Parameter] public string LabelUnCheckedText { get; set; } = "not selected";

		[Parameter] public int LabelHorizontalSpan { get; set; } = 2;

		[Parameter] public Dictionary<string,string> LinkDictionary { get; set; } = new Dictionary<string,string>();

		[Parameter] public object Max { get; set; }

		[Parameter] public object Min { get; set; }

		[Parameter] public WvpFieldMode Mode { get; set; } = WvpFieldMode.Form;

		[Parameter] public string Name { get; set; } = "";

		[Parameter] public List<WvpSelectOption> Options { get; set; } = new List<WvpSelectOption>();

		[Parameter] public string Pattern { get; set; } = "";

		[Parameter] public string FormatSpecifier { get; set; } = "";

		[Parameter] public string Placeholder { get; set; } = "";

		[Parameter] public bool Required { get; set; } = false;

		[Parameter] public WvpSize Size { get; set; } = WvpSize.Normal;

		[Parameter] public bool SingleFileOnly { get; set; } = true;

		[Parameter] public bool ShowFileList { get; set; } = true;

		[Parameter] public object Step { get; set; }

		[Parameter] public string TimezoneName { get; set; } = "Eastern Standard Time";

		[Parameter] public WvpFieldType Type { get; set; } = WvpFieldType.Text;

		[Parameter] public List<KeyValuePair<string, string>> ValidationErrors { get; set; } = new List<KeyValuePair<string, string>>();

		[Parameter] public object Value { get; set; } = null;

		[Parameter] public EventCallback<ChangeEventArgs> ValueChanged { get; set; }

		[Parameter] public EventCallback<KeyboardEventArgs> OnKeyPress { get; set; }


		[Parameter] public string ValueEmptyText { get; set; } = "no data";

		[Parameter] public string ValueNoAccessText { get; set; } = "access denied";

		[Parameter] public List<WvpSelectOption> Rows { get; set; } = new List<WvpSelectOption>();

		[Parameter] public List<WvpSelectOption> Columns { get; set; } = new List<WvpSelectOption>();

		[Parameter] public WvpFieldAlignmentMode AlignmentMode { get; set; } = WvpFieldAlignmentMode.Stacked;

		[Parameter] public int TypeaheadSearchEnabledItemCount { get; set; } = 5;

		[Parameter] public List<string> StringOptions { get; set; } = new List<string>();

		[Parameter] public int MinCharLimit { get; set; } = 1;
	}

}

