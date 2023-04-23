using System;
using System.Collections.Generic;
using System.Data;
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
    /// <summary>
    /// Логика взаимодействия для DispetcherWindow.xaml
    /// </summary>
    public partial class DispetcherWindow : Window
    {
        public static int id_u;
        public DispetcherWindow(int id_uchastka)
        {
            id_u = id_uchastka;
            InitializeComponent();
            var заявки = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Принята");
            dg_zayavki.ItemsSource = заявки.ToList();
            var сотрудники = ВодоканалEntities2.GetContext().Технический_сотрудник.Where(p => p.Id_участка == id_u && p.Занятость == "Свободен");
            dg_sotrudniki.ItemsSource = сотрудники.ToList();
        }

        private void zayavka_view_Click(object sender, RoutedEventArgs e)
        {
            Заявка заявка = dg_zayavki.SelectedItem as Заявка;
            ZayavkaWindow zayavkaWindow = new ZayavkaWindow(заявка.Текст_заявки);
            zayavkaWindow.Show();
        }

        //private void dispetcher_otchet_Click(object sender, RoutedEventArgs e)
        //{        
        //    string now = $"{DateTime.Now.DayOfWeek}-{DateTime.Now.Month}-{DateTime.Now.Year}";
        //    string newPath = $"C:\\reports\\ZayavkReport_{now}.docx";
        //    FileInfo fileInf = new FileInfo(@".\ExampleReportZayavka.docx");
        //    if (fileInf.Exists)
        //    {
        //        fileInf.CopyTo(newPath, true);
        //    }
        //    GenerateZayvkiReport(newPath, ВодоканалEntities2.GetContext().Заявка.ToList());
        //}
        //public static void GenerateZayvkiReport(string path, List<Заявка> data)
        //{
        //    var dt = new DataTable();
        //    dt.Columns.Add("Всего заявок", typeof(DateTime));
        //    dt.Columns.Add("Заявок закрыто, %", typeof(int));
        //    foreach (var item in data)
        //    {
        //        dt.Rows.Add(new object[] { ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Закрыта").Count().ToString() });
        //    }
        //    using (var document = Configuration.Factory.Open(path))
        //    {
        //        document.Process(new { Table = dt });
        //    }
        //    Process.Start(path);
        //}

        private void zayavki_reload_Click(object sender, RoutedEventArgs e)
        {
            var заявки = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Принята");
            dg_zayavki.ItemsSource = заявки.ToList();
        }

        private void sotrudniki_reload_Click(object sender, RoutedEventArgs e)
        {
            var сотрудники = ВодоканалEntities2.GetContext().Технический_сотрудник.Where(p => p.Id_участка == id_u && p.Занятость == "Свободен");
            dg_sotrudniki.ItemsSource = сотрудники.ToList();
        }

        private void set_zayavka_Click(object sender, RoutedEventArgs e)
        {
            Заявка заявка = dg_zayavki.SelectedItem as Заявка;
            Технический_сотрудник сотрудник = dg_sotrudniki.SelectedItem as Технический_сотрудник;
            заявка.Id_сотрудника = сотрудник.Id_сотрудника;
            заявка.Статус_заявки = "Передана техническому сотруднику";
            сотрудник.Занятость = "Занят";
            ВодоканалEntities2.GetContext().SaveChanges();
            MessageBox.Show("Заявка передана сотруднику");
            sotrudniki_reload_Click(sender,e);
            zayavki_reload_Click(sender,e);
        }

        private void dispetcher_otchet_Click(object sender, RoutedEventArgs e)
        {
            string newPath = $"C:\\reports\\DispetcherReport.docx";
            GenerateReport(newPath);
        }

        private void GenerateReport(string newPath)
        {
            var заявки_прин = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u).Count();
            var заявки_переданы = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Принята").Count();
            var заявки_закрыты = ВодоканалEntities2.GetContext().Заявка.Where(p => p.Id_участка == id_u && p.Статус_заявки == "Закрыта").Count();
            var сотрудники_зан = ВодоканалEntities2.GetContext().Технический_сотрудник.Where(p => p.Id_участка == id_u && p.Занятость != "Свободен").Count();
            var сотрудники_своб = ВодоканалEntities2.GetContext().Технический_сотрудник.Where(p => p.Id_участка == id_u && p.Занятость == "Свободен").Count();
            var data = new Otchet {Заявок_принято = заявки_прин, Заявок_в_работе = заявки_переданы, Заявок_закрыто = заявки_закрыты, Сотрудников_занято = сотрудники_зан, Свободных_сотрудников = сотрудники_своб };
            using (var document = Configuration.Factory.Open("C:\\reports\\DispetcherReport_.docx"))
            {
                document.Process(data);
            }
            Process.Start(newPath);
        }
    }
    public class Otchet
    {
        public int Заявок_принято { get; set; }
        public int Заявок_в_работе { get; set; }
        public int Заявок_закрыто { get; set; }
        public int Сотрудников_занято { get; set; }
        public int Свободных_сотрудников { get; set; }
    }
}
