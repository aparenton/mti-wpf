using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace parent_bMedecine.View.Flyout
{
    /// <summary>
    /// Logique d'interaction pour AddObservationFlyout.xaml
    /// </summary>
    public partial class AddObservationFlyout : UserControl
    {
        public AddObservationFlyout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open observation panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddObservation_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
            parentWindow.ToggleFlyout(2);
        }
    }
}
