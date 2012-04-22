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
    /// User controls representing buttons clicked by a user to verify/reject a word.
    /// </summary>
    public partial class VerificationButtons : UserControl
    {
        /// <summary>
        /// Handle the event of the verification
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="accepted">if set to <c>true</c> [accepted].</param>
        public delegate void OnVerificationHandler(object sender, bool accepted);
        /// <summary>
        /// Occurs when [on verification].
        /// </summary>
        public event OnVerificationHandler OnVerification;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationButtons"/> class.
        /// </summary>
        public VerificationButtons()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (OnVerification != null)
            {
                OnVerification(this, true);
            }
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (OnVerification != null)
            {
                OnVerification(this, false);
            }
        }
    }
}
