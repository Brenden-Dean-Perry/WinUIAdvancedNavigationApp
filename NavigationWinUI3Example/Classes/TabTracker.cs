using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;

namespace NavigationWinUI3Example
{
    internal class TabTracker
    {
        private List<TabTrackingData> _AllTabs { get; set; } = new List<TabTrackingData>();
        private List<TabTrackingData> _navigationHistory { get; set; } = new List<TabTrackingData>();

        /// <summary>
        /// Class intended to track the navigated tab order to be reference during 
        /// application back navigation.
        /// </summary>
        internal TabTracker() {}

        /// <summary>
        /// Stores tab reference.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="tabPageType"></param>
        internal void StoreTabData(TabViewItem tab, Type tabPageType)
        {
            _AllTabs.Add(new TabTrackingData(tab, tabPageType));
        }

        /// <summary>
        /// Contains tab by page type search
        /// </summary>
        /// <param name="tabPageType"></param>
        /// <returns></returns>
        internal bool ContainsTabByPageType(Type tabPageType)
        {
            return _AllTabs.Where(x => x.TabPageType == tabPageType).ToList().Count > 0;
        }

        /// <summary>
        /// Returns the first GUID from the list of tabs that has a matching page type.
        /// </summary>
        /// <param name="tabPageType"></param>
        /// <returns></returns>
        internal Guid GetFirstTabGUIDByPageType(Type tabPageType)
        {
            return (Guid)_AllTabs.Where(x => x.TabPageType == tabPageType).FirstOrDefault().Tab.Tag;
        }

        /// <summary>
        /// Gets list of tabs with matching GUIDs.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        internal IEnumerable<TabTrackingData> GetTabs(Guid guid)
        {
            foreach (TabTrackingData data in _AllTabs)
            {
                if (((Guid)data.Tab.Tag).Equals(guid) == true)
                {
                    yield return data;
                }
            }
        }

        /// <summary>
        /// Removes tab reference based on GUID.
        /// </summary>
        /// <param name="guid"></param>
        internal void RemoveTab(Guid guid)
        {
            List<TabTrackingData> tabs = GetTabs(guid).ToList();
            foreach(TabTrackingData tab in tabs)
            {
                _AllTabs.Remove(tab);
            }
        }


        /// <summary>
        /// Adds reference to tab instance within tab tracker.
        /// Used when tab is selected to store navigation history.
        /// </summary>
        /// <param name="tab"></param>
        internal void AddTabToNavigationToHistory(TabViewItem tab)
        {
            Guid guid = (Guid)tab.Tag;
            RemoveTabFromHistory(guid);
            if (HistoryContainsTab(guid) == false)
            {
                _navigationHistory.Add(new TabTrackingData(tab, null));
            }
        }

        /// <summary>
        /// Determines if you may navigate back
        /// </summary>
        /// <returns></returns>
        internal bool CanGoBack()
        {
            return _navigationHistory.Count >= 2;
        }

        /// <summary>
        /// Returns GUID of prior tab in the tab tracker.
        /// Used to identify which tab to activate when go back is called.
        /// </summary>
        /// <returns></returns>
        internal Guid GetPriorTabGUID()
        {
            if(CanGoBack() == true)
            {
                int index = _navigationHistory.Count - 1;
                _navigationHistory.RemoveAt(index);
                Guid priorTabId = (Guid)_navigationHistory.LastOrDefault().Tab.Tag;
                return priorTabId;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Determines whether the tab tracker already contains a reference to your tab 
        /// by comparing the GUID stored in the tag if the TabViewItem. Note: you must store
        /// a GUID string in the page tag for this to work!
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        internal bool HistoryContainsTab(Guid guid)
        {
            if (GetTabsFromHistory(guid).Count() >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes tab from nagivation tracker. 
        /// Use when tab is closed to remove from back navigation after disposed.
        /// </summary>
        /// <param name="tab"></param>
        internal void RemoveTabFromHistory(Guid guid)
        {
            if (HistoryContainsTab(guid) == true)
            {
                List<TabTrackingData> tabList = GetTabsFromHistory(guid);
                foreach (TabTrackingData item in tabList)
                {
                    _navigationHistory.Remove(item);
                }
            }
        }

        /// <summary>
        /// Returns list of tabs that have matching GUID in the tab navigation history.
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        private List<TabTrackingData> GetTabsFromHistory(Guid guid)
        {
            return _navigationHistory.Where(x => x.Tab.Tag.Equals(guid) == true).ToList();
        }

    }
}
