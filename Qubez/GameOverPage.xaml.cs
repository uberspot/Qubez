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
using Microsoft.Phone.Controls;

namespace Qubez
{
    /// <summary>
    /// Page displayed after the game ends
    /// </summary>
    public partial class GameOverPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverPage"/> class.
        /// </summary>
        public GameOverPage()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                string winner = string.Empty;
                NavigationContext.QueryString.TryGetValue("winner", out winner);

                PageTitle.Text = winner + " wins!";
            };
        }

        private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }
    }
}