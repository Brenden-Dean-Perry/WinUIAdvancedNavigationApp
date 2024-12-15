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
        internal Type TabPageType { get; set; }
        internal DateTime CreateTime { get; set; } = DateTime.Now;
        internal TabTrackingData() { }
        internal TabTrackingData(TabViewItem tab, Type tabPageType) 
        {
            this.Tab = tab;
            this.TabPageType = tabPageType;
        }


    }
}
