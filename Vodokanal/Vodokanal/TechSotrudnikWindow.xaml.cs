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

namespace Vodokanal
{
    /// <summary>
    /// Логика взаимодействия для SotrudnikWindow.xaml
    /// </summary>
    public partial class TechSotrudnikWindow : Window
    {
        int id_s;
        public TechSotrudnikWindow(int id_sotrudnika)
        {
            id_s= id_sotrudnika;
            InitializeComponent();
            var заявки = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_сотрудника == id_sotrudnika && p.Статус_заявки != "Закрыта");
            dg_zayavki.ItemsSource = заявки.ToList();
        }
        private void check_zayavku_Click(object sender, RoutedEventArgs e)
        {
            Заявка заявка = dg_zayavki.SelectedItem as Заявка;
            ZayavkaWindow zayavkaWindow = new ZayavkaWindow(заявка.Текст_заявки);
            zayavkaWindow.Show();
        }
        private void close_zayavku_Click(object sender, RoutedEventArgs e)
        {
            Заявка заявка = dg_zayavki.SelectedItem as Заявка;
            заявка.Статус_заявки = "Закрыта";
            ВодоканалEntities2.GetContext().SaveChanges();
            MessageBox.Show("Заявка закрыта");
            var заявки = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_сотрудника == id_s && p.Статус_заявки != "Закрыта");
            dg_zayavki.ItemsSource = заявки.ToList();
        }
    }
}