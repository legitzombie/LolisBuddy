using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public class Foods
    {

    }

    public class FoodEntry
    {
        [Line] public string Name { get; set; }
        [Line] public int Eaten { get; set; }
    }
}
