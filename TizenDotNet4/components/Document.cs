using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TizenDotNet4.components
{
    public class Document
    {
        public string _key { get; set; }
        public string _id { get; set; }
        public string _rev { get; set; }
        public string ProcessStep { get; set; }
        public string MeasuredValue { get; set; }
    }
}
