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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vodokanal
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            Участок участок = new Участок();
            LoadCombo();
        }
        public void LoadCombo()
        {
            int count = ВодоканалEntities2.GetContext().Участок.ToList().Count;
            List<int> uchast = new List<int>();
            while (uchast.Count < count) 
            { 
                uchast.Add(uchast.Count+1);
            }
            Uchastok_num.ItemsSource = uchast;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string success = "Успешнаяя регистрация";
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(Login.Text))
                errors.AppendLine("Укажите логин");
            if (string.IsNullOrEmpty(Password.Text))
                errors.AppendLine("Укажите пароль");
            if (string.IsNullOrEmpty(TB_Familia.Text))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrEmpty(TB_Imya.Text))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrEmpty(TB_Otchestvo.Text))
                errors.AppendLine("Укажите отчество");
            if (string.IsNullOrEmpty(Uchastok_num.Text))
                errors.AppendLine("Укажите номер участка");
            if (string.IsNullOrEmpty(Sotrud_type.Text))
                errors.AppendLine("Укажите тип учётной записи");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                switch (Sotrud_type.SelectedIndex)
                {
                    case 0:
                        Абонент абонент = new Абонент
                        {
                            Id_абонента = ВодоканалEntities2.GetContext().Абонент.Count() + 1,
                            Фамилия = TB_Familia.Text,
                            Имя = TB_Imya.Text,
                            Отчество = TB_Otchestvo.Text,
                            Дата_подключения = DateTime.Today,
                            Id_участка = Convert.ToInt32(Uchastok_num.SelectedItem),
                            Логин = Login.Text,
                            Пароль = Password.Text
                        };
                        ВодоканалEntities2.GetContext().Абонент.Add(абонент);
                        ВодоканалEntities2.GetContext().SaveChanges();
                        MessageBox.Show(success);
                        break;
                    case 1:
                        Диспетчер диспетчер = new Диспетчер
                        {
                            Id_диспетчера = ВодоканалEntities2.GetContext().Диспетчер.Count() + 1,
                            Фамилия = TB_Familia.Text,
                            Имя = TB_Imya.Text,
                            Отчество = TB_Otchestvo.Text,
                            Id_участка = Convert.ToInt32(Uchastok_num.SelectedItem),
                            Логин = Login.Text,
                            Пароль = Password.Text
                        };
                        ВодоканалEntities2.GetContext().Диспетчер.Add(диспетчер);
                        ВодоканалEntities2.GetContext().SaveChanges();
                        MessageBox.Show(success);
                        break;
                    case 2:

                        Директор директор = new Директор
                        {
                            Id_директора = ВодоканалEntities2.GetContext().Директор.Count() + 1,
                            Фамилия = TB_Familia.Text,
                            Имя = TB_Imya.Text,
                            Отчество = TB_Otchestvo.Text,
                            Id_участка = Convert.ToInt32(Uchastok_num.SelectedItem),
                            Логин = Login.Text,
                            Пароль = Password.Text
                        };
                        ВодоканалEntities2.GetContext().Директор.Add(директор);
                        ВодоканалEntities2.GetContext().SaveChanges();
                        MessageBox.Show(success);
                        break;
                    case 3:

                        Технический_сотрудник техсотрудник = new Технический_сотрудник
                        {
                            Id_сотрудника = ВодоканалEntities2.GetContext().Технический_сотрудник.Count() + 1,
                            Фамилия = TB_Familia.Text,
                            Имя = TB_Imya.Text,
                            Отчество = TB_Otchestvo.Text,
                            Id_участка = Convert.ToInt32(Uchastok_num.SelectedItem),
                            Логин = Login.Text,
                            Пароль = Password.Text,
                            Занятость = "Свободен"
                        };
                        ВодоканалEntities2.GetContext().Технический_сотрудник.Add(техсотрудник);
                        ВодоканалEntities2.GetContext().SaveChanges();
                        MessageBox.Show(success);
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
    }
}