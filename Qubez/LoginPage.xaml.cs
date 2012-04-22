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
    /// The initial page in which the player enters his name.
    /// </summary>
    public partial class LoginPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the KeyUp event of the txtName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOptions.Focus();
            }
        }

        /// <summary>
        /// Determines whether [has assigned name].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has assigned name]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasAssignedName()
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Please, provide a username", "Invalid name", MessageBoxButton.OK);
                return false;
            }

            Globals.FirstPlayerName = txtName.Text.Trim();
            
            return true;
        }

        /// <summary>
        /// Handles the Click event of the btnOnline control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!HasAssignedName())
            {
                return;
            }

            NavigationService.Navigate(new Uri("/AddPlayersPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Handles the Click event of the btnOffline control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/OptionsPage.xaml", UriKind.Relative));
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }
    }
}