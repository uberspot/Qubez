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
using System.Collections.Generic;

namespace Qubez.Core
{
    /// <summary> A class representing Player entities. </summary>
    public class Player : Elem, IEquatable<Player>
    {

        ///<summary> Default constructor </summary>
        public Player()
            : this(string.Empty, string.Empty, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Player(string name)
            : this(System.Guid.NewGuid().ToString(), name, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="numOfLetters">The num of letters.</param>
        public Player(string name, int numOfLetters)
            : this(System.Guid.NewGuid().ToString(), name, 0,(uint) numOfLetters) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="score">The score.</param>
        public Player(string name, uint score)
            : this(System.Guid.NewGuid().ToString(), name, score) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="score">The score.</param>
        public Player(string id, string name, uint score)
            : this(id, name, score, 7) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="score">The score.</param>
        /// <param name="numOfLetters">The num of letters.</param>
        public Player(string id, string name, uint score, uint numOfLetters)
        {
            Id = id;
            Name = name;
            Score = score;
            LostTurn = false;
            _numOfLetters = numOfLetters;
            Chars = new CharacterPool(numOfLetters);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [the player lost turn].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [he lost turn]; otherwise, <c>false</c>.
        /// </value>
        public bool LostTurn { get; set; }

        ///<summary> The player's id </summary>
        public string Id { get; set; }

        private uint _numOfLetters;

        /// <summary> The name of the Player. </summary>
        public string Name { get; set; }

        /// <summary> The score of the Player. </summary>
        public uint Score { get; set; }

        /// <summary> A pool holding all the available characters of the player. </summary>
        public CharacterPool Chars { get; set; }

        /// <summary>
        /// Returns the players current (_numOfLetters) letters .
        /// </summary>
        /// <returns>A list containing the letters currently viewed by the player.</returns>
        public List<Letter> GetCurrentLetters() {
            List<Letter> currentLetters = new List<Letter>();
            for (int i = 0; i < _numOfLetters; i++)
            {
                if (Chars[i].Char != '-') {
                    currentLetters.Add(Chars[i]);
                } else {
                    break;
                }
            }
            return currentLetters;
        }

        /// <summary>
        /// Uses the player's letter.
        /// </summary>
        /// <param name="letter">The letter.</param>
        public void UseLetter(Letter letter){
            int index = Chars.IndexOf(letter);
            LostTurn = false;
            if (index < Chars.GetNullsPosition()) {
                Chars.SwapElement(index, Chars.RIGHT_TURN);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="right">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(Object right) {
            if (Object.ReferenceEquals(right, null))
                return false;
            if (Object.ReferenceEquals(this, right))
                return true;

            if (this.GetType() != right.GetType())
                return false;
            return this.Equals(right as Player);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Player other) {
            return (other.Id.Equals(this.Id) && other.Name.Equals(this.Name));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            int code = 0;
            foreach (char c in this.Id)
                code += c;
            return code;
        }

        /// <summary>
        /// Returns a list with the remaining letters of the player.
        /// </summary>
        /// <returns>Returns a list with the remaining letters of the player</returns>
        public List<Letter> GetRemainingLetters() { 
            int position = Chars.GetNullsPosition();
            return Chars.GetRange(0, position - 1);
        }

        /// <summary> Returns true when the given element matches the null one. </summary>
        public override bool IsNull() {
            return this.Id == string.Empty && this.Name == string.Empty && this.Score == 0;
        }
    }
}
