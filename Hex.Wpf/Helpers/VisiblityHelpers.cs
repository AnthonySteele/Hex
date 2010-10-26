//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
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
