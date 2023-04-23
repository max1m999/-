using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

namespace Vodokanal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(TextBox_Login.Text))
                errors.AppendLine("Укажите логин");
            if (string.IsNullOrEmpty(TextBox_Password.Text))
                errors.AppendLine("Укажите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                switch (ComboBox_Role.SelectedIndex)
                {
                    case 0:
                        Абонент абонент = ВодоканалEntities2.GetContext().Абонент.FirstOrDefault(p => p.Логин == TextBox_Login.Text && p.Пароль == TextBox_Password.Text);
                        if (абонент != null)
                        {
                            AbonentWindow abonentWindow = new AbonentWindow(абонент.Id_абонента);
                            abonentWindow.Show();
                            this.Close();
                        }
                        else
                            MessageBox.Show("Неверно введены логин или пароль");
                        break;
                    case 1:
                        Диспетчер диспетчер = ВодоканалEntities2.GetContext().Диспетчер.FirstOrDefault(p => p.Логин == TextBox_Login.Text && p.Пароль == TextBox_Password.Text);
                        if (диспетчер != null)
                        {
                            DispetcherWindow dispetcherWindow = new DispetcherWindow(диспетчер.Id_участка);
                            dispetcherWindow.Show();
                            this.Close();
                        }
                        else
                            MessageBox.Show("Неверно введены логин или пароль");
                        break;
                    case 2:

                        Директор директор = ВодоканалEntities2.GetContext().Директор.FirstOrDefault(p => p.Логин == TextBox_Login.Text && p.Пароль == TextBox_Password.Text );
                        if (директор != null)
                        {
                            DirectorWindow directorWindow = new DirectorWindow(директор.Id_участка);
                            directorWindow.Show();
                            this.Close();
                        }
                        else
                            MessageBox.Show("Неверно введены логин или пароль");
                        break;
                    case 3:

                        Технический_сотрудник техсотрудник = ВодоканалEntities2.GetContext().Технический_сотрудник.FirstOrDefault(p => p.Логин == TextBox_Login.Text && p.Пароль == TextBox_Password.Text );
                        if (техсотрудник != null)
                        {
                            TechSotrudnikWindow techSotrudnikWindow = new TechSotrudnikWindow(техсотрудник.Id_сотрудника);
                            techSotrudnikWindow.Show();
                            this.Close();
                        }
                        else
                            MessageBox.Show("Неверно введены логин или пароль");
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void ComboBox_Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }
    }
}