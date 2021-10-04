using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Linq;
using WebVella.Pulsar.Services;

namespace WebVella.Pulsar.Components
{
	public partial class WvpDisplayAutocomplete : WvpDisplayBase
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public IEnumerable<string> Options { get; set; }

		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// Current Autocomplete value
		/// </summary>
		[Parameter] public string Value { get; set; } = "";

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _value = null;


		#endregion

		#region << Lifecycle methods >>
		protected override void OnParametersSet()
		{
			_value = FieldValueService.InitAsString(Value);

			base.OnParametersSet();
		}

		#endregion

		#region << Private methods >>


		#endregion

		#region << Ui handlers >>

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}