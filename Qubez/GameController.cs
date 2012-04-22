/*
This code is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using Qubez.Core;
using Microsoft.Phone.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Resources;
using System.Windows;

namespace Qubez
{

    /// <summary>
    /// A class that handles all the logic/rules of the game.
    /// </summary>
    public class GameController : IDisposable
    {

        #region Events

        /// <summary>
        /// Occurs when [a popup message needs to be displayed].
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The message to be displayed.</param>
        public delegate void OnDisplayPopUpHandler(object sender, string message);

        /// <summary>
        /// Occurs when [a popup message needs to be displayed].
        /// </summary>
        public event OnDisplayPopUpHandler OnDisplayPopup;

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the game is online.
        /// </summary>
        /// <value><c>true</c> if the game is online; otherwise, <c>false</c>.</value>
        public bool IsOnline { get; set; }

        private uint _numberOfSwaps;

        /// <summary>
        /// Gets or sets the current/last change.
        /// </summary>
        /// <value>The current change.</value>
        public Change CurrentChange { get; set; }

        /// <summary> 
        /// Entity containing all the changes made by a player.
        /// </summary>
        public CircularList<Change> Changes { get; set; }

        ///<summary> The Game's Players. </summary>
        public Players Players { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether anybody in the game is playing for the first time.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is first time; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstTime { get; set; }

        ///<summary> The Game Board. </summary>
        public Board Board { get; set; }

        #region Events

        /// <summary>
        /// Event fired when a change in a Message to be displayed occurs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Qubez.MessageEventArgs"/> instance containing the event data.</param>
        public delegate void MessageChangedHandler(object sender, MessageEventArgs e);
        /// <summary>
        /// Occurs when [message is changed and has to be displayed in the infobox].
        /// </summary>
        public event MessageChangedHandler MessageChanged;

        /// <summary>
        /// Event fired when a definition of a draggable occurs
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Qubez.DraggableEventArgs"/> instance containing the event data.</param>
        public delegate void DraggablesDefinedHandler(object sender, DraggableEventArgs e);
        /// <summary>
        /// Occurs when [draggables are defined].
        /// </summary>
        public event DraggablesDefinedHandler DraggablesDefined;

        /// <summary>
        /// Occurs when [players list is renewed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public delegate void OnRenewPlayersHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when [players list is renewed].
        /// </summary>
        public event OnRenewPlayersHandler OnRenewPlayers;

        /// <summary>
        /// Handles the ending of the game and the transitionto the final screen.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="name">The name.</param>
        public delegate void WinnerFoundHandler(object sender, string name);
        /// <summary>
        /// Occurs when [winner found].
        /// </summary>
        public event WinnerFoundHandler WinnerFound;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        /// <param name="isOnline">if set to <c>true</c> [the game is online].</param>
        /// <param name="names">The names of the players.</param>
        /// <param name="sizeOfBoard">The size of board.</param>
        public GameController(bool isOnline, List<string> names, uint sizeOfBoard)
        {
            IsOnline = isOnline;
            Players = new Players(names, Globals.NumberOfLetters);
            Board = new Board(0, sizeOfBoard);
            Changes = new CircularList<Change>();
            IsFirstTime = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Applies the given change to the board.
        /// </summary>
        /// <param name="change">The change to be applied.</param>
        /// <returns>True if the change is applied succesfully, false otherwise.</returns>
        public bool ApplyChange(Change change)
        {
            Changes.Add(change);
            Players[0].UseLetter(change.AddedLetter);
            return Board.AddLetter(change.X, change.Y, change.AddedLetter);
        }

        /// <summary>
        /// Validates all changes by emptying the list of changes that where applied.
        /// </summary>
        public void ClearAllChanges()
        {
            Changes.Clear();
        }

        /// <summary>
        /// Renews the draggable objects.
        /// </summary>
        public void RenewDraggables()
        {
            if (DraggablesDefined != null)
            {
                DraggablesDefined(this, new DraggableEventArgs(Globals.GameController.Players[0].GetCurrentLetters()));
            }
            if (OnRenewPlayers != null)
            {
                OnRenewPlayers(this, new EventArgs());
            }
        }

        /// <summary>
        /// Validates the specified word according to a dictionary.
        /// </summary>
        /// <param name="word">A specified word.</param>
        /// <returns>True if the specified word exists in the dictionary. False otherwise</returns>
        public bool IsWordValid(string word)
        {
            word = word.ToUpper().Trim();
            if (word.Length != 0)
            {
                StreamResourceInfo info = Application.GetResourceStream(new Uri("Files/Dictionaries/" + word[0] + ".dic", UriKind.Relative));
                using (StreamReader reader = new StreamReader(info.Stream))
                {
                    while (!reader.EndOfStream)
                    {
                        if (reader.ReadLine().Trim().Equals(word))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Validates the specified line by tokenizing it and checking it with a dictionary.
        /// </summary>
        /// <param name="line">A specified line.</param>
        /// <returns>True if the specified words from the line exist in the dictionary. False otherwise</returns>
        public bool IsLineValid(string line)
        {
            char[] separator = { ' ', '-' };
            line = line.ToUpper().Trim();
            if (!string.IsNullOrEmpty(line))
            {
                foreach (string word in line.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!IsWordValid(word))
                    {
                        if (MessageChanged != null)
                        {
                            MessageChanged(this, new MessageEventArgs("Invalid word: " + word));
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the changes made are part of a valid final word/move.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if  the changes made are part of a valid final word/move otherwise, <c>false</c>.
        /// </returns>
        public bool IsFinalWordValid()
        {
            bool validRed = false;
            bool validLocation = true;
            bool validDirection = true;

            if (IsFirstTime && Changes.Count == 0)
                return false;

            if (Changes.Count != 0)
            {
                uint x, y;
                foreach (Change change in Changes) {
                    if (!IsChangeValid(change)) {
                        UndoAllLastMoves();
                        return false;
                    }
                }
                if (IsFirstTime)
                { //if i am at the first move of the first player and at least one of his moves isn't on a red square the word is invalid.
                    foreach (Change change in Changes)
                    {
                        if (Board.IsARedSquare(change.X, change.Y))
                        {
                            validRed = true;
                        }
                    }
                }
                if (Changes.Count == 1 && !Board.IsNearAnExistingWord(Changes[0].X, Changes[0].Y)) {
                    if (MessageChanged != null) {
                        MessageChanged(this, new MessageEventArgs("Invalid move. You must add a word containing more than two letters."));
                    }
                    UndoAllLastMoves();
                    return false;
                }
                if (validRed || !IsFirstTime)
                {
                    x = Changes[0].X;
                    y = Changes[0].Y;
                    foreach (Change change in Changes)
                    {  //if at least one of the player's moves isn't near an existing word the word is invalid.
                        if ((!IsFirstTime && !Board.IsNearAnExistingWord(change.X, change.Y)))
                        {
                            if (MessageChanged != null)
                            {
                                MessageChanged(this, new MessageEventArgs("Invalid move. At least one of the letter's isn't near an existing word"));
                            }
                            UndoAllLastMoves();
                            validLocation = false;
                            break;
                        }

                        if (change.X != x && change.Y != y)  //if the changes made aren't all together horizontal or vertical then the word is invalid.
                        {
                            if (MessageChanged != null)
                            {
                                MessageChanged(this, new MessageEventArgs("Invalid move. You can only add horizontal or vertical words."));
                            }
                            UndoAllLastMoves();
                            validDirection = false;
                            break;
                        }
                    }

                    if (validDirection)
                    {
                        if (!IsSpelledCorrectly()) //check spelling
                        { 
                            UndoAllLastMoves();
                            return false;
                        }
                    }
                }
                else
                {
                    if (MessageChanged != null)
                    {
                        MessageChanged(this, new MessageEventArgs("Invalid move. A letter from the first move must be on a red square."));
                    }
                    UndoAllLastMoves();
                }
            }

            return (validRed || !IsFirstTime) && validLocation && validDirection;
        }

        private bool IsSpelledCorrectly()
        {
            if (!Globals.UseDictionary)
                return true;
            string lineVertical = "", lineHorizontal = "";
            for (uint i = 0; i < Board.Size; i++)
            {
                for (uint j = 0; j < Board.Size; j++)
                {
                    lineHorizontal += Board.GetTopLetter(j, i).Char;
                    lineVertical += Board.GetTopLetter(i, j).Char;
                }
                if (!IsLineValid(lineHorizontal) || !IsLineValid(lineVertical))
                {
                    return false;
                }
                lineVertical = "";
                lineHorizontal = "";
            }
            return true;
        }

        /// <summary>
        /// Undoes the last move of the player.
        /// </summary>
        /// <returns>True if the move is reversed succesfully, false otherwise.</returns>
        public bool UndoLastMove()
        {
            bool succeded = Board.RemoveLetter(Changes[Changes.Count - 1].X, Changes[Changes.Count - 1].Y);
            Changes.RemoveAt(Changes.Count - 1);
            return succeded;
        }

        /// <summary>
        /// Undoes all the last moves played.
        /// </summary>
        /// <returns> True if all the moves are succesfull, false otherwise.</returns>
        public bool UndoAllLastMoves()
        {
            bool succeded = true;
            while (Changes.Count != 0)
            {
                if (!UndoLastMove())
                    succeded = false;
                Players[0].Chars.Undo();
            }
            _numberOfSwaps = 0;
            return succeded;
        }

        /// <summary>
        /// Determines whether the given change is a valid one.
        /// </summary>
        /// <param name="change">The change to be.</param>
        /// <returns>
        ///   <c>true</c> if it is a valid change otherwise, <c>false</c>.
        /// </returns>
        public bool IsChangeValid(Change change) {
            if (Board.GetHeight(change.X, change.Y) > 1 && change.AddedLetter.Equals(Board.Letters[change.X, change.Y][Board.Letters[change.X, change.Y].Count - 2])) {
                if (MessageChanged != null) {
                    MessageChanged(this, new MessageEventArgs("Invalid move. Adding a letter on it's equal is not allowed."));
                }

                return false;
            }
            if (change.X >= Board.Size || change.Y >= Board.Size) {
                if (MessageChanged != null) {
                    MessageChanged(this, new MessageEventArgs("Invalid move. Letter added in overflowing coordinates"));
                }

                return false;
            }
            if (Board.GetHeight(change.X, change.Y) > 5) {
                if (MessageChanged != null) {
                    MessageChanged(this, new MessageEventArgs("Invalid move. Can't add more than 5 letters to a cell."));
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the change to all the players.
        /// </summary>
        private void UpdatePlayersScore()
        {
            Players[0].Score += GetPlayersNewScore();
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void BeginGame()
        {
            String firstPlayer = Players[0].Name;
            string player = DetermineFirstPlayer();
            if (OnDisplayPopup != null && firstPlayer != player)
            {
                OnDisplayPopup(this, Players[0].Name + "'s turn. Pass the phone.");
            }
            if (MessageChanged != null)
            {
                MessageChanged(this, new MessageEventArgs(Players[0].Name + "'s turn."));
            }
            RenewDraggables();
        }

        /// <summary>
        /// Determines the first player by picking the player whose first letter in the queue is the closest one to 'A'.
        /// </summary>
        /// <returns> The Name of the first Player.</returns>
        private string DetermineFirstPlayer()
        {
            if (Players != null && Players.Count != 0)
            {
                int firstPlayer = 0;
                for (int i = 1; i < Players.Count; i++)
                {
                    if (Players[i].Chars[0].Char < Players[firstPlayer].Chars[0].Char)
                        firstPlayer = i;
                }
                int moves = (Players.Count - firstPlayer) + 1;
                while (moves-- != 0)
                {
                    Players.Undo();
                }
                Players.Reset();
            }
            return Players[0].Name;
        }


        /// <summary>
        /// Gets the players new score (according to the changes in the Changes object).
        /// </summary>
        /// <returns>The new score of the player</returns>
        private uint GetPlayersNewScore()
        {
            uint score = 0; bool specialLetter = false;
            foreach (Change change in Changes)
            {
                score += change.AddedLetter.Points;              //point for each letter.
                score += Board.GetWordScore(change.X, change.Y); //point for each letter near this new letter.
                if (Board.GetHeight(change.X, change.Y) > 1)
                {   //point for every letter on the same x, y.
                    score += Board.GetCellScore(change.X, change.Y);
                }
                if (Board.GetHeight(change.X, change.Y) == 1)
                {  //double points for being on the first level.
                    score += 2 * change.AddedLetter.Points;
                }
                if (!specialLetter && (change.AddedLetter.Char == 'Z' || change.AddedLetter.Char == 'X' || change.AddedLetter.Char == 'Q' || change.AddedLetter.Char == 'K' || change.AddedLetter.Char == 'J'))
                {
                    score += 2;                                  //special character bonus points.
                    specialLetter = true;
                }
            }
            if (Changes.Count == Globals.NumberOfLetters)
            {                           //used all letters bonus.
                score += 10;
            }

            return score;
        }

        /// <summary>
        /// Determines whether the game is over.
        /// </summary>
        /// <returns>
        /// True if is game over otherwise,  false.
        /// </returns>
        private bool IsGameOver()
        {
            bool endForLostTurn = true;
            foreach (Player player in Players)
            {
                if (player.Chars[0].Char == player.Chars.NULL_CHAR)
                { //if the player runs out of letters
                    return true;
                }
                if (!player.LostTurn)
                {
                    endForLostTurn = false;
                }
                //Also check if all  of the players can't make any more words with their current letters
            }
            if (endForLostTurn)
                return true;

            return false;
        }

        /// <summary>
        /// Calculates the final points for each player according to the number of letters they still have left after the game is over.
        /// </summary>
        private Player GetWinner()
        {
            foreach (Player player in Players)
            { //for every player remove 5 points for each letter still remaining in their character pool
                foreach (Letter letter in player.GetRemainingLetters())
                {
                    player.Score -= 5;
                }
            }

            return (from player in Players
                    orderby player.Score descending
                    select player).FirstOrDefault();
        }

        public Letter SwapLetter(Letter letter) {
            if (_numberOfSwaps < Globals.NumberOfLetters) {
                Letter temp = Players[0].Chars.SwapLetter(letter);
                if (MessageChanged != null) {
                    MessageChanged(this, new MessageEventArgs("Letter changed to " + temp.Char));
                }
                _numberOfSwaps++;
                return temp;
            }
            else {
                if (MessageChanged != null) {
                    MessageChanged(this, new MessageEventArgs("You can't swap more than "+ Globals.NumberOfLetters + " letters per turn."));
                }
                return letter;
            }
        }

        /// <summary>
        /// Loses the turn of a player and moves on to the next.
        /// </summary>
        public void PassTurn(bool loseTurn)
        {
            UndoAllLastMoves();
            Players[0].LostTurn = loseTurn;
            ChangeTurn();
        }

        /// <summary>
        /// Changes the turn of the players by deleting all the changes and replacing tha game elements to those of the other player.
        /// </summary>
        public void ChangeTurn()
        {
            _numberOfSwaps = 0;
            UpdatePlayersScore();
            Changes.Clear();
            if (!IsGameOver())
            {
                Players.GetNext();
                RenewDraggables();
                if (OnDisplayPopup != null)
                {
                    OnDisplayPopup(this, Players[0].Name + "'s turn. Pass the phone.");
                }
                if (MessageChanged != null)
                {
                    MessageChanged(this, new MessageEventArgs(Players[0].Name + "'s turn."));
                }
            }
            else
            {
                Player winner = GetWinner();
                if (WinnerFound != null)
                {
                    WinnerFound(this, winner.Name);
                }
            }
        }
    }
}
