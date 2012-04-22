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

namespace Qubez
{
    /// <summary>
    /// A class holding some important Global Variables so they can be accesible by all instances.
    /// </summary>
    public class Globals
    {
        /// <summary>
        /// A static class that will be used by all the forms to control the game.
        /// </summary>
        public static GameController GameController;

        /// <summary>
        /// The Name of the first player (the one that initialized the game).
        /// </summary>
        public static string FirstPlayerName;

        /// <summary>
        /// Size of the game board.
        /// </summary>
        public static int BoardSize;

        /// <summary>
        /// Option containing the number of letters displayed on the lower part of the screen each time.
        /// </summary>
        public static int NumberOfLetters;

        /// <summary>
        /// Option stating whether or not a dictionary should be used for spellchecking or not.
        /// </summary>
        public static bool UseDictionary;

        /// <summary>
        /// Global containing a number for the ZIndex which is always increased every time a draggable object is moved on the board.
        /// </summary>
        public static int ZIndex;

        public static System.Windows.Media.Imaging.WriteableBitmap Screenshot;

        public static string Tweet;
    }
}
