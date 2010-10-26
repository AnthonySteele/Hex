//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Hex.Board;

    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Played", GroupName = "CommonStates")]
    public class HexCell : Control
    {
        static HexCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexCell), new FrameworkPropertyMetadata(typeof(HexCell)));
        }

        public HexCell()
        {
            this.DataContextChanged += this.OnDataContextChanged;
        }

        public HexCellViewModel ViewModel
        {
            get { return (HexCellViewModel)this.DataContext; }
        }

        public override void OnApplyTemplate()
        {
            FrameworkElement focusPart = GetTemplateChild("FocusPart") as FrameworkElement;
            if (focusPart != null)
            {
                focusPart.MouseEnter += this.HexCellMouseEnter;
                focusPart.MouseLeave += this.HexCellMouseLeave;
                focusPart.MouseLeftButtonUp += this.HexCellMouseClick;
            }

            this.GoToStateForOcccupied();
        }
        
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            INotifyPropertyChanged oldValue = e.OldValue as INotifyPropertyChanged;
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= this.OnViewModelPropertyChanged;
            }

            INotifyPropertyChanged newValue = e.NewValue as INotifyPropertyChanged;
            if (newValue != null)
            {
                newValue.PropertyChanged += this.OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Occupied")
            {
                this.GoToStateForOcccupied();
            }
        }
        
        private bool IsEmpty()
        {
            return this.ViewModel.Occupied == Occupied.Empty;
        }

        private bool CanPlay()
        {
            return this.IsEmpty() && this.ViewModel.NextMoveIsHuman()  && this.ViewModel.CanPlay;
        }
        
        private void HexCellMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.CanPlay())
            {
                this.ViewModel.EnterCell();
                VisualStateManager.GoToState(this, "MouseOver", true);
            }
        }

        private void HexCellMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.CanPlay())
            {
                this.ViewModel.LeaveCell();
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        private void HexCellMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.CanPlay())
            {
                this.PlayCell();
            }
        }

        private void PlayCell()
        {
            this.ViewModel.PlayCell();
            VisualStateManager.GoToState(this, "Played", true);
        }

        private void GoToStateForOcccupied()
        {
            if (this.ViewModel.Occupied == Occupied.Empty)
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Played", true);
            }
        }
    }
}
