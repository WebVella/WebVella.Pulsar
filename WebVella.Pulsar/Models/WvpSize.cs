using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WebVella.Pulsar.Models
{
	public enum WvpSize
	{
		[Description("")]
		Normal = 0,
		[Description("sm")]
		Small = 1,
		[Description("md")]
		Medium = 2,
		[Description("lg")]
		Large = 3,
		[Description("xs")]
		ExtraSmall = 4,
		[Description("xl")]
		ExtraLarge = 5
	}
}
