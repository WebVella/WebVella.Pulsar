using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Pulsar.Models
{
	public enum WvpDomEventType
	{
		[Description("mousedown")]
		MouseDown = 1,
		[Description("keydown-escape")]
		KeydownEscape = 2,
		[Description("keydown-enter")]
		KeydownEnter = 3,
		[Description("mousedown-non-dropdown")]
		MouseDownNonDropdown = 4,
		[Description("mousedown-non-modal")]
		MouseDownNonModal = 5,
		[Description("keydown")]
		Keydown = 6
	}
}
