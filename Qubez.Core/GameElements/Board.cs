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

namespace Qubez.Core {
    /// <summary> A class representing a Board entity. </summary>
    public class Board {
        ///<summary> Default constructor </summary>
        public Board() : this(-1, 1) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="size">The size.</param>
        public Board(int id, uint size) {
            Id = id;
            Size = size;
            Letters = new List<Letter>[Size, Size];

            for (uint i = 0; i < Size; i++) {
                for (uint j = 0; j < Size; j++) {
                    Letters[i, j] = new List<Letter>();
                }
            }
        }

        ///<summary> The Board's id. </summary>
        public int Id { get; set; }

        ///<summary> The Board's Size. </summary>
        public uint Size { get; set; }

        ///<summary> An array of List holding the letters of each cell. </summary>
        public List<Letter>[,] Letters { get; set; }

        /// <summary>
        /// Determines whether the given coordinates belong to a red square.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   true if the coordinates belong to a red square (one of the 4 central squares of the board), otherwise, false.
        /// </returns>
        public bool IsARedSquare(uint x, uint y) {
            if (x < Size && y < Size)
                if(Size % 2 == 1)
                    return (x == Math.Floor(Size / 2) && (y == Math.Floor(Size / 2)));
                else
                    return (x + 1 == Size / 2 || x + 1 == 1 + (Size / 2)) && (y + 1 == Size / 2 || y + 1 == 1 + (Size / 2));
            return false;
        }

        /// <summary>
        /// Determines whether the coordinates are near an existing Letter (therefore a preexisting word).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// 	True if the coordinates are near(up, down, right, left of the new coordinates) an existing Letter, otherwise, false.
        /// </returns>
        public bool IsNearAnExistingWord(uint x, uint y) {
            if (x < Size && y < Size) {
                bool left = false, right = false, up = false, down = false;
                if (y != Size-1)
                    up = (Letters[x, y + 1].Count != 0);
                if (x != Size-1)
                    right = (Letters[x + 1, y].Count != 0);
                if (x != 0)
                    left = (Letters[x - 1, y].Count != 0);
                if (y != 0)
                    down = (Letters[x, y - 1].Count != 0);
                return right || left || up || down;
            }
            return false;
        }

        /// <summary>
        /// Adds the letter to the given coordinates if there is enough space.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="letter">The letter.</param>
        /// <returns>True if the letter is added succesfully, false otherwise.</returns>
        public bool AddLetter(uint x, uint y, Letter letter) {
            if (x < Size && y < Size) {
                Letters[x, y].Add(letter);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the letter from the given coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>True if the letter is removed succesfully, false otherwise (e.g. if there are no more letters in that coordinates.</returns>
        public bool RemoveLetter(uint x, uint y) {
            if (x < Size && y < Size && Letters[x, y].Count != 0) {
                Letters[x, y].RemoveAt(Letters[x, y].Count - 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the height of the given stack of letters in those coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Returns the height of the given stack of letters in those coordinates.</returns>
        public uint GetHeight(uint x, uint y) {
            if (x < Size && y < Size) {
                return (uint) Letters[x, y].Count;
            }
            return 0;
        }


        /// <summary>
        /// Counts the total letters on the board instance.
        /// </summary>
        /// <returns></returns>
        public uint Count() {
            uint totalElements = 0;
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    totalElements += (uint) this.Letters[i, j].Count;
                }
            }
            return totalElements;
        }

        /// <summary>
        /// Gets the top letter from the given coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public Letter GetTopLetter(uint x, uint y) {
            if (x < Size && y < Size && Letters[x, y].Count != 0) {
                return Letters[x, y][Letters[x, y].Count - 1];
            }
            return new Letter(' ');
        }

        /// <summary>
        /// Gets the score for all the letters in a specific cell.
        /// </summary>
        /// <param name="x">The x of the cell.</param>
        /// <param name="y">The y of the cell.</param>
        /// <returns></returns>
        public uint GetCellScore(uint x, uint y) {
            uint score = 0;
            if (x < Size && y < Size && Letters[x, y].Count != 0) {
                foreach (Letter letter in Letters[x, y]) {
                    score += letter.Points;
                }
            }
            return score;
        }

        /// <summary>
        /// Gets the score for the letters next to the given one.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public uint GetWordScore(uint x, uint y) {
            uint score = 0;
            if (x < Size && y < Size && Letters[x, y].Count != 0) {
                uint height = (uint) Letters[x, y].Count;
                if (x != Size-1 && Letters[x + 1, y].Count == height) {
                    score += GetTopLetter(x + 1, y).Points;
                }
                if (y != Size-1 && Letters[x, y + 1].Count == height) {
                    score += GetTopLetter(x, y + 1).Points;
                }
                if (x != 0 && Letters[x -1, y].Count == height) {
                    score += GetTopLetter(x - 1, y).Points;
                }
                if (y != 0 && Letters[x, y - 1].Count == height) {
                    score += GetTopLetter(x, y -1).Points;
                }
            }
            return score;
        }

        /// <summary>
        /// Converts all the letters of a cell to a string.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public string CellToString(uint x, uint y) {
            string contents = "The letters underneath are: ";
            if (x < Size && y < Size && Letters[x, y].Count != 0) {
                foreach (Letter letter in Letters[x, y]) {
                    if (letter != null && letter.Char != '-') {
                        contents += letter.Char + ", ";
                    }
                }
                return contents.Substring(0, contents.Length - 2);
            }
            return contents;
        }
    }
}
