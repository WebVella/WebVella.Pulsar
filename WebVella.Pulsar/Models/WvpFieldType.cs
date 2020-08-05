using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Pulsar.Models
{
    public enum WvpFieldType
    {
		[Description("autonumber")]
		AutoNumber = 1,
		[Description("checkbox")]
		Checkbox = 2,
		[Description("currency")]
		Currency = 3,
		[Description("date")]
		Date = 4,
		[Description("datetime")]
		DateTime = 5,
		[Description("email")]
		Email = 6,
		[Description("file")]
		File = 7,
		[Description("html")]
		Html = 8,
		[Description("image")]
		Image = 9,
		[Description("textarea")]
		Textarea = 10,
		[Description("multiselect")]
		MultiSelect = 11,
		[Description("number")]
		Number = 12,
		[Description("password")]
		Password = 13,
		[Description("percent")]
		Percent = 14,
		[Description("phone")]
		Phone = 15,
		[Description("guid")]
		Guid = 16,
		[Description("select")]
		Select = 17,
		[Description("text")]
		Text = 18,
		[Description("url")]
		Url = 19,
		[Description("multiselect-list")]
		MultiSelectList = 20,
		[Description("checkbox-grid")]
		CheckboxGrid = 21,
		[Description("dropdown")]
		Dropdown = 22,
		[Description("checkbox-list")]
		CheckboxList = 23,
		[Description("radio-list")]
		RadioList = 24,
		[Description("typeahead")]
		Typeahead = 25,
		[Description("grid")]
		Grid = 26

	}
}
