using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace SportStore
{
    public partial class ReportForm : Form
    {
        List<Sale> sales;
        public ReportForm(List<Sale> lst)
        {
            InitializeComponent();
            sales = lst;
            saleBindingSource.DataSource = sales;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) // Данные в начальном DateTimePicker были изменены 
        {
            DateChanged();
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e) // Данные в конечном DateTimePicker были изменены
        {
            DateChanged();
        }
        
        private void DateChanged() // метод обработки событий изменение даты в DateTimePicker'ах
        {
            List<Sale> new_sales = sales.Where(p => p.SaleDate >= BeginDateTimePicker.Value && p.SaleDate <= EndDateTimePicker.Value).ToList();
            saleBindingSource.DataSource = new_sales;
            EarningsLabel.Text = $"Прибыль за выбранный промежуток времени: {CountEarnings(new_sales)}";
        }
        private void ReportForm_Load(object sender, EventArgs e) // событие возникающее при загрузке формы
        {
            BeginDateTimePicker.Value = DateTime.Today.AddMonths(-1);
            EndDateTimePicker.Value = DateTime.Today;
            List<Sale> new_sales = sales.Where(p => p.SaleDate >= BeginDateTimePicker.Value && p.SaleDate <= EndDateTimePicker.Value).ToList();
            saleBindingSource.DataSource = new_sales;
            EarningsLabel.Text = $"Прибыль за выбранный промежуток времени: {CountEarnings(new_sales)}";
        }

        private decimal CountEarnings(IEnumerable<Product> products) // Расчёт заработанных денег в данном промежутке времени
        {
            decimal sum = 0m;
            foreach(var p in products)
            {
                sum += p.Price * p.Amount;
            }
            return sum;
        }

        private void WriteExcelButton_Click(object sender, EventArgs e) // запись в Excel
        {
            // Выборка данных
            List<Sale> new_sales = sales.Where(p => p.SaleDate >= BeginDateTimePicker.Value && p.SaleDate <= EndDateTimePicker.Value).ToList();
            // Создание приложения Excel
            var excelApp = new Excel.Application();
            // Добавление рабочей книжки
            excelApp.Workbooks.Add();
            // Приложение не видно для пользователя
            excelApp.Visible = false;
            // Заполнение данных промежутка времени и наименований столбцок
            excelApp.Range["A3"].Value = "Id";
            excelApp.Range["B3"].Value = "Наименование";
            excelApp.Range["C3"].Value = "Категория";
            excelApp.Range["D3"].Value = "Цена";
            excelApp.Range["E3"].Value = "Количество";
            excelApp.Range["F3"].Value = "Дата продажи";
            excelApp.Range["G3"].Value = "Описание";
            excelApp.Range["A4"].Select();
            // Заполнение строк
            foreach (var p in new_sales)
            {
                excelApp.ActiveCell.Value = p.Id;
                excelApp.ActiveCell.Offset[0, 1].Value = p.Name; // Offset - ссылка на нынешнюю ячейку(можно указать адрес смещения)
                excelApp.ActiveCell.Offset[0, 2].Value = p.Category;
                excelApp.ActiveCell.Offset[0, 3].Value = p.Price;
                excelApp.ActiveCell.Offset[0, 4].Value = p.Amount;
                excelApp.ActiveCell.Offset[0, 5].Value = p.SaleDate;
                excelApp.ActiveCell.Offset[0, 6].Value = p.Description;
                excelApp.ActiveCell.Offset[1, 0].Select();
            }
            // установка ширины столбцов
            excelApp.Range["A1"].ColumnWidth = 3;
            excelApp.Range["B1"].ColumnWidth = 15;
            excelApp.Range["C1"].ColumnWidth = 12;
            excelApp.Range["D1"].ColumnWidth = 12;
            excelApp.Range["E1"].ColumnWidth = 12;
            excelApp.Range["F1"].ColumnWidth = 15;
            excelApp.Range["G1"].ColumnWidth = 40;
            // объединение ячеек A1 - G1
            excelApp.Range["A1:G1"].Merge();
            // запись отчётных данных
            excelApp.Range["A1"].Value = $"Отчет предоставлен с {BeginDateTimePicker.Value.ToString("dd/MM/yyyy")} по {EndDateTimePicker.Value.ToString("dd/MM/yyyy")}";
            // вычисление индекса строки после последней заполненой
            var total_index = 4 + new_sales.Count;
            // объединение ячеек А-F и присваивание значения
            excelApp.Range[$"A{total_index}:F{total_index}"].Merge();
            excelApp.Range[$"A{total_index}"].Value = "ИТОГО: " + CountEarnings(new_sales).ToString();
            // установка выравнивания по центру
            excelApp.Range[$"A1:F{total_index - 1}"].Style.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            // установка формата данных для столбца F
            excelApp.Range["F2"].EntireColumn.NumberFormat = "DD/MM/YYYY";
            // делаем эксель видимым для пользователя
            excelApp.Visible = true;
        }

        private void WriteWordButton_Click(object sender, EventArgs e) // запись в Word
        {
            // выборка данных
            List<Sale> new_sales = sales.Where(p => p.SaleDate >= BeginDateTimePicker.Value && p.SaleDate <= EndDateTimePicker.Value).ToList();
            var wordApp = new Word.Application(); // Создание приложения            
            object objMissing = System.Reflection.Missing.Value; // Создание поля, в котором хранится Missing Value
            object objEndOfDocument = "\\endofdoc"; // Создание поля в котором хранится значение для конца документа            
            Document wordDoc = wordApp.Documents.Add(ref objMissing, ref objMissing, ref objMissing, ref objMissing);// Создание документа
            Paragraph para = wordDoc.Paragraphs.Add();// Создание параграфа           
            Range r = para.Range; //Создание Range
            para.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Установление центрирования текста для параграфа
            // Запись промежутка времени за который представлен отчёт в виде текста
            r.Text = ($"Отчет предоставлен с {BeginDateTimePicker.Value.ToString("dd/MM/yyyy")} по {EndDateTimePicker.Value.ToString("dd/MM/yyyy")}");
            Table wordTable; // Создание таблицы
            Range wordRange = wordDoc.Bookmarks[ref objEndOfDocument].Range; // создание Range 
            wordTable = wordDoc.Tables.Add(wordRange, new_sales.Count + 1, 7, ref objMissing, ref objMissing); // работа с таблицей
            wordTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle; // Установление стиля внешних границ таблицы
            wordTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle; // Установление стиля внутренних границ таблицы
            wordApp.Visible = true; // Приложение видно пользователю
            // Запись наименований столбцов
            wordTable.Cell(1, 1).Range.Text = "Id"; 
            wordTable.Cell(1, 2).Range.Text = "Наименование";
            wordTable.Cell(1, 3).Range.Text = "Категория";
            wordTable.Cell(1, 4).Range.Text = "Цена";
            wordTable.Cell(1, 5).Range.Text = "Количество";
            wordTable.Cell(1, 6).Range.Text = "Дата продажи";
            wordTable.Cell(1, 7).Range.Text = "Описание";
            // Запись объектов
            for (int i = 2; i < new_sales.Count + 2; i++)
            {
                wordTable.Cell(i, 1).Range.Text = new_sales[i - 2].Id.ToString();
                wordTable.Cell(i, 2).Range.Text = new_sales[i - 2].Name;
                wordTable.Cell(i, 3).Range.Text = new_sales[i - 2].Category;
                wordTable.Cell(i, 4).Range.Text = new_sales[i - 2].Price.ToString();
                wordTable.Cell(i, 5).Range.Text = new_sales[i - 2].Amount.ToString();
                wordTable.Cell(i, 6).Range.Text = new_sales[i - 2].SaleDate.ToString("dd/MM/yyyy");
                wordTable.Cell(i, 7).Range.Text = new_sales[i - 2].Description;
            }
            Paragraph par = wordDoc.Paragraphs.Add();// Создание параграфа           
            Range rng = par.Range; //Создание Range
            para.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Установление центрирования текста для параграфа
            // Запись промежутка времени за который представлен отчёт в виде текста
            rng.Text = $"Выручка за этот промежуток: {CountEarnings(new_sales)}";
        }

    }
}
