using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class TicTacToeLogicTests
    {
        GameControllerLogic gameControllerLogic = new GameControllerLogic();

        // A Test behaves as an ordinary method
        [Test]
        public void WinOrLose()
        {
            char[,] boardFirstRowWinX = {{ 'x', 'x', 'x' },
                                         { '_', '_', '_' },
                                         { '_', '_', '_' }};

            char[,] boardSecondRowWinX = {{ '_', '_', '_' },
                                         { 'x', 'x', 'x' },
                                         { '_', '_', '_' }};

            char[,] boardDiagonalWinO = {{ 'o', 'x', '_' },
                                        { 'x', 'o', 'x' },
                                        { '_', '_', 'o' }};

            char[,] boardDiagonalWinX = {{ 'o', 'x', 'x' },
                                        { 'o', 'x', 'x' },
                                        { 'x', 'o', 'o' }};

            char[,] boardNoWinners =    {{ 'x', 'o', 'x' },
                                        { 'o', 'o', 'x' },
                                        { 'x', 'x', 'o' }};

            Assert.AreEqual(true, gameControllerLogic.isWin(boardFirstRowWinX, "ExTarget"));
            Assert.AreEqual(true, gameControllerLogic.isWin(boardSecondRowWinX, "ExTarget"));
            Assert.AreEqual(true, gameControllerLogic.isWin(boardDiagonalWinO, "CircleTarget"));
            Assert.AreEqual(false, gameControllerLogic.isWin(boardDiagonalWinO, "ExTarget"));
            Assert.AreEqual(true, gameControllerLogic.isWin(boardDiagonalWinX, "ExTarget"));
            Assert.AreEqual(false, gameControllerLogic.isWin(boardDiagonalWinO, null));
            Assert.AreEqual(false, gameControllerLogic.isWin(boardNoWinners, null));
            Assert.AreEqual(false, gameControllerLogic.isWin(boardNoWinners, "ExTarget"));
            Assert.AreEqual(false, gameControllerLogic.isWin(boardNoWinners, "CircleTarget"));
        }



        
        [Test]
        public void Hint()
        {
            char[,] hintShouldBe1 =     {{ 'x', '_', 'x' },
                                         { '_', '_', '_' },
                                         { '_', '_', '_' }};

            char[,] hintShouldBe4 =     {{ '_', '_', 'x' },
                                         { '_', '_', '_' },
                                         { 'x', '_', '_' }};

            char[,] hintShouldBe5 =     {{ '_', '_', 'x' },
                                         { '_', '_', '_' },
                                         { '_', '_', 'x' }};

            char[,] hintShouldBeFour =   {{ 'x', '_', '_' },
                                         { '_', '_', '_' },
                                         { '_', '_', 'x' }};

            char[,] hintShouldBe8 =     {{ '_', '_', 'x' },
                                         { '_', '_', 'x' },
                                         { '_', '_', '_' }};

            char[,] hintShouldBe0 =     {{ '_', '_', '_' },
                                         { '_', '_', '_' },
                                         { '_', '_', '_' }};
            ///
            /// hint is working with MiniMax algo, hence hard to anticipate the next move in a complicated board
            ///
            Assert.AreEqual(1, gameControllerLogic.getHintIndex(hintShouldBe1));
            Assert.AreEqual(0, gameControllerLogic.getHintIndex(hintShouldBe0));
            Assert.AreEqual(4, gameControllerLogic.getHintIndex(hintShouldBe4));
            Assert.AreEqual(5, gameControllerLogic.getHintIndex(hintShouldBe5));
            Assert.AreEqual(4, gameControllerLogic.getHintIndex(hintShouldBeFour));
            Assert.AreEqual(8, gameControllerLogic.getHintIndex(hintShouldBe8));
        }

        [Test]
        public void Undo() {
            char[,] initialState =        {{ '_', '_', '_' },
                                         { '_', '_', '_' },
                                         { '_', '_', '_' }};

            char[,] firstState =        {{ 'x', '_', '_' },
                                         { '_', '_', '_' },
                                         { '_', '_', '_' }};

            char[,] secondState =        {{ 'x', 'o', '_' },
                                         { '_', '_', '_' },
                                         { '_', '_', '_' }};

            char[,] thirdState =        {{ 'x', 'o', '_' },
                                         { 'x', '_', '_' },
                                         { '_', '_', '_' }};

            char[,] fourthState=        {{ 'x', 'o', '_' },
                                         { 'x', 'o', '_' },
                                         { '_', '_', '_' }};
            Stack<char[,]> boardHistory = new Stack<char[,]>();
            boardHistory.Push(initialState);
            boardHistory.Push(firstState);
            boardHistory.Push(secondState);
            boardHistory.Push(thirdState);
            boardHistory.Push(fourthState);
            
            Assert.AreEqual(secondState, gameControllerLogic.getPreviusBoardState(boardHistory));
            Assert.AreEqual(initialState, gameControllerLogic.getPreviusBoardState(boardHistory));
            Assert.AreEqual(initialState, gameControllerLogic.getPreviusBoardState(boardHistory));


        }
    }
}
