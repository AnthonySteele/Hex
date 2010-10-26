//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Wpf.SelectGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Hex.Wpf.Helpers;
    using Hex.Wpf.Model;

    public class SelectGameViewModel : BaseViewModel
    {
        private readonly ObservableCollection<int> allowedBoardSizes = new ObservableCollection<int>
            {
                5,
                6,
                7, 
                8,
                9,
                10,
                11,
                12
            };

        private readonly ActionCommand<SelectGameViewModel> successCommand;
        private readonly ActionCommand<SelectGameViewModel> cancelCommand;

        private int selectedBoardSize = 7;
        private GameType gameType;

        public SelectGameViewModel(Action<SelectGameViewModel> successAction, Action<SelectGameViewModel> cancelAction)
        {
            this.successCommand = new ActionCommand<SelectGameViewModel>(successAction);
            this.cancelCommand = new ActionCommand<SelectGameViewModel>(cancelAction);
            this.SelectedBoardSize = 7;
            this.HumanVersusComputer = true;
            this.SkillLevel = ComputerSkillLevel.Medium;
        }

        public int SelectedBoardSize
        {
            get
            {
                return this.selectedBoardSize;
            }

            set
            {
                if (this.selectedBoardSize != value)
                {
                    this.selectedBoardSize = value;
                    this.EnableOk();
                    this.OnPropertyChanged("SelectedBoardSize");
                }
            }
        }

        public ObservableCollection<int> AllowedBoardSizes
        {
            get { return this.allowedBoardSizes; }
        }

        public GameType GameType
        {
            get
            {
                return this.gameType;
            }

            set
            {
                if (this.gameType != value)
                {
                    this.gameType = value;
                    this.EnableOk();
                    this.OnPropertyChanged("GameType");
                }
            }
        }

        public bool ComputerVersusHuman
        {
            get
            {
                return this.gameType == GameType.ComputerVersusHuman;
            }

            set
            {
                if (value != this.ComputerVersusHuman)
                {
                    this.GameType = value ? GameType.ComputerVersusHuman : GameType.Unknown;
                    this.OnPropertyChanged("ComputerVersusHuman");
                }
            }
        }

        public bool HumanVersusComputer
        {
            get
            {
                return this.gameType == GameType.HumanVersusComputer;
            }

            set
            {
                if (value != this.HumanVersusComputer)
                {
                    this.GameType = value ? GameType.HumanVersusComputer : GameType.Unknown;
                    this.OnPropertyChanged("HumanVersusComputer");
                }
            }
        }

        public bool HumanVersusHuman
        {
            get
            {
                return this.gameType == GameType.HumanVersusHuman;
            }

            set
            {
                if (value != this.HumanVersusHuman)
                {
                    this.GameType = value ? GameType.HumanVersusHuman : GameType.Unknown;
                    this.OnPropertyChanged("HumanVersusHuman");
                }
            }
        }

        public bool SkillLowChecked
        {
            get
            {
                return this.SkillLevel == SelectGame.ComputerSkillLevel.Low;
            }

            set
            {
                if (value != this.SkillLowChecked)
                {
                    this.SkillLevel = value ? ComputerSkillLevel.Low : ComputerSkillLevel.Unknown;
                    this.OnPropertyChanged("SkillLowChecked");
                }
            }
        }

        public bool SkillMediumChecked
        {
            get
            {
                return this.SkillLevel == SelectGame.ComputerSkillLevel.Medium;
            }

            set
            {
                if (value != this.SkillMediumChecked)
                {
                    this.SkillLevel = value ? ComputerSkillLevel.Medium : ComputerSkillLevel.Unknown;
                    this.OnPropertyChanged("SkillMediumChecked");
                }
            }
        }

        public bool SkillGoodChecked
        {
            get
            {
                return this.SkillLevel == SelectGame.ComputerSkillLevel.Good;
            }

            set
            {
                if (value != this.SkillGoodChecked)
                {
                    this.SkillLevel = value ? ComputerSkillLevel.Good : ComputerSkillLevel.Unknown;
                    this.OnPropertyChanged("SkillGoodChecked");
                }
            }
        }

        public bool SkillExcellentChecked
        {
            get
            {
                return this.SkillLevel == SelectGame.ComputerSkillLevel.Excellent;
            }

            set
            {
                if (value != this.SkillExcellentChecked)
                {
                    this.SkillLevel = value ? ComputerSkillLevel.Excellent : ComputerSkillLevel.Unknown;
                    this.OnPropertyChanged("SkillExcellentChecked");
                }
            }
        }

        public ComputerSkillLevel SkillLevel { get; set; }

        public ICommand SuccessCommand
        {
            get { return this.successCommand; }
        }

        public ICommand CancelCommand
        {
            get { return this.cancelCommand; }
        }

        private void EnableOk()
        {
            this.successCommand.Enabled = this.IsOk();
        }

        private bool IsOk()
        {
            return this.selectedBoardSize > 0 && this.gameType != GameType.Unknown;
        }
    }
}
