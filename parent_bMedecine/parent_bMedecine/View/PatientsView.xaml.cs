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

namespace parent_bMedecine.View
{
    /// <summary>
    /// Logique d'interaction pour PatientsView.xaml
    /// </summary>
    public partial class PatientsView : UserControl
    {
        public PatientsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open patient panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
            parentWindow.ToggleFlyout(1);
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

        /// <summary>
        /// Open patient deletion panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
            parentWindow.ToggleFlyout(3);
        }
    }
}
