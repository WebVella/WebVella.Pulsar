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
	public partial class WvpDisplayImage : WvpDisplayBase
	{

		#region << Parameters >>

		/// <summary>
		/// Pattern of accepted string values. Goes with title attribute as description of the pattern
		/// </summary>
		[Parameter] public WvpFileInfo Value { get; set; } = null;

		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private WvpFileInfo _originalValue = null;

		private WvpFileInfo _value = null;

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

			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				if (Value == null)
					_value = null;
				else
					_value = JsonConvert.DeserializeObject<WvpFileInfo>(JsonConvert.SerializeObject(Value));

			}

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