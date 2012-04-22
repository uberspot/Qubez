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
    /// A user control representing a message displayed on a small 'cloud'.
    /// </summary>
    public partial class PopUp : UserControl
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message to be displayed.
        /// </value>
        public string Message
        {
            get { return tblMessage.Text; }
            set { tblMessage.Text = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopUp"/> class.
        /// </summary>
        public PopUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopUp"/> class.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public PopUp(string message)
        {
            InitializeComponent();
            Message = message;
        }

        /// <summary>
        /// Handles the Click event of the Close control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
