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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Resources;
using System.Windows;

namespace Qubez.Core
{
    /// <summary> A Circular list containing all of the characters the user can still use. </summary>
    public class CharacterPool : CircularList<Letter>
    {
        private uint _numOfLetters;
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterPool"/> class.
        /// </summary>
        public CharacterPool(uint numOfLetters)
        {
            _numOfLetters = numOfLetters;
            StreamResourceInfo xml = Application.GetResourceStream(new Uri("/Qubez.Core;component/Files/Letters.xml", UriKind.Relative));

            XDocument document = XDocument.Load(xml.Stream);

            var letters = from node in document.Descendants("letter")
                          select new
                          {
                              LetterChar = node.Element("character").Value[0],
                              LetterPoints = uint.Parse(node.Element("points").Value),
                              LetterQuantity = int.Parse(node.Element("quantity").Value)
                          };

            foreach (var letter in letters)
            {
                for (int i = 0; i < letter.LetterQuantity; i++)
                {
                    this.Add(new Letter(letter.LetterChar, letter.LetterPoints));
                }
            }
            this.Shuffle();
            this.Add(new Letter());
        }


        /// <summary>
        /// Swaps the first occurence in the seven first letters of the given letter to another random one from the remaining pool.
        /// </summary>
        /// <param name="let">The letter to be swapped.</param>
        /// <returns>The new Letter that replaced the previous one.</returns>
        public Letter SwapLetter(Letter let)
        {
            for (int i = 0; i < _numOfLetters; i++)
            {
                if (this[i].Equals(let))
                {
                    return this.SwapRandomlyElement(i);
                }
            }
            return let;
        }
    }
}
