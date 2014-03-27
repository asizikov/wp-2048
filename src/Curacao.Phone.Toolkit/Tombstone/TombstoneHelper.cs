using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;

namespace Curacao.Phone.Toolkit.Tombstone
{
    [PublicAPI]
    public static class TombstoneHelper
    {
        [PublicAPI]
        public static void RestoreState(this PhoneApplicationPage page, Pivot pivot)
        {
            RoutedEventHandler handler = null;
            var selectedIndex = page.GetPivotSelectedIndex(pivot);
            if (!selectedIndex.HasValue) return;

            handler = (s, e) => pivot.SelectedIndex = selectedIndex.Value;
            pivot.Loaded += handler;
        }

        [PublicAPI]
        public static object RestoreState(this PhoneApplicationPage page, string key)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (page.State.ContainsKey(key))
            {
                return page.State[key];
            }
            return null;
        }

        [PublicAPI]
        public static void RestoreState(this PhoneApplicationPage page, TextBox textBox)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (textBox == null) throw new ArgumentNullException("textBox");
            string key = textBox.Name + "_Text";
            if (page.State.ContainsKey(key))
            {
                textBox.Text = page.State[key].ToString();
            }
        }

        [PublicAPI]
        public static void SaveState(this PhoneApplicationPage page,
            Pivot pivot)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (pivot == null) throw new ArgumentNullException("pivot");
            string key = pivot.Name + "_SelectedIndex";
            if (pivot.SelectedIndex >= 0)
            {
                if (page.State.ContainsKey(key))
                {
                    page.State[key] = pivot.SelectedIndex;
                }
                else
                {
                    page.State.Add(key, pivot.SelectedIndex);
                }
            }
        }

        [PublicAPI]
        public static void SaveState(this PhoneApplicationPage page, ScrollViewer scrollViewer)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (scrollViewer == null) throw new ArgumentNullException("scrollViewer");

            var key = scrollViewer.Name + "_VerticalOffset";
            if (scrollViewer.VerticalOffset > 0.0)
            {
                if (page.State.ContainsKey(key))
                {
                    page.State[key] = scrollViewer.VerticalOffset;
                }
                else
                {
                    page.State.Add(key, scrollViewer.VerticalOffset);
                }
            }
        }

        [PublicAPI]
        public static void SaveState(this PhoneApplicationPage page, TextBox textBox)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (textBox == null) throw new ArgumentNullException("textBox");
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (page.State.ContainsKey(textBox.Name + "_Text"))
                {
                    page.State[textBox.Name + "_Text"] = textBox.Text;
                }
                else
                {
                    page.State.Add(textBox.Name + "_Text", textBox.Text);
                }
            }
        }

        [PublicAPI]
        public static void SaveState(this PhoneApplicationPage page, string key,
            object value)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");
            if (page.State.ContainsKey(key))
            {
                page.State[key] = value;
            }
            else
            {
                page.State.Add(key, value);
            }
        }

        [PublicAPI]
        public static bool ShouldTombstone(this PhoneApplicationPage page,
            NavigatingCancelEventArgs e)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (e == null) throw new ArgumentNullException("e");

            return !e.IsNavigationInitiator;
        }

        [PublicAPI, CanBeNull]
        private static int? GetPivotSelectedIndex(this PhoneApplicationPage page, FrameworkElement pivot)
        {
            if (((page != null) && (pivot != null)) && (page.State != null))
            {
                var key = pivot.Name + "_SelectedIndex";
                if (page.State.ContainsKey(key))
                {
                    return int.Parse(page.State[key].ToString());
                }
            }
            return null;
        }
    }
}
