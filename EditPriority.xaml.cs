using Ayupov_Glazki;
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
using System.Windows.Shapes;

namespace Ayupov_Glazki
{
    /// <summary>
    /// Логика взаимодействия для EditPriority.xaml
    /// </summary>
    public partial class EditPriority : Window
    {
        private Agent _currentAgent = new Agent();
        public EditPriority(int p)
        {
            InitializeComponent();
            Priority.Text = p.ToString();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Priority.Text != string.Empty)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Введите приоритет");
            }
        }
    }
}