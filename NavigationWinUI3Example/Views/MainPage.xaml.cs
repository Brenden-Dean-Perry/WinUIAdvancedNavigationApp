using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace NavigationWinUI3Example.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string AppName { get; set; }
        private TabViewHelper TabViewHelper { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            AppName = ((App)Application.Current).AppName;
            TabViewHelper = new TabViewHelper(tabView);
        }

        private void NavigationView_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            var selectedItem = (NavigationViewItem)e.SelectedItem;
            if (selectedItem != null && selectedItem.Tag.ToString() == "Extend")
            {
                var newWindow = WindowHelper.CreateWindow();
                var rootPage = new Frame();
                newWindow.Content = rootPage;
                newWindow.Activate();

                // C# code to navigate in the new window
                var targetPageType = typeof(MainPage);
                string targetPageArguments = string.Empty;
                rootPage.Navigate(targetPageType, targetPageArguments);
            }
            else
            {
                Type pageType = GetPageType(selectedItem, e);
                if (pageType != null)
                {
                    TabViewHelper.SmartTabSelect(pageType);
                }
                else
                {
                    ShowDialog_Click(this, null);
                }
            }
        }

        private Type GetPageType(NavigationViewItem navigationViewItem, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.IsSettingsSelected)
            {
                return typeof(Settings);
            }
            string pageName = navigationViewItem.Tag.ToString();
            return Type.GetType(pageName);
        }

        private void NavigationView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)sender;
            if (selectedItem != null && selectedItem.Tag.ToString() != "Extend")
            {
                string pageName = selectedItem.Tag.ToString();
                Type pageType = Type.GetType(pageName);
                if (pageType != null)
                {
                    Guid tabId = TabViewHelper.AddNewTab(pageType);
                    TabViewHelper.SelectTabByGUID(tabId);
                }
            }
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            Guid tabId = TabViewHelper.AddNewTab(typeof(Home));
            TabViewHelper.SelectTabByGUID(tabId);
        }

        private void tabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabViewItem tabItem = (TabViewItem)((TabView)sender).SelectedItem;
            if (tabItem != null)
            {
                TabViewHelper.StoreTabNavigation(tabItem);
            }
        }
        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            TabViewHelper.RemoveTabByGUID((Guid)args.Tab.Tag);
        }

        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (TabViewHelper.CanGoBack() == true)
            {
                TabViewHelper.GoBack();
            }
        }

        private async void ShowDialog_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "404 Error: Page not found.";
            dialog.PrimaryButtonText = "Ok";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = "A page could not be instatiated from the Navigation View item tag.";

            var result = await dialog.ShowAsync();
        }

    }
}
