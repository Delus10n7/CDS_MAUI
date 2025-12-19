using CDS_MAUI.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

using Font = iTextSharp.text.Font;
using Element = iTextSharp.text.Element;

namespace CDS_MAUI.PdfGenerator
{
    public class CarContractPdfGenerator
    {
        private BaseFont _russianFont;

        public CarContractPdfGenerator()
        {
            // Инициализируем шрифт для кириллицы
            InitializeRussianFont();
        }

        private void InitializeRussianFont()
        {
            try
            {
                // Путь к шрифту Arial
                string fontPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                    "arial.ttf");

                // Альтернативные пути к шрифтам на Windows
                if (!File.Exists(fontPath))
                {
                    fontPath = @"C:\Windows\Fonts\arial.ttf";
                }

                if (File.Exists(fontPath))
                {
                    _russianFont = BaseFont.CreateFont(
                        fontPath,
                        BaseFont.IDENTITY_H, // Важно: используем IDENTITY_H для Unicode
                        BaseFont.EMBEDDED); // Встраиваем шрифт в PDF
                }
                else
                {
                    // Используем стандартный шрифт, если Arial не найден
                    _russianFont = BaseFont.CreateFont(
                        BaseFont.HELVETICA,
                        BaseFont.CP1252, // Кодировка Windows-1252
                        BaseFont.NOT_EMBEDDED);
                }
            }
            catch
            {
                _russianFont = BaseFont.CreateFont(
                    BaseFont.HELVETICA,
                    BaseFont.CP1252,
                    BaseFont.NOT_EMBEDDED);
            }
        }

        public string FillPdfTemplate(CarContractDataModel data, string templatePdfPath, string outputFolder)
        {
            try
            {
                if (!File.Exists(templatePdfPath))
                    throw new FileNotFoundException($"Шаблон PDF не найден: {templatePdfPath}");

                // Создаем имя файла
                string fileName = $"ДКП_{data.CarBrand}_{data.CarModel}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string outputPath = Path.Combine(outputFolder, fileName);

                // Создаем папку если не существует
                Directory.CreateDirectory(outputFolder);

                // Заполняем PDF форму
                FillPdfFormFields(templatePdfPath, outputPath, data);

                return outputPath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка заполнения PDF: {ex.Message}", ex);
            }
        }

        private void FillPdfFormFields(string templatePath, string outputPath, CarContractDataModel data)
        {
            PdfReader reader = null;
            FileStream fs = null;
            PdfStamper stamper = null;

            try
            {
                // Открываем шаблон
                reader = new PdfReader(templatePath);

                // Создаем выходной файл
                fs = new FileStream(outputPath, FileMode.Create);

                // Создаем stamper для модификации
                stamper = new PdfStamper(reader, fs);

                // Получаем поля формы из PDF
                AcroFields form = stamper.AcroFields;

                // Включаем генерацию внешнего вида для кириллицы
                form.GenerateAppearances = true;

                // Устанавливаем шрифт для всех полей
                if (_russianFont != null)
                {
                    // Создаем объект Font для использования в полях
                    var font = new iTextSharp.text.Font(_russianFont, 12);

                    // Альтернативный способ: устанавливаем шрифт для каждого поля
                    SetFieldFonts(form, _russianFont);
                }

                // Заполняем все поля данными
                FillAllFormFields(form, data);

                // Делаем форму неизменяемой (flatten)
                stamper.FormFlattening = true;

                stamper.Close();
                reader = null;
            }
            finally
            {
                stamper?.Close();
                reader?.Close();
                fs?.Close();
            }
        }

        // Устанавливаем шрифт для всех полей формы
        private void SetFieldFonts(AcroFields form, BaseFont font)
        {
            foreach (string fieldName in form.Fields.Keys)
            {
                try
                {
                    // Получаем тип поля
                    int fieldType = form.GetFieldType(fieldName);

                    // Создаем новый шрифт для поля
                    form.SetFieldProperty(
                        fieldName,
                        "textfont",
                        font,
                        null);

                    // Устанавливаем размер шрифта
                    form.SetFieldProperty(
                        fieldName,
                        "textsize",
                        12f, // Размер шрифта
                        null);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка настройки шрифта для поля {fieldName}: {ex.Message}");
                }
            }
        }

        // Заполнение всех полей формы
        private void FillAllFormFields(AcroFields form, CarContractDataModel data)
        {
            // 1. Информация о договоре
            SetField(form, "Text1", data.CarDealershipCity);
            SetField(form, "Text2", data.ContractDay);
            SetField(form, "Text3", data.ContractMonth);
            SetField(form, "Text4", data.ContractYear);

            // 2. Данные покупателя
            SetField(form, "Text5", data.CustomerFullName);

            // 3. Данные автомобиля
            SetField(form, "Text6", data.CarBrand);
            SetField(form, "Text7", data.CarModel);
            SetField(form, "Text8", data.CarVIN);
            SetField(form, "Text9", data.CarReleaseYear);
            SetField(form, "Text10", data.CarMileage);
            SetField(form, "Text11", data.CarEngineVolume);
            SetField(form, "Text12", data.CarEnginePower);
            SetField(form, "Text13", data.CarEngineType);
            SetField(form, "Text14", data.CarTransmissionType);
            SetField(form, "Text15", data.CarDriveType);
            SetField(form, "Text16", data.CarBodyType);
            SetField(form, "Text17", data.CarColor);

            // 4. Финансовая информация
            SetField(form, "Text18", data.FormattedPrice);
        }

        private void SetField(AcroFields form, string fieldName, string value)
        {
            if (form.Fields.ContainsKey(fieldName))
            {
                try
                {
                    form.SetField(fieldName, value ?? "");
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка заполнения полей PDF");
                }
            }
            else
            {
                throw new Exception($"Поле '{fieldName}' не найдено в шаблоне");
            }
        }

        public string GenerateContractPdf(CarContractDataModel contractData, string outputDirectory)
        {
            // Создаем директорию, если она не существует
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Генерируем имя файла на основе данных договора
            string fileName = $"Договор_купли_продажи_{contractData.CarBrand}_{contractData.CarModel}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(outputDirectory, fileName);

            // Создаем документ
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Настройка шрифтов
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font titleFont = new Font(baseFont, 16, Font.BOLD);
            Font headerFont = new Font(baseFont, 12, Font.BOLD);
            Font normalFont = new Font(baseFont, 10, Font.NORMAL);
            Font smallFont = new Font(baseFont, 9, Font.NORMAL);

            // Заголовок документа
            Paragraph title = new Paragraph("ДОГОВОР КУПЛИ-ПРОДАЖИ АВТОМОБИЛЯ", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20;
            document.Add(title);

            // Место и дата заключения договора
            Paragraph locationDate = new Paragraph($"г. {contractData.CarDealershipCity} \"{contractData.ContractDay}\" {contractData.ContractMonth} {contractData.ContractYear} г.", normalFont);
            locationDate.Alignment = Element.ALIGN_RIGHT;
            locationDate.SpacingAfter = 20;
            document.Add(locationDate);

            // Основной текст договора
            string contractText = @"
Мы, нижеподписавшиеся:

Продавец: Автосалон '" + contractData.CarDealershipCity + @"', в лице уполномоченного представителя,

и

Покупатель: " + contractData.CustomerFullName + @", действующий(ая) от своего имени,

заключили настоящий договор о нижеследующем:

1. ПРЕДМЕТ ДОГОВОРА

1.1. Продавец обязуется передать в собственность Покупателю, а Покупатель обязуется принять и оплатить следующий автомобиль:";

            Paragraph text1 = new Paragraph(contractText, normalFont);
            text1.Alignment = Element.ALIGN_JUSTIFIED;
            text1.SpacingAfter = 10;
            document.Add(text1);

            // Таблица с характеристиками автомобиля
            PdfPTable carTable = new PdfPTable(2);
            carTable.WidthPercentage = 100;
            carTable.SpacingAfter = 10;

            // Настройка ширины колонок
            float[] widths = new float[] { 40f, 60f };
            carTable.SetWidths(widths);

            AddCarTableRow(carTable, "Марка:", contractData.CarBrand, normalFont);
            AddCarTableRow(carTable, "Модель:", contractData.CarModel, normalFont);
            AddCarTableRow(carTable, "Идентификационный номер (VIN):", contractData.CarVIN, normalFont);
            AddCarTableRow(carTable, "Год выпуска:", contractData.CarReleaseYear, normalFont);
            AddCarTableRow(carTable, "Пробег, км:", contractData.CarMileage, normalFont);
            AddCarTableRow(carTable, "Объем двигателя, л:", contractData.CarEngineVolume, normalFont);
            AddCarTableRow(carTable, "Мощность двигателя, л.с.:", contractData.CarEnginePower, normalFont);
            AddCarTableRow(carTable, "Тип двигателя:", contractData.CarEngineType, normalFont);
            AddCarTableRow(carTable, "Тип трансмиссии:", contractData.CarTransmissionType, normalFont);
            AddCarTableRow(carTable, "Привод:", contractData.CarDriveType, normalFont);
            AddCarTableRow(carTable, "Тип кузова:", contractData.CarBodyType, normalFont);
            AddCarTableRow(carTable, "Цвет:", contractData.CarColor, normalFont);

            document.Add(carTable);

            // Продолжение текста договора
            string contractText2 = @"

2. ЦЕНА И ПОРЯДОК РАСЧЕТОВ

2.1. Стоимость автомобиля составляет: " + contractData.FormattedPrice + @" руб.

2.2. Покупатель оплачивает стоимость автомобиля в полном объеме в день подписания настоящего договора.

3. ПЕРЕДАЧА АВТОМОБИЛЯ

3.1. Продавец передает Покупателю автомобиль в день подписания настоящего договора.

3.2. Автомобиль передается по акту приема-передачи, который является неотъемлемой частью настоящего договора.

4. ПРАВА И ОБЯЗАННОСТИ СТОРОН

4.1. Продавец гарантирует, что на момент заключения договора автомобиль не заложен, не арестован, не является предметом судебного спора.

4.2. Покупатель принимает автомобиль в том состоянии, в котором он находится на момент передачи.

5. ПРОЧИЕ УСЛОВИЯ

5.1. Настоящий договор составлен в двух экземплярах, имеющих одинаковую юридическую силу, по одному для каждой из сторон.

5.2. Все споры решаются путем переговоров, а при невозможности достижения согласия - в судебном порядке.

6. ПОДПИСИ СТОРОН

ПОКУПАТЕЛЬ:                                    ПРОДАВЕЦ:

_____________________                   _____________________
(подпись)                                             (подпись)

_____________________                   _____________________
(Ф.И.О.)                                                (Ф.И.О.)

Паспорт: _______________
Выдан: _________________
Дата выдачи: ___________
Адрес: _________________

Дата передачи: ________________________
";

            Paragraph text2 = new Paragraph(contractText2, normalFont);
            text2.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(text2);

            // Добавляем нумерацию страниц
            AddPageNumbers(writer, document, smallFont);

            document.Close();

            return filePath;
        }

        public string GenerateTradeInContractPdf(CarContractDataModel contractData, string outputDirectory)
        {
            // Создаем директорию, если она не существует
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Генерируем имя файла на основе данных договора
            string fileName = $"Договор_купли_продажи_трейд_ин_{contractData.CarBrand}_{contractData.CarModel}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(outputDirectory, fileName);

            // Создаем документ
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Настройка шрифтов
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font titleFont = new Font(baseFont, 16, Font.BOLD);
            Font headerFont = new Font(baseFont, 12, Font.BOLD);
            Font normalFont = new Font(baseFont, 10, Font.NORMAL);
            Font smallFont = new Font(baseFont, 9, Font.NORMAL);

            // Заголовок документа
            Paragraph title = new Paragraph("ДОГОВОР КУПЛИ-ПРОДАЖИ АВТОМОБИЛЯ", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20;
            document.Add(title);

            // Место и дата заключения договора
            Paragraph locationDate = new Paragraph($"г. {contractData.CarDealershipCity} \"{contractData.ContractDay}\" {contractData.ContractMonth} {contractData.ContractYear} г.", normalFont);
            locationDate.Alignment = Element.ALIGN_RIGHT;
            locationDate.SpacingAfter = 20;
            document.Add(locationDate);

            // Основной текст договора
            string contractText = @"
Мы, нижеподписавшиеся:

Продавец: " + contractData.CustomerFullName + @", действующий(ая) от своего имени,

и

Покупатель: Автосалон '" + contractData.CarDealershipCity + @"', в лице уполномоченного представителя,

заключили настоящий договор о нижеследующем:

1. ПРЕДМЕТ ДОГОВОРА

1.1. Продавец обязуется передать в собственность Покупателю, а Покупатель обязуется принять и оплатить следующий автомобиль:";

            Paragraph text1 = new Paragraph(contractText, normalFont);
            text1.Alignment = Element.ALIGN_JUSTIFIED;
            text1.SpacingAfter = 10;
            document.Add(text1);

            // Таблица с характеристиками автомобиля
            PdfPTable carTable = new PdfPTable(2);
            carTable.WidthPercentage = 100;
            carTable.SpacingAfter = 10;

            // Настройка ширины колонок
            float[] widths = new float[] { 40f, 60f };
            carTable.SetWidths(widths);

            AddCarTableRow(carTable, "Марка:", contractData.CarBrand, normalFont);
            AddCarTableRow(carTable, "Модель:", contractData.CarModel, normalFont);
            AddCarTableRow(carTable, "Идентификационный номер (VIN):", contractData.CarVIN, normalFont);
            AddCarTableRow(carTable, "Год выпуска:", contractData.CarReleaseYear, normalFont);
            AddCarTableRow(carTable, "Пробег, км:", contractData.CarMileage, normalFont);
            AddCarTableRow(carTable, "Объем двигателя, л:", contractData.CarEngineVolume, normalFont);
            AddCarTableRow(carTable, "Мощность двигателя, л.с.:", contractData.CarEnginePower, normalFont);
            AddCarTableRow(carTable, "Тип двигателя:", contractData.CarEngineType, normalFont);
            AddCarTableRow(carTable, "Тип трансмиссии:", contractData.CarTransmissionType, normalFont);
            AddCarTableRow(carTable, "Привод:", contractData.CarDriveType, normalFont);
            AddCarTableRow(carTable, "Тип кузова:", contractData.CarBodyType, normalFont);
            AddCarTableRow(carTable, "Цвет:", contractData.CarColor, normalFont);

            document.Add(carTable);

            // Продолжение текста договора
            string contractText2 = @"

2. ЦЕНА И ПОРЯДОК РАСЧЕТОВ

2.1. Стоимость автомобиля составляет: " + contractData.FormattedPrice + @" руб.

2.2. Покупатель оплачивает стоимость автомобиля в полном объеме в день подписания настоящего договора.

3. ПЕРЕДАЧА АВТОМОБИЛЯ

3.1. Продавец передает Покупателю автомобиль в день подписания настоящего договора.

3.2. Автомобиль передается по акту приема-передачи, который является неотъемлемой частью настоящего договора.

4. ПРАВА И ОБЯЗАННОСТИ СТОРОН

4.1. Продавец гарантирует, что на момент заключения договора автомобиль не заложен, не арестован, не является предметом судебного спора.

4.2. Покупатель принимает автомобиль в том состоянии, в котором он находится на момент передачи.

5. ПРОЧИЕ УСЛОВИЯ

5.1. Настоящий договор составлен в двух экземплярах, имеющих одинаковую юридическую силу, по одному для каждой из сторон.

5.2. Все споры решаются путем переговоров, а при невозможности достижения согласия - в судебном порядке.

6. ПОДПИСИ СТОРОН

ПРОДАВЕЦ:                                        ПОКУПАТЕЛЬ:

_____________________                   _____________________
(подпись)                                             (подпись)

_____________________                   _____________________
(Ф.И.О.)                                                (Ф.И.О.)

Паспорт: _______________
Выдан: _________________
Дата выдачи: ___________
Адрес: _________________

Дата передачи: ________________________
";

            Paragraph text2 = new Paragraph(contractText2, normalFont);
            text2.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(text2);

            // Добавляем нумерацию страниц
            AddPageNumbers(writer, document, smallFont);

            document.Close();

            return filePath;
        }

        private void AddCarTableRow(PdfPTable table, string label, string value, Font font)
        {
            PdfPCell labelCell = new PdfPCell(new Phrase(label, font));
            labelCell.Border = PdfPCell.NO_BORDER;
            labelCell.Padding = 5;
            labelCell.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell valueCell = new PdfPCell(new Phrase(value, font));
            valueCell.Border = PdfPCell.NO_BORDER;
            valueCell.Padding = 5;
            valueCell.HorizontalAlignment = Element.ALIGN_LEFT;

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        private void AddPageNumbers(PdfWriter writer, Document document, Font font)
        {
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footer.DefaultCell.Border = PdfPCell.NO_BORDER;
            footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(new Phrase($"Страница {writer.PageNumber}", font));
            cell.Border = PdfPCell.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.AddCell(cell);

            footer.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
        }
    }
}
