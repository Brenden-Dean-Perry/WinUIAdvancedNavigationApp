using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationWinUI3Example
{
    internal class TabHelper
    {
        private TabTracker _TabTracker {  get; set; } = new TabTracker();
        internal TabHelper(){}

        internal TabViewItem CreateNewTab(Type pageType)
        {
            TabViewItem newItem = new TabViewItem();
            newItem.Tag = Guid.NewGuid().ToString();
            newItem.Header = pageType.Name;
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };

            // The content of the tab is often a frame that contains a page, though it could be any UIElement.
            Frame frame = new Frame();
            if (pageType != null)
            {
                frame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
            newItem.Content = frame;

            TabTrackingData tabData = new TabTrackingData(newItem);
            _TabTracker.StoreNavigation(tabData);

            return newItem;
        }
    }
}
