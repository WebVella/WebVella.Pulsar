using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Utils;

namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldWrapper : WvpFieldBase
	{ 
		#region << Parameters >>
		[Parameter] public RenderFragment Body { get; set; }
		#endregion

		#region << Callbacks >>
		#endregion

		#region << Private properties >>

		private List<string> _cssList = new List<string>();

		private List<string> _inputCssList = new List<string>();

		public List<KeyValuePair<string, string>> _validationErrors = new List<KeyValuePair<string, string>>();

		#endregion

		#region << Lifecycle methods >>

		protected override void OnInitialized()
		{
			//Do some translation
			ValueEmptyText = WVT[ValueEmptyText];
			ValueNoAccessText = WVT[ValueNoAccessText];
			LabelCheckedText = WVT[LabelCheckedText];
			LabelUnCheckedText = WVT[LabelUnCheckedText];

			base.OnInitialized();
		}

		protected override async Task OnParametersSetAsync()
		{
			#region << cssList >>
			{
				_cssList = new List<string>();
				//If the Params are dynamically changed the classes should be recalculated
				if (!string.IsNullOrWhiteSpace(Class))
				{
					_cssList.Add(Class);
				}
				_cssList.Add($"wvp-field--{Mode.ToDescriptionString()}");
				_cssList.Add($"wvp-field--{Type.ToDescriptionString()}");

				if(LabelMode == WvpFieldLabelMode.Horizontal){
					_cssList.Add("row");

					_inputCssList.Add($"col-sm-{12-LabelHorizontalSpan}");

				}



			}
			#endregion

			if(ValidationErrors.Count > 0)
				_validationErrors = ValidationErrors.FindAll(x=> x.Key.ToLowerInvariant() == Name.ToLowerInvariant());

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI Handlers >>

		#endregion

		#region << JS Callbacks methods >>

		#endregion
	}
}