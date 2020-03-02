using CaptainDocker.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Extensions
{
    public static class ComboBoxExtension
    {
        public static void SelectById(this ComboBox comboBox, Guid id)
        {
            int index = 0;
            foreach (SelectListItem item in comboBox.Items)
            {
                if (item.Value == id)
                {
                    comboBox.SelectedIndex = index;
                    break;
                }
                index++;
            }
        }
    }
}
