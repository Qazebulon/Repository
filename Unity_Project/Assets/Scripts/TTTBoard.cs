using System;

namespace TTT
{
    class TTTBoard
    {
        public Char[] board = new Char[9];//TTT board array
        public TTTBoard() {
            resetBoard();
        }
        public void resetBoard() {
            for (int ctr = 0; ctr < 9; ctr++){
                board[ctr] = ' ';
            }
        }//Fills board array with initial values of ' ' (space).
        public TTTBoard deepCopyBoard() {
            TTTBoard copy = new TTTBoard();
            for (int ctr = 0; ctr < 9; ctr++) {
                copy.board[ctr] = board[ctr];
            }
            return copy;
        }//Return a deep copy of the board.
        public bool checkMove(int boardIndex){
            return (boardIndex >= 0 && boardIndex < 9 && board[boardIndex] == ' ');
        }//Returns true if move is valid.
        public bool attemptMove(int boardIndex, char playerMarker){
            bool retval = checkMove(boardIndex);
            if (retval) { board[boardIndex] = playerMarker; }
            return retval;
        }//Makes move if valid, otherwise returns false.
        public bool checkVictory()
        {
            if (board[0] == board[4] && board[0] == board[8] && board[0] != ' '){ return true; }// TopLeftDownRight
            if (board[2] == board[4] && board[2] == board[6] && board[2] != ' '){ return true; }//TopRightDownLeft
            if (board[0] == board[3] && board[0] == board[6] && board[0] != ' '){ return true; }//LeftVertical
            if (board[1] == board[4] && board[1] == board[7] && board[1] != ' '){ return true; }// MiddleVertical
            if (board[2] == board[5] && board[2] == board[8] && board[2] != ' '){ return true; }//RightVertical
            if (board[0] == board[1] && board[0] == board[2] && board[0] != ' '){ return true; }//TopHorizontal
            if (board[3] == board[4] && board[3] == board[5] && board[3] != ' '){ return true; }//MiddleHorizontal
            if (board[6] == board[7] && board[6] == board[8] && board[6] != ' '){ return true; }//BottomHorizontal
            return false;
        }//Returns True if victory conditions have been met.
        public String getBoard(){
            String retval;
            retval = "+----+-----+----+\n";
            retval += "|\t" + board[0] + "\t|\t" + board[1] + "\t|\t" + board[2] + "\t|\n";
            retval += "+----+-----+----+\n";
            retval += "|\t" + board[3] + "\t|\t" + board[4] + "\t|\t" + board[5] + "\t|\n";
            retval += "+----+-----+----+\n";
            retval += "|\t" + board[6] + "\t|\t" + board[7] + "\t|\t" + board[8] + "\t|\n";
            retval += "+----+-----+----+\n";
            return retval;
        }//Returns a formated string of the board for output.
    }//TTTBoard handles all board interaction logic
}//TTBoard is part of the TTT namespace
