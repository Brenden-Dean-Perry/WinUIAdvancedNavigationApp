using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationWinUI3Example
{
    internal class TabViewHelper
    {
        private TabTracker _TabTracker {  get; set; } = new TabTracker();
        private TabView _TabView { get; set; }
        internal TabViewHelper(TabView tabView)
        {
            _TabView = tabView;
        }

        /// <summary>
        /// Tries to navigate to page if exists. Otherwise, a new page is created and selected.
        /// </summary>
        internal void SmartTabSelect(Type pageType)
        {
            if (_TabTracker.ContainsTabByPageType(pageType) == true)
            {
                Guid guid = _TabTracker.GetFirstTabGUIDByPageType(pageType);
                SelectTabByGUID(guid);
            }
            else
            {
                Guid tabId = AddNewTab(pageType);
                SelectTabByGUID(tabId);
            }
        }

        internal Guid AddNewTab(Type pageType)
        {
            TabViewItem newItem = CreateNewPageTab(pageType);
            _TabView.TabItems.Add(newItem);
            return (Guid)newItem.Tag;
        }

        internal void RemoveTabByGUID(Guid guid)
        {
            TabViewItem tab = (TabViewItem)_TabView.TabItems.Where(x => ((TabViewItem)x).Tag.Equals(guid)).FirstOrDefault();
            _TabTracker.RemoveTabFromHistory(guid);
            _TabTracker.RemoveTab(guid);
            _TabView.TabItems.Remove(tab);
        }
        
        internal bool CanGoBack()
        {
            return _TabTracker.CanGoBack();
        }

        internal void GoBack()
        {
            Guid guid = _TabTracker.GetPriorTabGUID();
            SelectTabByGUID(guid);
        }

        internal void SelectTabByGUID(Guid guid)
        {
            TabViewItem selectedTab = (TabViewItem)_TabView.TabItems.Where(x => ((TabViewItem)x).Tag.Equals(guid) == true).FirstOrDefault();
            if (selectedTab != null)
            {
                int index = _TabView.TabItems.IndexOf(selectedTab);
                _TabView.SelectedIndex = index;
                _TabTracker.AddTabToNavigationToHistory(selectedTab);
            }
        }

        internal void StoreTabNavigation(TabViewItem selectedTab)
        {
            _TabTracker.AddTabToNavigationToHistory(selectedTab);
        }

        private TabViewItem CreateNewPageTab(Type pageType)
        {
            TabViewItem newItem = new TabViewItem();
            newItem.Tag = Guid.NewGuid();
            newItem.Header = pageType.Name;
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };

            // The content of the tab is often a frame that contains a page, though it could be any UIElement.
            Frame frame = new Frame();
            if (pageType != null)
            {
                frame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
            newItem.Content = frame;
            _TabTracker.StoreTabData(newItem, pageType);
            return newItem;
        }
    }
}
