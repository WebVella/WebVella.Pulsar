using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;
using System;
using WebVella.Pulsar.Services;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;

namespace WebVella.Pulsar.Components
{
	public partial class WvpDisplayDateTime : WvpDisplayBase
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public string Placeholder { get; set; } = "";

		/// <summary>
		/// If empty string - user input will be considered in the user's local timezone. If set to specific timezone, user's input will be considered as provided in the said timezone
		/// </summary>
		[Parameter] public string TimezoneName { get; set; } = "";

		/// <summary>
		/// DateTime in UTC format
		/// </summary>
		[Parameter] public DateTime? Value { get; set; } = null;

		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private TimeZoneInfo _timezone = null;

		private string _value = null;

		#endregion

		#region << Lifecycle methods >>

		protected override async Task OnParametersSetAsync()
		{
			_cssList = new List<string>();
			if (!String.IsNullOrWhiteSpace(Class))
			{
				_cssList.Add(Class);
				if (!Class.Contains("form-control"))
				{//Handle input-group case
					_cssList.Add("form-control-plaintext");
					if (Value == null)
						_cssList.Add("form-control-plaintext--empty");
				}
			}
			else
			{
				_cssList.Add("form-control-plaintext");
				if (Value == null)
					_cssList.Add("form-control-plaintext--empty");
			}

			var sizeSuffix = Size.ToDescriptionString();
			if (!String.IsNullOrWhiteSpace(sizeSuffix))
				_cssList.Add($"form-control-{sizeSuffix}");

			_value = null;
			var date = Value.WvpConvertDateToBrowserLocal(await JsService.GetBrowserUtcOffsetInMinutes(), TimezoneName);
			if (date != null)
				_value = date.Value.ToShortDateString() + " " + date.Value.ToShortTimeString();

			await base.OnParametersSetAsync();
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