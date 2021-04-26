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
	public partial class WvpDisplaySelect<TItem> : WvpDisplayBase
	{

		#region << Parameters >>

		[Parameter] public RenderFragment<TItem> WvpDisplaySelectField { get; set; }

		[Parameter] public TItem Value { get; set; }

		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private TItem _value;

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

			var jsonSettings = new JsonSerializerSettings { 
				TypeNameHandling = TypeNameHandling.All,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
			};
			jsonSettings.Converters.Insert(0, new PrimitiveJsonConverter());
			_value = JsonConvert.DeserializeObject<TItem>(JsonConvert.SerializeObject(Value, Formatting.None, jsonSettings), jsonSettings);
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