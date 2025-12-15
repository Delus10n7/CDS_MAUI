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

        // Заполнение всех полей формы
        private void FillAllFormFields(AcroFields form, CarContractDataModel data)
        {
            // 1. Информация о договоре
            SetField(form, "car_dealership_city", data.CarDealershipCity);
            SetField(form, "order_day", data.ContractDay);
            SetField(form, "order_month", data.ContractMonth);
            SetField(form, "order_year", data.ContractYear);

            // 2. Данные покупателя
            SetField(form, "customer_fullname", data.CustomerFullName);

            // 3. Данные автомобиля
            SetField(form, "car_brand", data.CarBrand);
            SetField(form, "car_model", data.CarModel);
            SetField(form, "car_vin", data.CarVIN);
            SetField(form, "car_release_year", data.CarReleaseYear);
            SetField(form, "car_mileage", data.CarMileage);
            SetField(form, "car_engine_volume", data.CarEngineVolume);
            SetField(form, "car_engine_power", data.CarEnginePower);
            SetField(form, "car_engine_type", data.CarEngineType);
            SetField(form, "car_transmission_type", data.CarTransmissionType);
            SetField(form, "car_drive_type", data.CarDriveType);
            SetField(form, "car_body_type", data.CarBodyType);
            SetField(form, "car_color", data.CarColor);

            // 4. Финансовая информация
            SetField(form, "order_saleprice", data.FormattedPrice);
        }

        private void SetField(AcroFields form, string fieldName, string value)
        {
            if (form.Fields.ContainsKey(fieldName))
            {
                form.SetField(fieldName, value ?? "");
            }
            else
            {
                // Для отладки - логируем отсутствующие поля
                System.Diagnostics.Debug.WriteLine($"Поле '{fieldName}' не найдено в шаблоне");
            }
        }
    }
}
