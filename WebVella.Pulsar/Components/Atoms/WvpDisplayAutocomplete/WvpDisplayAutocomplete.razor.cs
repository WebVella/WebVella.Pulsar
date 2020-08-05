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
	public partial class WvpDisplayAutocomplete<TItem> : WvpDisplayBase
	{

		#region << Parameters >>

		/// <summary>
		/// Autocomplete data source
		/// </summary>
		[Parameter] public IEnumerable<TItem> Options { get; set; }

		/// <summary>
		/// Method used to get the display field from the data source
		/// </summary>
		[Parameter] public Func<TItem, string> GetTextFunc { get; set; }


		/// <summary>
		/// Method used to get the value field from the data source
		/// </summary>
		[Parameter] public Func<TItem, string> GetValueFunc { get; set; }

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

		private string _valueLabel = null;

		#endregion

		#region << Lifecycle methods >>
		protected override void OnInitialized()
		{
			base.OnInitialized();
		}

		protected override void OnParametersSet()
		{
			_value = FieldValueService.InitAsString(Value);
			_valueLabel = null;

			if (!String.IsNullOrWhiteSpace(_value))
			{
				_valueLabel = _value;
				if (Options != null)
				{
					foreach (var item in Options)
					{
						if (GetValueFunc(item) == _value)
						{
							_valueLabel = GetTextFunc(item);
							break;
						}
					}
				}
			}

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