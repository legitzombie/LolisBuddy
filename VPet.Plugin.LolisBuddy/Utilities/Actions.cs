using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public class Actions
    {

    }

    public class ActionEntry
    {
        [Line] public string Name { get; set; }

        [Line] public int Interactions { get; set; }
    }
}
