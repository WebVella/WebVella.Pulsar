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
	public partial class WvpDisplayCheckbox : WvpDisplayBase, IDisposable
	{

		#region << Parameters >>

		[Parameter] public bool? Value { get; set; } = false;

		[Parameter] public string Label { get; set; } = "checked";

		[Parameter] public bool ShowIcon { get; set; } = false;

		[Parameter] public string LabelUnknown { get; set; } = "unknown";

		[Parameter] public string LabelNotChecked { get; set; } = "not checked";

		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private DotNetObjectReference<WvpDisplayCheckbox> _objectReference;

		private bool? _value = false;

		#endregion

		#region << Lifecycle methods >>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
				_objectReference = DotNetObjectReference.Create(this);
			await base.OnAfterRenderAsync(firstRender);
		}

		void IDisposable.Dispose()
		{
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnInitializedAsync()
		{
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


			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			_value = FieldValueService.InitAsNullBool(Value);
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