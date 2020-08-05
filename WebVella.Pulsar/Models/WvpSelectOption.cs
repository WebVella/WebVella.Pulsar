using System;
using System.Collections.Generic;
using System.Text;


namespace WebVella.Pulsar.Models
{
	public class WvpSelectOption
	{
		public string Value { get; set; } = "";

		public string Label { get; set; } = "";

		public string IconClass { get; set; } = "";

		public string Color { get; set; } = "";

		public WvpSelectOption()
		{

		}

		public WvpSelectOption(string value, string label)
		{
			Value = value;
			Label = label;
		}

		public WvpSelectOption(string value, string label, string iconClass, string color)
		{
			Value = value;
			Label = label;
			IconClass = iconClass;
			Color = color;
		}

		public WvpSelectOption(WvpSelectOption option) : this(option.Value, option.Label)
		{
		}

	}
}
