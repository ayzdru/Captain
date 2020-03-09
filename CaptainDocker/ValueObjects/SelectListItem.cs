using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
    public class SelectListItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
    }
    public class SelectListItem : SelectListItem<Guid>
    {
       
    }
}
