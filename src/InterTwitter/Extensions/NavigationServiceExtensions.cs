﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterTwitter.Controls;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace InterTwitter.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static async Task<INavigationResult> SelectTabAsync(this INavigationService navigationService, string name, INavigationParameters parameters = null)
        {
            try
            {
                var currentPage = ((IPageAware)navigationService).Page;

                var canNavigate = await PageUtilities.CanNavigateAsync(currentPage, parameters);
                if (!canNavigate)
                    throw new Exception($"IConfirmNavigation for {currentPage} returned false");

                TabbedPage tabbedPage = null;

                if (currentPage is TabbedPage)
                {
                    tabbedPage = currentPage as TabbedPage;
                }
                if (currentPage.Parent is TabbedPage parent)
                {
                    tabbedPage = parent;
                }
                else if (currentPage.Parent is NavigationPage navPage)
                {
                    if (navPage.Parent != null && navPage.Parent is TabbedPage parent2)
                    {
                        tabbedPage = parent2;
                    }
                }

                if (tabbedPage == null)
                    throw new Exception("No parent TabbedPage could be found");

                var tabToSelectedType = PageNavigationRegistry.GetPageType(UriParsingHelper.GetSegmentName(name));
                if (tabToSelectedType is null)
                    throw new Exception($"No View Type has been registered for '{name}'");

                Page target = null;
                foreach (var child in tabbedPage.Children)
                {
                    if (child.GetType() == tabToSelectedType)
                    {
                        target = child;
                        break;
                    }

                    if (child is NavigationPage childNavPage)
                    {
                        if (childNavPage.CurrentPage.GetType() == tabToSelectedType ||
                            childNavPage.RootPage.GetType() == tabToSelectedType)
                        {
                            target = child;
                            break;
                        }
                    }
                }

                if (target is null)
                    throw new Exception($"Could not find a Child Tab for '{name}'");

                var tabParameters = UriParsingHelper.GetSegmentParameters(name, parameters);

                tabbedPage.CurrentPage = target;
                PageUtilities.OnNavigatedFrom(currentPage, tabParameters);
                PageUtilities.OnNavigatedTo(target, tabParameters);
            }
            catch (Exception ex)
            {
                return new NavigationResult { Exception = ex };
            }

            return new NavigationResult { Success = true };
        }

        public static async Task<INavigationResult> SelectTabFromFlyoutAsync(this INavigationService navigationService, string name, INavigationParameters parameters = null)
        {
            try
            {
                var currentPage = ((IPageAware)navigationService).Page as MasterDetailPage;

                var canNavigate = await PageUtilities.CanNavigateAsync(currentPage, parameters);
                if (!canNavigate)
                    throw new Exception($"IConfirmNavigation for {currentPage} returned false");

                TabbedPage tabbedPage = null;

                if (currentPage.Detail is CustomTabbedPage)
                {
                    tabbedPage = currentPage?.Detail as TabbedPage;
                }
                else if (currentPage.Detail is NavigationPage navPage)
                {
                    if (navPage.CurrentPage != null && navPage.CurrentPage is CustomTabbedPage parent2)
                    {
                        tabbedPage = parent2;
                    }
                }


                if (tabbedPage == null)
                    throw new Exception("No parent TabbedPage could be found");


                var tabToSelectedType = PageNavigationRegistry.GetPageType(UriParsingHelper.GetSegmentName(name));
                if (tabToSelectedType is null)
                    throw new Exception($"No View Type has been registered for '{name}'");

                Page target = null;
                foreach (var child in tabbedPage.Children)
                {
                    if (child.GetType() == tabToSelectedType)
                    {
                        target = child;
                        break;
                    }

                    if (child is NavigationPage childNavPage)
                    {
                        if (childNavPage.CurrentPage.GetType() == tabToSelectedType ||
                            childNavPage.RootPage.GetType() == tabToSelectedType)
                        {
                            target = child;
                            break;
                        }
                    }
                }

                if (target is null)
                    throw new Exception($"Could not find a Child Tab for '{name}'");

                var tabParameters = UriParsingHelper.GetSegmentParameters(name, parameters);

                tabbedPage.CurrentPage = target;
                PageUtilities.OnNavigatedFrom(currentPage, tabParameters);
                PageUtilities.OnNavigatedTo(target, tabParameters);
            }
            catch (Exception ex)
            {
                return new NavigationResult { Exception = ex };
            }

            return new NavigationResult { Success = true };
        }



    }
}
