using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationWinUI3Example
{
    internal class TabTrackingData
    {
        internal TabViewItem Tab { get; set; }
        internal DateTime CreateTime { get; set; }
        internal TabTrackingData() { }
        internal TabTrackingData(TabViewItem tab) 
        {
            this.Tab = tab;
            this.CreateTime = DateTime.Now;
        }


    }
}
