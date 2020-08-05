using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Pulsar.Models
{
    public enum WvpDropDownDirection
    {
        [Description("")]
        DropDown = 0,
        [Description("dropup")]
        DropUp = 1,
        [Description("dropright")]
        DropRight = 2,
        [Description("dropleft")]
        DropLeft = 3,
        [Description("dropdown--right")]
        DropDownRight = 4
    }
}
