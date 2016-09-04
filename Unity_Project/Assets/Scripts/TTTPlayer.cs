using System;
using UnityEngine.UI;

namespace TTT
{
    class TTTPlayer
    {
        public Text console;

        char marker = 'X';
        public bool human = true;
        AI computer;

        public TTTPlayer(char markerTmp, bool humanTmp)
        {
            marker = markerTmp;
            human = humanTmp;
            if (!human) {
                computer = new AI(marker);
            }
        }
        public TTTPlayer(char markerTmp, bool humanTmp, AI comp)
        {
            marker = markerTmp;
            human = humanTmp;
            computer = comp;
        }
        public char getMarker()
        {
            return marker;
        }
        public int getMoveFromComputer(TTTBoard currentBoard) {
            int retval = -1;
            computer.console = console;
            retval = computer.nextMove(currentBoard);
            return retval;
        }
        public int getMoveFromComputer_old(TTTBoard currentBoard)
        {
            TTTBoard testBoard = new TTTBoard();
            testBoard.resetBoard();
            char testmarker = marker;
            for (int magic = 0; magic < 2; magic++)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        testBoard.board[y] = currentBoard.board[y];
                    }
                    if (testBoard.attemptMove(i, testmarker))
                    {
                        if (testBoard.checkVictory())
                        {
                            return i;
                        }
                    }
                }
                if (testmarker == 'X') { testmarker = 'O'; }
                else { testmarker = 'X'; }
            }
            //Move in center if that is available
            if (currentBoard.checkMove(4))
            {
                return 4;
            }
            //Move in corner if available (center is already taken...)
            Random random = new Random();
            for (int ctr = 0; ctr < 5; ctr++)
            {
                //int move = random.Next(0, 4);
                int move = ctr;
                move *= 2; //ensure move is even (0,2,4,6,8)
                if (currentBoard.checkMove(move))
                {
                    return move;
                }
            }
            return random.Next(0, 9);
        }
    }
}
