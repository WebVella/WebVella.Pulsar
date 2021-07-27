using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WebVella.Pulsar.Models
{
	public class WvpKeyDownArgs
	{
		public bool altKey { get; set; } = false;
		public bool ctrlKey { get; set; } = false;
		public bool metaKey { get; set; } = false;
		public bool shiftKey { get; set; } = false;
		public string code { get; set; } = "";
	}
}
