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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Hex.Board;
    using Hex.Engine.PathLength;
    using Hex.Wpf.Helpers;
    using Hex.Wpf.Model;

    public class GetDebugDataCommand : GenericCommand<HexBoardViewModel>
    {
        public override void ExecuteOnValue(HexBoardViewModel value)
        {
            ShowPaths(value);
            ShowBestMove(value);
        }

        private static void ShowPaths(HexBoardViewModel boardViewModel)
        {
            PathLengthBase pathLength = new PathLengthAStar(boardViewModel.Board);

            List<Location> playerXPath = pathLength.GetCleanPath(true);
            List<Location> playerYPath = pathLength.GetCleanPath(false);

            foreach (HexCellViewModel cell in boardViewModel.Cells)
            {
                bool onXPath = playerXPath.Contains(cell.Location);
                cell.DebugData.OnShortestPathForPlayerX = onXPath.ToVisibilityCollapsed();

                bool onYPath = playerYPath.Contains(cell.Location);
                cell.DebugData.OnShortestPathForPlayerY = onYPath.ToVisibilityCollapsed();
            }
        }

        private static void ShowBestMove(HexBoardViewModel value)
        {
            ResetBestMove(value);
            ShowBestMove(value, Occupied.PlayerX);
            ShowBestMove(value, Occupied.PlayerY);
        }

        private static void ShowBestMove(HexBoardViewModel boardViewModel, Occupied player)
        {
            const int MaxSkillLevel = 6;
            for (int skillLevel = 1; skillLevel <= MaxSkillLevel; skillLevel++)
            {
                int currentSkillLevel = skillLevel;
                Action<ComputerMoveData> finishedAction = cmd => ShowBestMoveCompleted(cmd, boardViewModel, currentSkillLevel);

                ComputerMoveCalculator computerMoveCalculatorX = new ComputerMoveCalculator(boardViewModel.Game, finishedAction, skillLevel);
                computerMoveCalculatorX.ForcePlayer = player;
                computerMoveCalculatorX.Move();
            }
        }
        
        private static void ShowBestMoveCompleted(ComputerMoveData bestMoveData, HexBoardViewModel boardViewModel, int moveSkillLevel)
        {
            HexCellViewModel bestMoveCell = boardViewModel.Cells.FirstOrDefault(cell => cell.Location == bestMoveData.Move);
            if (bestMoveCell != null)
            {
                HexCellDebugDataViewModel debugData = bestMoveCell.DebugData;
                if (bestMoveData.PlayerX)
                {
                    debugData.IsMoveForPlayerX = Visibility.Visible;
                    debugData.MoveForPlayerXText = AddSkillText(debugData.MoveForPlayerXText, moveSkillLevel);
                }
                else
                {
                    debugData.IsMoveForPlayerY = Visibility.Visible;
                    debugData.MoveForPlayerYText = AddSkillText(debugData.MoveForPlayerYText, moveSkillLevel);
                }
            }
        }

        private static string AddSkillText(string existingText, int moveSkillLevel)
        {
            if (string.IsNullOrEmpty(existingText))
            {
                return moveSkillLevel.ToString();
            }

            return existingText + "," + moveSkillLevel;
        }

        private static void ResetBestMove(HexBoardViewModel boardViewModel)
        {
            foreach (HexCellViewModel hexCellViewModel in boardViewModel.Cells)
            {
                hexCellViewModel.DebugData.IsMoveForPlayerX = Visibility.Collapsed;
                hexCellViewModel.DebugData.IsMoveForPlayerY = Visibility.Collapsed;
            }
        }
    }
}
