using UnityEngine.UI;

namespace TTT
{
    class AI
    {
        public Text console;

//        private bool testMemory = false;

        private char mark;
        private TTTBoard board = new TTTBoard();
        private int movesIndex = 0;
        private int[] moves_board = new int[9];//Contains the list of moves
        private int[] moves_memory = new int[9];
        private MemoryCell memory = new MemoryCell(9);//Scoring: 0 = loss, 1 = draw, 2 = unkown, 3 = victory
        public enum Scores {Loss, Draw, Unkown, Victory};

        //-- AI CONSTRUCTOR --
        public AI(char tmpMark) {
            mark = tmpMark;
            console.text += "\nAI CONSTURCTOR!\n\n";
        }

        //-- UPDATE BOARD (Other Player Moved) --
        public int nextMove(TTTBoard updatedBoard) {
///            console.text += "\nDo I Remember? " + testMemory + "\n\n";

            MemoryCell currentCell = memory.getCellForIndex(moves_memory, movesIndex);//MCell when other player moved started their turn
            console.text += currentCell.outputMemoryCell();

            int change = boardDiff(updatedBoard);
            console.text += "Change == "+change+" @MovesIndex:"+movesIndex+"\n";               //DEBUG

            if (change >= 0){//Skip if AI is making the first move (-1 denotes no change) //DEBUG
                moves_board[movesIndex] = boardDiff(updatedBoard);
                moves_memory[movesIndex] = currentCell.getMemoryIndexForMove(change);
                console.text += "Moves:\n";               //Debug
                for (int ctr = 0; ctr <= movesIndex; ctr++) {
                    console.text += "MB_"+ctr+": "+moves_board[ctr] + "\t";               //Debug
                    console.text += "MM_"+ctr+": "+moves_memory[ctr] + "\n";              //Debug
                }

                //CHECK TO SEE IF OTHER'S INPUT NEEDS MAPPING...
                int memoryIndex = moves_memory[movesIndex];
                if (memoryIndex == -1) {//Needs mapping
                    for (int ctr = 0; memoryIndex == -1 && ctr < currentCell.possibleMoves.Length; ctr++) {
                        if (currentCell.possibleMoves[ctr].move == -1) {
                            memoryIndex = ctr;
                            moves_memory[movesIndex] = movesIndex;
                            currentCell.possibleMoves[memoryIndex].move = moves_board[movesIndex];
                        }
                    }
                }//new input mapped...

                currentCell = currentCell.possibleMoves[moves_memory[movesIndex]];//Advance current cell (they moved)
                movesIndex++;   //AI's Turn now
                board = updatedBoard.deepCopyBoard();

                //Check Defeat
                if (board.checkVictory())
                {//Update MemoryCell score if AI lost.
                    console.text += "AI LOST :( ";
                    currentCell = memory.getCellForIndex(moves_memory, movesIndex - 2);//movesIndex is current move, -1 opponent's last move, -2 AI's last.
                    currentCell.score = (int)Scores.Loss;
                    console.text += "score is now "+ currentCell.score+"\n";            //Debug
///                    testMemory = true;
                }
            }

            //Get Next Move
            int retval = getMove();//Get Next Move

            console.text += "AI Move Retval: "+retval+"\n";

            return retval;
        }
        private int boardDiff(TTTBoard updatedBoard) {
            int change = 0;
            bool changeNotFound = true;
            for (; change < 9 && changeNotFound; change++) {
                if (board.board[change] != updatedBoard.board[change]) { changeNotFound = false; }
            }
            change--;//tmp test...
            if (changeNotFound){ change = -1; }
            return change;
        }//Returns the last move (Other Player's Move). -1 denotes no change (eg. blank board)

        //-- SELECT NEXT AI MOVE --
        public int getMove() {

            //GET POSSIBLE MOVES (Get Current Memory Cell)
            MemoryCell currentCell = memory.getCellForIndex(moves_memory,movesIndex);

            //EVALUATE POSSIBLE MOVES (Get Best Move)
            int bestIndex = 0;
            int retval = currentCell.possibleMoves[bestIndex].move;
            for (int ctr = 1; ctr < currentCell.possibleMoves.Length; ctr++) {//index 0 is assummed to be best initially...
                int newScore = currentCell.possibleMoves[ctr].score;
                if (currentCell.possibleMoves[ctr].score > currentCell.possibleMoves[bestIndex].score) {
                    bestIndex = ctr;
                    retval = currentCell.possibleMoves[bestIndex].move;
                }
            }

            //CONFIRM BEST MOVE (-1 => next valid move)
            for (int ctr = 0; retval == -1 && ctr < 9; ctr++){//This will only run if move is unkown...
                if (board.checkMove(ctr)){
                    retval = ctr;
                    currentCell.possibleMoves[bestIndex].move = retval;//Update the MemoryCell so it will be known next time.
                }
            }

            //Make move (on AI's board only... real move is handled by TTTGame object.)
            board.attemptMove(retval, mark);//AI should never attempt illeagal moves...

            moves_board[movesIndex] = retval;
            moves_memory[movesIndex] = bestIndex;
            currentCell = currentCell.possibleMoves[bestIndex];//Advance current cell (they moved)

            //CHECK VICTORY? (If so, update memory to denote this is a victorious move sequence)
            if (board.checkVictory()){//Update MemoryCell score if AI won.
                currentCell.score = (int)Scores.Victory;
            }

            //UPDATE MOVES INDEX
            movesIndex++;

            return retval;
        }
    }

    class MemoryCell
    {
        public MemoryCell[] possibleMoves;
        public int score;
        public int move;
        public MemoryCell(int size) {
            score = (int)AI.Scores.Unkown;
            move = -1;//-1 Denotes the move is not yet know... will be determined at runtime?
            possibleMoves = new MemoryCell[size];
            for (int ctr = 0; ctr < size; ctr++){
                possibleMoves[ctr] = new MemoryCell(size - 1);
            }//Initialize MemoryCell in a recursive fashion... MemoryCell memory = new MemoryCell(9) will do the whole thing :)
        }
        public MemoryCell getCellForIndex(int[] moves, int index) {
            MemoryCell currentCell = this;
            for (int ctr = 0; ctr < index; ctr++){
                currentCell = currentCell.possibleMoves[moves[ctr]];
            }//Traverse the MemoryCell structure to get the current cell
            return currentCell;
        }
        public int getMemoryIndexForMove(int move) {
            int retval = -1;
            for (int ctr = 0; ctr < possibleMoves.Length; ctr++) {
                if (possibleMoves[ctr].move == move) { retval = ctr; }
            }
            return retval;
        }//-1 if not found... otherwise return index of cell with move

        //DEBUG
        public string outputMemoryCell() {
            string retval = "-- CURRENT MEMORY CELL --\n";
            retval += "Score:" + score + ", Move:" + move + "\n";
            retval += "---------------------------------\n";
            for (int ctr = 0; ctr < possibleMoves.Length; ctr++){
                retval += "Memory Index:" + ctr + ", Score:" + possibleMoves[ctr].score + ", Move:" + possibleMoves[ctr].move + "\n";
            }
            retval += "---------------------------------\n";
            return retval;
        }
        //TODO: Init from file constructor
        //TODO: Save to file method
    }
}
