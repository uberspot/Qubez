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
using System.Collections.Generic;

namespace Qubez.Core
{
    /// <summary>
    /// Class representing a player.
    /// </summary>
    public class Players : CircularList<Player>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Players"/> class.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="numOfLetters">The num of letters.</param>
        public Players(List<string> names, int numOfLetters) 
        {
            foreach(string name in names)
            {
                this.Add(new Player(name, numOfLetters));
            }
        }

        /// <summary>
        /// Returns the player according to his id.
        /// </summary>
        /// <param name="guid">The GUID of the player to be found.</param>
        /// <returns>The player with the given guid</returns>
        public Player GetPlayerById(string guid) {
            return this.FirstOrDefault(x => x.Id == guid);
        }
    }
}
