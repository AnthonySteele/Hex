namespace Hex.Engine.Test
{
    using Hex.Board;
    using NUnit.Framework;

    [TestFixture]
    public class MoveScoreConverterTest
    {
        [Test]
        public void WinForPlayerXIsGreaterThanWinForPlayerY()
        {
            int playerXWin = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 0);
            int playerYWin = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 0);

            Assert.Greater(playerXWin, playerYWin);
            Assert.Greater(playerXWin, 0);
            Assert.Less(playerYWin, 0);
        }

        [Test]
        public void NearWinForPlayerXIsGreater()
        {
            int playerXWin = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 1);
            int playerXFarWin = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 2);

            Assert.Greater(playerXWin, playerXFarWin);
            Assert.Greater(playerXFarWin, 0);
            Assert.Greater(playerXWin, 0);
        }

        [Test]
        public void NearWinForPlayerYIsLessNegative()
        {
            int playerYWin = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 1);
            int playerYFarWin = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 2);

            Assert.Less(playerYWin, playerYFarWin);
            Assert.Less(playerYFarWin, 0);
            Assert.Less(playerYWin, 0);
        }

        [Test]
        public void DescribeZeroScore()
        {
            string description = MoveScoreConverter.DescribeScore(0);

            Assert.AreEqual("Score: 0", description);
        }

        [Test]
        public void DescribePositiveScore()
        {
            string description = MoveScoreConverter.DescribeScore(12);

            Assert.AreEqual("X ahead with 12", description);
        }

        [Test]
        public void DescribeNegativeScore()
        {
            string description = MoveScoreConverter.DescribeScore(-13);

            Assert.AreEqual("Y ahead with -13", description);
        }

        [Test]
        public void DescribeXWinner()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 1);
            string description = MoveScoreConverter.DescribeScore(score);
            Assert.AreEqual("Win by X in 1 moves", description);
        }

        [Test]
        public void DescribeYWinner()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 2);
            string description = MoveScoreConverter.DescribeScore(score);
            Assert.AreEqual("Win by Y in 2 moves", description);
        }

        [Test]
        public void IsImmediateWinSuccessForPlayerX()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 0);
            bool result = MoveScoreConverter.IsImmediateWin(Occupied.PlayerX, score);

            Assert.IsTrue(result);
        }

        [Test]
        public void WinForPlayerXIsNotWinForPlayerY()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 0);
            bool result = MoveScoreConverter.IsImmediateWin(Occupied.PlayerY, score);

            Assert.IsFalse(result);
        }

        [Test]
        public void IsImmediateWinSuccessForPlayerY()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 0);
            bool result = MoveScoreConverter.IsImmediateWin(Occupied.PlayerY, score);

            Assert.IsTrue(result);
        }

        [Test]
        public void WinForPlayerYIsNotWinForPlayerX()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 0);
            bool result = MoveScoreConverter.IsImmediateWin(Occupied.PlayerX, score);

            Assert.IsFalse(result);
        }

        [Test]
        public void LaterWinForPLayerXIsNotImmediateWin()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 1);
            bool resultForX = MoveScoreConverter.IsImmediateWin(Occupied.PlayerX, score);
            bool resultForY = MoveScoreConverter.IsImmediateWin(Occupied.PlayerY, score);

            Assert.IsFalse(resultForX, "X");
            Assert.IsFalse(resultForY, "Y");
        }

        [Test]
        public void PositiveScoreIsNotImmediateWin()
        {
            const int Score = 12;
            bool resultForX = MoveScoreConverter.IsImmediateWin(Occupied.PlayerX, Score);
            bool resultForY = MoveScoreConverter.IsImmediateWin(Occupied.PlayerY, Score);

            Assert.IsFalse(resultForX, "X");
            Assert.IsFalse(resultForY, "Y");
        }

        [Test]
        public void NegativeScoreIsNotImmediateWin()
        {
            const int Score = -12;
            bool resultForX = MoveScoreConverter.IsImmediateWin(Occupied.PlayerX, Score);
            bool resultForY = MoveScoreConverter.IsImmediateWin(Occupied.PlayerY, Score);

            Assert.IsFalse(resultForX, "X");
            Assert.IsFalse(resultForY, "Y");
        }

        [Test]
        public void HigherScoreIsBetterForPlayerX()
        {
            const int PlayerXScore = 1;
            const int PlayerYScore = -1;

            Assert.IsTrue(MoveScoreConverter.IsBetterFor(PlayerXScore, PlayerYScore, true));
            Assert.IsFalse(MoveScoreConverter.IsBetterFor(PlayerXScore, PlayerYScore, false));
        }

        [Test]
        public void LowerScoreIsBetterForPlayerX()
        {
            const int PlayerXScore = 1;
            const int PlayerYScore = -1;

            Assert.IsFalse(MoveScoreConverter.IsBetterFor(PlayerYScore, PlayerXScore, true));
            Assert.IsTrue(MoveScoreConverter.IsBetterFor(PlayerYScore, PlayerXScore, false));
        }

        [Test]
        public void WinIsBetterForPlayerX()
        {
            int winScoreX = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 2);
            int winScoreY = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 2);

            Assert.IsTrue(MoveScoreConverter.IsBetterFor(winScoreX, winScoreY, true));
            Assert.IsFalse(MoveScoreConverter.IsBetterFor(winScoreX, winScoreY, false));
        }

        [Test]
        public void WinIsBetterForPlayerY()
        {
            int winScoreX = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 2);
            int winScoreY = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 2);

            Assert.IsTrue(MoveScoreConverter.IsBetterFor(winScoreY, winScoreX, false));
            Assert.IsFalse(MoveScoreConverter.IsBetterFor(winScoreY, winScoreX, true));
        }

        [Test]
        public void EqualScoresAreNotBetterForEitherPlayer()
        {
            const int Score = 0;
            Assert.IsFalse(MoveScoreConverter.IsBetterFor(Score, Score, true));
            Assert.IsFalse(MoveScoreConverter.IsBetterFor(Score, Score, false));
        }

        [Test]
        public void WinnerXIsWinner()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerX, 1);

            Assert.AreEqual(Occupied.PlayerX, MoveScoreConverter.Winner(score));
        }

        [Test]
        public void WinnerYIsWinner()
        {
            int score = MoveScoreConverter.ConvertWin(Occupied.PlayerY, 1);

            Assert.AreEqual(Occupied.PlayerY, MoveScoreConverter.Winner(score));
        }

        [Test]
        public void NoWinner()
        {
            Assert.AreEqual(Occupied.Empty, MoveScoreConverter.Winner(0));
        }
    }
}
