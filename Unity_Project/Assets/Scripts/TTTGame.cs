using System;
using UnityEngine.UI;

namespace TTT
{
    public class TTTGame
    {
        AI computerX = new AI('X');
        AI computerO = new AI('O');


        Text console;
        String gameText;
        String menuText;
        TTTPlayer player_x;
        TTTPlayer player_o;
        TTTPlayer currentPlayer;
        TTTBoard board = new TTTBoard();
        int numberOfMoves = 0;
        bool victory = false;

        public TTTGame(Text consoleTmp, bool human_x, bool human_o){
            console = consoleTmp;
            player_x = new TTTPlayer('X', human_x);
            player_o = new TTTPlayer('O', human_o);
            gameText = "Test Tic Tac Toe AI Test Version 0.4.\n";
            gameText += "Say \"Menu\" for a list of instructions.\n";
            gameText += board.getBoard();
            console.text = gameText;
            displayMenuText();
            currentPlayer = player_x;
            if (!player_x.human) { handleInput(-1); }//invalid input is used here to get the ball rolling if AI is first up.
        }
        public void resetGame(Text consoleTmp, bool human_x, bool human_o){
            console = consoleTmp;
            board.resetBoard();
            player_x = new TTTPlayer('X', human_x);
            if (player_o.human) {
                player_o = new TTTPlayer('O', human_o);
            } else {
                player_o = new TTTPlayer('O', human_o, computerO);                      //DEBUG
            }
            gameText = "Test Tic Tac Toe AI Test Version 0.4.\n";
            gameText += "Say \"Menu\" for a list of instructions.\n";
            gameText += board.getBoard();
            console.text = gameText;
            numberOfMoves = 0;
            victory = false;
            currentPlayer = player_x;
            if (!player_x.human) { handleInput(-1); }
        }
        public void displayMenuText(){//NON-MONOSPACED FONT...
            menuText = "+------------------------------------------------------------------------------------------------+\n";
            menuText += "| Welcome to Tic-Tac-Toe: Voice Edition 0.3                              \t\t\t\t\t\t|\n";
            menuText += "| The objective of the game is to get three of your marks in a row.      \t\t\t\t|\n";
            menuText += "| The following is a list of voice commands that the game can recognize. \t\t|\n";
            menuText += "+------------------------------------------------------------------------------------------------+\n";
            menuText += "|                                                                                  \t\t\t\t\t\t\t\t\t\t\t|\n";
            menuText += "| -- General Commands --                                                           \t\t\t\t\t\t|\n";
            menuText += "| \"New Game\":                       \t\t\tStarts a new game.                     \t\t\t\t|\n";
            menuText += "| \"Exit\":                           \t\t\t\t\tExits the program.                 \t\t\t\t\t\t|\n";
            menuText += "| \"Menu\":                           \t\t\t\tShows/Hides these instructions       \t\t\t|\n";
            menuText += "|                                                                                  \t\t\t\t\t\t\t\t\t\t\t|\n";
            menuText += "| -- Game Setting Commands --                                                      \t\t\t\t\t|\n";
            menuText += "| \"Human Verses Human\":       \t\tSets X and O as humans.               \t\t\t|\n";
            menuText += "| \"Human Verses Computer\":    \t\tSets X as a human and O as a computer.\t|\n";
            menuText += "| \"Computer Verses Human\":    \t\tSets X as a human and O as a computer.\t|\n";
            menuText += "| \"Computer Verses Computer\": \tSets X and O as computers.              \t\t|\n";
            menuText += "|                                                                                  \t\t\t\t\t\t\t\t\t\t\t|\n";
            menuText += "| -- Marker Placement Commands --                                                  \t\t\t\t|\n";
            menuText += "| \"Top Left\" or \"One\":            \t\t\tPlace Mark in the top left space.      \t\t\t|\n";
            menuText += "| \"Top Center\" or \"Two\":          \t\tPlace Mark in the top center space.      \t\t|\n";
            menuText += "| \"Top Right\" or \"Three\":         \t\t\tPlace Mark in the top right space.     \t\t\t|\n";
            menuText += "| \"Center Left\" or \"Four\":        \t\t\tPlace Mark in the center left space.   \t\t|\n";
            menuText += "| \"Center\" or \"Five\":             \t\t\tPlace Mark in the center space.        \t\t\t|\n";
            menuText += "| \"Center Right\" or \"Six\":        \t\t\tPlace Mark in the center right space.  \t\t|\n";
            menuText += "| \"Bottom Left\" or \"Seven\":       \t\tPlace Mark in the bottom left space.     \t\t|\n";
            menuText += "| \"Bottom Center\" or \"Eight\":     \t\tPlace Mark in the bottom center space.   \t|\n";
            menuText += "| \"Bottom Right\" or \"Nine\":       \t\tPlace Mark in the bottom right space.    \t|\n";
            menuText += "+------------------------------------------------------------------------------------------------+\n";
            console.fontSize = 15;
            console.text = menuText;
        }
        public void hideMenuText(){
//            console.fontSize = 30;
//            console.text = gameText;
        }
        public void handleInput(int input){
            console.text += "::";
            if (numberOfMoves < 9 && !victory){
                int boardIndex = 0;
                if (currentPlayer.human) {
                    //HUMAN
                    console.text += "Human\n";
                    boardIndex = input;
                }
                else {
                    //AI SECTION IN-PROGRESS
                    console.text += "AI\n";
                    currentPlayer.console = console;
                    boardIndex = currentPlayer.getMoveFromComputer(board);
                }
                bool worked = board.attemptMove(boardIndex, currentPlayer.getMarker());
                if (worked){
                    numberOfMoves++;
                    victory = board.checkVictory();
                    if (currentPlayer.getMarker() == 'X') { currentPlayer = player_o; }
                    else { currentPlayer = player_x; }
                    console.text += "Move Completed\n";
                }else{
                    console.text += "Move Failed\n";
                }
                console.text += board.getBoard();

                if (victory) {
                    console.text += "Player " + currentPlayer.getMarker() + " Lost!\nSay \"New Game\" to play again.\n";
                    if (!currentPlayer.human) { currentPlayer.getMoveFromComputer(board); }//Update AI with the loss
                }
                else if (numberOfMoves == 9) { console.text += "Cat's Game!\nSay \"New Game\" to play again.\n"; }
                else if (!currentPlayer.human){
                    handleInput(-1);
                }//Don't wait for human command to take AI's Turn...
                else { console.text += "Player " + currentPlayer.getMarker() + ", select your move.\n"; }//Prompt Human Player
            }
            else
            {
                console.text += "Cat's Game!\nSay \"New Game\" to play again.\n";
            }
//            console.text += gameText;
        }
    }
}

/*
- Two Logs system: have one Text object that is connected to the unity project and 2 additional Text objects that are not (swap which is pointed to by unity Text)...
- Separate voice response text field that fades or vanishes after ~2 seconds ("Command Recognized") so the user knows if (s)he was heard.
- Computer still does not play a "perfect" game... could be improved.
- Computer will lose to 15,93,76,8X
- Increase command vocab?
- Holo board?
- Gaze input?
- Guesture input?
- BT keyboard input?
*/
