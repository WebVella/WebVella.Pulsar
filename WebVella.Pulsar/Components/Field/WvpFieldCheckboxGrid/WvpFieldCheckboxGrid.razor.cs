using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebVella.Pulsar.Models;
using WebVella.Pulsar.Services;
using WebVella.Pulsar.Utils;


namespace WebVella.Pulsar.Components
{
	public partial class WvpFieldCheckboxGrid : WvpFieldBase, IDisposable
	{

		#region << Parameters >>
		#endregion

		#region << Callbacks >>

		#endregion

		#region << Private properties >>

		private string _domElementId = "";

		private DotNetObjectReference<WvpFieldCheckboxGrid> _objectReference;

		private bool _editEnabled = false;

		private List<WvpKeyStringList> _value = new List<WvpKeyStringList>();

		private Guid _originalFieldId;

		private object _originalValue;

		bool _allColumnsWithEmptyLabels = true;
		bool _allRowsWithEmptyLabels = true;

		#endregion

		#region << Lifecycle methods >>
		void IDisposable.Dispose()
		{
			new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
			new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domElementId}", FieldId.ToString());
			if (_objectReference != null)
			{
				_objectReference.Dispose();
				_objectReference = null;
			}
		}

		protected override async Task OnParametersSetAsync()
		{
			if (JsonConvert.SerializeObject(_originalValue) != JsonConvert.SerializeObject(Value))
			{
				_originalValue = Value;
				if (Value != null)
				{
					if (Value is List<WvpKeyStringList>)
						_value = (List<WvpKeyStringList>)Value;

					if (Value is string)
						_value = JsonConvert.DeserializeObject<List<WvpKeyStringList>>((string)Value);

					foreach (var column in Columns)
					{
						if (!String.IsNullOrWhiteSpace(column.Label))
						{
							_allColumnsWithEmptyLabels = false;
							break;
						}
					}

					foreach (var row in Rows)
					{
						if (!String.IsNullOrWhiteSpace(row.Label))
						{
							_allRowsWithEmptyLabels = false;
							break;
						}
					}
				}

			}
			if (_originalFieldId != FieldId)
			{
				_originalFieldId = FieldId;
				_domElementId = "wvp-field-checkbox-grid-" + FieldId;
			}

			await base.OnParametersSetAsync();
		}

		#endregion

		#region << Private methods >>

		#endregion

		#region << UI handlers >>
		private void _onInputEvent(ChangeEventArgs e, WvpSelectOption row, WvpSelectOption column)
		{
			string _row = row.Value;
			string _column = column.Value;
			bool isChecked = (bool)e.Value;

			if (_value == null)
				_value = new List<WvpKeyStringList>();

			var rowValuesIndex = _value.FindIndex(x => x.Key == _row);
			if (rowValuesIndex == -1)
			{
				_value.Add(new WvpKeyStringList() { Key = _row, Values = new List<string>() });
				rowValuesIndex = _value.FindIndex(x => x.Key == _row);
			}

			List<string> rowValues = _value[rowValuesIndex].Values;

			if (isChecked && !rowValues.Contains(_column))
				_value[rowValuesIndex].Values.Add(_column);

			if (!isChecked && rowValues.Contains(_column))
				_value[rowValuesIndex].Values.Remove(_column);

			if (Mode == WvpFieldMode.Form)
			{
				ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
			}
		}

		private void _toggleInlineEditClickHandler(bool enableEdit, bool applyChange)
		{

			//Show Edit
			if (enableEdit && !_editEnabled)
			{
				_editEnabled = true;
				new JsService(JSRuntime).AddDocumentEventListener(WvpDomEventType.KeydownEscape, _objectReference, FieldId.ToString(), "OnEscapeKey");
				new JsService(JSRuntime).AddOutsideClickEventListener($"#{_domElementId}", _objectReference, FieldId.ToString(), "OnFocusOutClick");
			}

			//Hide edit
			if (!enableEdit && _editEnabled)
			{
				var originalValue = (List<WvpKeyStringList>)Value;
				//Apply Change
				if (applyChange)
				{
					if (JsonConvert.SerializeObject(_value) != JsonConvert.SerializeObject(originalValue))
					{
						//Update Function should be called
						ValueChanged.InvokeAsync(new ChangeEventArgs() { Value = _value });
					}
				}
				//Abandon change
				else
				{
					_value = originalValue;
				}
				_editEnabled = false;
				new JsService(JSRuntime).RemoveDocumentEventListener(WvpDomEventType.KeydownEscape, FieldId.ToString());
				new JsService(JSRuntime).RemoveOutsideClickEventListener($"#{_domElementId}", FieldId.ToString());
			}

		}


		#endregion

		#region << JS Callbacks methods >>

		[JSInvokable]
		public async Task OnEscapeKey()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, false);
				StateHasChanged();
			}
		}

		[JSInvokable]
		public async Task OnFocusOutClick()
		{
			if (_editEnabled)
			{
				_toggleInlineEditClickHandler(false, true);
				StateHasChanged();
			}
		}
		#endregion
	}
}