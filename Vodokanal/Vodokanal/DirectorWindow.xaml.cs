using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
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
using NGS.Templater;

namespace Vodokanal
{
    public partial class DirectorWindow : Window
    {
        public static int id_u;
        public List<string> ZayavkClsd = new List<string>();
        public List<string> ZayavkAll = new List<string>();
        public List<int> Facttime = new List<int>();

        public DirectorWindow(int id_uchastka)
        {
            id_u = id_uchastka;
            InitializeComponent();
            LoadGrids();
        }

        public void LoadGrids()
        {
            var sotrud = ВодоканалEntities2.GetContext().Технический_сотрудник.ToList().Where(p => p.Id_участка == id_u);
            dg_Sotrudniki.ItemsSource = sotrud.ToList();

            var kach = ВодоканалEntities2.GetContext().Проба.ToList().Where(p => p.Id_участка == id_u);
            dg_kachestvo.ItemsSource = kach.ToList();

           
            var tech = ВодоканалEntities2.GetContext().Техническая_информация.Where(p => p.Id_участка == id_u);
            DateTime zeroTime = new DateTime(1, 1, 1);
            foreach (var item in tech)
            {
                TimeSpan span = DateTime.Now - item.Дата_прокладки_труб;
                int years = (zeroTime + span).Year - 1;
                Facttime.Add(years);
            }
            dg_tech.ItemsSource = Facttime.ToList();
            dg_tech_1.ItemsSource = tech.ToList();
            ZayavkClsd.Add( ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Закрыта").Count().ToString());
            dg_zayavki_stat1.ItemsSource = ZayavkClsd;
            ZayavkAll.Add(ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u).Count().ToString());
            dg_zayavki_stat.ItemsSource = ZayavkAll;
        }
        private void BT_Delete_sotr_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить этого сотрудника?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Технический_сотрудник сотрудник = dg_Sotrudniki.SelectedItem as Технический_сотрудник;
                    ВодоканалEntities2.GetContext().Технический_сотрудник.Remove(сотрудник);
                    ВодоканалEntities2.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    dg_Sotrudniki.Items.Remove(dg_Sotrudniki.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadGrids();
        }

        private void BT_Otchet_voda_Click(object sender, RoutedEventArgs e)
        {
            string newPath = $"C:\\reports\\DirectorReport.docx";
            GenerateReport(newPath);
        }

        private void GenerateReport(string newPath)
        {
            var заявки = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u).Count();
            var заявки_закрыты = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Закрыта").Count();
            var сотрудники_зан = ВодоканалEntities2.GetContext().Технический_сотрудник.Where(p => p.Id_участка == id_u && p.Занятость != "Свободен").Count();
            var сотрудники_своб = ВодоканалEntities2.GetContext().Технический_сотрудник.Where(p => p.Id_участка == id_u && p.Занятость == "Свободен").Count();
            var фактический = ВодоканалEntities2.GetContext().Техническая_информация.Where(p => p.Id_участка == id_u);
            string fact = "Не превышает допустимый срок эксплуатации";
            foreach (var item in фактический)
            {
                if (item.Срок_службы__лет < (DateTime.Now - item.Дата_прокладки_труб).TotalDays/365) fact = "Превышает допустимый срок эксплуатации";
            }
            var отбор = ВодоканалEntities2.GetContext().Проба.Where(p => p.Id_участка == id_u);
            double качество = 0;
            foreach (var item in отбор)
            {
                качество += item.Качеcтво_воды___;
            }
            качество /= отбор.Count();
            var data = new Otchet1 {Заявок_всего =заявки, Заявок_закрыто =заявки_закрыты, Фактический_срок = fact, Качество_в_среднем = качество, Свободных_сотрудников = сотрудники_своб, Сотрудников_занято = сотрудники_зан };
            using (var document = Configuration.Factory.Open("C:\\reports\\DirectorReport_.docx"))
            {
                document.Process(data);
            }
            Process.Start(newPath);
        }

    }
    public class Otchet1
    {
        public int Заявок_всего { get; set; }
        public int Заявок_закрыто { get; set; }
        public string Фактический_срок { get; set; }
        public double Качество_в_среднем { get; set; }
        public int Сотрудников_занято { get; set; }
        public int Свободных_сотрудников { get; set; }
    }
}