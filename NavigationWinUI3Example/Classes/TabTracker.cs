using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationWinUI3Example
{
    internal class TabTracker
    {
        private List<TabTrackingData> _data {  get; set; }
        /// <summary>
        /// Class intended to track the navigated tab order to be reference during 
        /// application back navigation.
        /// </summary>
        internal TabTracker() {}
        internal void StoreNavigation(TabTrackingData data)
        {
            TryRemovePriorNavigationReference(data);
            if (Contains(data.Tab) == false)
            {
                _data.Add(data);
            }
        }

        /// <summary>
        /// Determines whether the tab tracker already contains a reference to your tab 
        /// by comparing the GUID stored in the tag if the TabViewItem. Note: you must store
        /// a GUID string in the page tag for this to work!
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        internal bool Contains(TabViewItem tab)
        {
            if (_data.Where(x => x.Tab.Tag == tab.Tag).Count() >= 1)
            {
                return true;
            }
            return false;
        }

        private void TryRemovePriorNavigationReference(TabTrackingData data)
        {
            if(Contains(data.Tab) == true)
            {
                _data.Remove(data);
            }
        }
    }
}
