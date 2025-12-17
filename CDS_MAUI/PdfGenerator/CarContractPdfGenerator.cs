using CDS_MAUI.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

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
    }
}
