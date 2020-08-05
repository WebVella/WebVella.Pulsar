using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Pulsar.Models
{
    public enum WvpFieldMode
    {
        [Description("display")]
        Display = 0,
        [Description("form")]
        Form = 1,
        [Description("inline-edit")]
        InlineEdit = 2,
        [Description("print")]
        Print = 3
    }
}
