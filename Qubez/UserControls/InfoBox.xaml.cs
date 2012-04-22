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
    /// User control representing a simple message box.
    /// </summary>
    public partial class InfoBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBox"/> class.
        /// </summary>
        public InfoBox()
        {
            InitializeComponent();
            Globals.GameController.MessageChanged += new GameController.MessageChangedHandler(GameController_MessageChanged);
        }

        /// <summary>
        /// Handles the MessageChanged event of the GameController control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Qubez.MessageEventArgs"/> instance containing the event data.</param>
        void GameController_MessageChanged(object sender, MessageEventArgs e)
        {
            tblmessage.Text = e.Message;
        }
    }
}
