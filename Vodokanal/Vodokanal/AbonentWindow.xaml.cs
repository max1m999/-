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
    /// Логика взаимодействия для AbonentWindow.xaml
    /// </summary>
    public partial class AbonentWindow : Window
    {
        int id_abonenta;
        public AbonentWindow(int abonent_id)
        {
            InitializeComponent();
            id_abonenta = abonent_id;
            Loadgrid();
        }

        public void Loadgrid()
        {
             var pred_pokazania = ВодоканалEntities2.GetContext().Показания_счётчиков.Where(p => p.Id_абонента == id_abonenta);
            Pokazania_list.ItemsSource = pred_pokazania.ToList();
        }

        private void Send_pokazania_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            string success = "Успешно отправлено";
            if (string.IsNullOrEmpty(Hot.Text))
                errors.AppendLine("Введите показания счётчика горячей воды");
            if (string.IsNullOrEmpty(Cold.Text))
                errors.AppendLine("Введите показания счётчика холодной воды");
            Показания_счётчиков показания = new Показания_счётчиков
            {
                Id_абонента = id_abonenta,
                Показания_счётчика_гор__воды = Convert.ToDouble(Hot.Text),
                Показания_счётчика_хол__воды = Convert.ToDouble(Cold.Text),
                Дата_передачи_показаний = DateTime.Today,
                Id_показаний = ВодоканалEntities2.GetContext().Показания_счётчиков.Count() + 1
            };
            ВодоканалEntities2.GetContext().Показания_счётчиков.Add(показания);
            ВодоканалEntities2.GetContext().SaveChanges();
            MessageBox.Show(success);
            Hot.Text = "";
            Cold.Text = "";
            Loadgrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Абонент абонент = ВодоканалEntities2.GetContext().Абонент.Where(p => p.Id_абонента == id_abonenta).FirstOrDefault();
            int Id_uchastka = абонент.Id_участка;
            StringBuilder errors = new StringBuilder();
            string success = "Успешно отправлено";
            if (string.IsNullOrEmpty(Zayavka.Text))
                errors.AppendLine("Заполните заявку");
            Заявка заявка = new Заявка
            {
                Id_абонента = id_abonenta,
                Id_заявки = ВодоканалEntities2.GetContext().Заявка.Count() + 1,
                Статус_заявки = "Принята",
                Текст_заявки = Zayavka.Text,
                Id_участка = Id_uchastka 
            };
            ВодоканалEntities2.GetContext().Заявка.Add(заявка);
            ВодоканалEntities2.GetContext().SaveChanges();
            MessageBox.Show(success);
            Zayavka.Text = "";
        }

        private void Pokazania_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    } 
}
