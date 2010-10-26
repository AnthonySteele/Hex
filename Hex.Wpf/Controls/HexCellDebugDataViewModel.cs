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
    using System.Windows;

    public class HexCellDebugDataViewModel : BaseViewModel
    {
        private Visibility onShortestPathForPlayerX = Visibility.Collapsed;
        private Visibility onShortestPathForPlayerY = Visibility.Collapsed;

        private Visibility isMoveForPlayerX = Visibility.Collapsed;
        private Visibility isMoveForPlayerY = Visibility.Collapsed;

        private string moveForPlayerXText;
        private string moveForPlayerYText;

        public Visibility OnShortestPathForPlayerX
        {
            get
            {
                return this.onShortestPathForPlayerX;
            }

            set
            {
                if (this.onShortestPathForPlayerX != value)
                {
                    this.onShortestPathForPlayerX = value;
                    this.OnPropertyChanged("OnShortestPathForPlayerX");
                }
            }
        }

        public Visibility OnShortestPathForPlayerY
        {
            get
            {
                return this.onShortestPathForPlayerY;
            }

            set
            {
                if (this.onShortestPathForPlayerY != value)
                {
                    this.onShortestPathForPlayerY = value;
                    this.OnPropertyChanged("OnShortestPathForPlayerY");
                }
            }
        }

        public Visibility IsMoveForPlayerX
        {
            get
            {
                return this.isMoveForPlayerX;
            }

            set
            {
                if (this.isMoveForPlayerX != value)
                {
                    this.isMoveForPlayerX = value;
                    this.OnPropertyChanged("IsMoveForPlayerX");
                }
            }
        }

        public Visibility IsMoveForPlayerY
        {
            get
            {
                return this.isMoveForPlayerY;
            }

            set
            {
                if (this.isMoveForPlayerY != value)
                {
                    this.isMoveForPlayerY = value;
                    this.OnPropertyChanged("isMoveForPlayerY");
                }
            }
        }

        public string MoveForPlayerXText
        {
            get
            {
                return this.moveForPlayerXText;
            }

            set
            {
                if (this.moveForPlayerXText != value)
                {
                    this.moveForPlayerXText = value;
                    this.OnPropertyChanged("MoveForPlayerXText");
                }
            }
        }

        public string MoveForPlayerYText
        {
            get
            {
                return this.moveForPlayerYText;
            }

            set
            {
                if (this.moveForPlayerYText != value)
                {
                    this.moveForPlayerYText = value;
                    this.OnPropertyChanged("MoveForPlayerYText");
                }
            }
        }
    }
}
