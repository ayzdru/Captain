using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
    public class SelectListItem
    {
        public string Text { get; set; }
        public Guid Value { get; set; }
    }
}
