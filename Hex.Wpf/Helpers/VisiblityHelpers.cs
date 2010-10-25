namespace Hex.Wpf.Helpers
{
    using System.Windows;

    public static class VisiblityHelpers
    {
        public static Visibility ToVisibility(this bool value)
        {
            return value ? Visibility.Visible : Visibility.Hidden;
        }

        public static Visibility ToVisibilityCollapsed(this bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
