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
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Qubez.UserControls
{
    /// <summary>
    /// A user control to represent the Players names and scores in the main page.
    /// </summary>
    public partial class PlayersList : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersList"/> class.
        /// </summary>
        public PlayersList()
        {
            InitializeComponent();
                        
            Loaded += new RoutedEventHandler(PlayersList_Loaded);
        }

        /// <summary>
        /// Handles the Loaded event of the PlayersList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void PlayersList_Loaded(object sender, RoutedEventArgs e)
        {
            FillPlayers();
        }

        /// <summary>
        /// Fills the players list.
        /// </summary>
        public void FillPlayers()
        {
            lbxPlayers.ItemsSource = from player in Globals.GameController.Players
                                     where player.Id != string.Empty && player.Name != string.Empty
                                     select player;
        }
    }
}
