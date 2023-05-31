using QRCoder;
using SepaQr;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomlyn;
using Tomlyn.Model;
using System.Text.RegularExpressions;

namespace TomlToEpcQrCode
{
    public class Helper
    {
        public Dictionary<string, QRCode> tomlToEpcQrCode(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string content = File.ReadAllText(filePath);
            TomlTable model = Toml.ToModel(content);

            //Get receiver information
            TomlTable receiver = (TomlTable)model["receiver"];
            string name = (string)receiver["name"];
            string iban = (string)receiver["iban"];
            string bic = (string)receiver["bic"];

            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(iban) || String.IsNullOrEmpty(bic))
                throw new ArgumentException();

            //Generate Receiver QR Code information
            SepaQrCode qrCodeInfo = new SepaQrCode();
            qrCodeInfo.SetName(name);
            qrCodeInfo.SetAccountNumber(iban);
            qrCodeInfo.SetBic(bic);

            //Get payments 
            Dictionary<string, QRCode> qrCodes = new Dictionary<string, QRCode>();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            TomlTableArray payments = (TomlTableArray)model["payments"];

            foreach (TomlTable payment in payments)
            {
                //Generate payment info in qrCode
                string reference = (string)payment["reference"];
                decimal amount = (decimal)(double)payment["amount"];

                qrCodeInfo.SetStructuredRemittanceInformation(reference);
                qrCodeInfo.SetAmount(amount);

                //Generate QR Code
                string qrCodeContent = qrCodeInfo.GetQrContent();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.M); // ECC level needs to be "M" per specification
                QRCode qrCode = new QRCode(qrCodeData);

                //Add to list
                string qrCodeName = reference + "_" + amount;
                qrCodes.Add(qrCodeName, qrCode);
            }

            return qrCodes;
        }

        public void saveQrCodesAsJpeg(Dictionary<string, QRCode> qrCodes, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine($"Folder created: {folderPath}");
            }

            foreach (KeyValuePair<string, QRCode> qrCode in qrCodes)
            {
                Bitmap qrCodeImage = qrCode.Value.GetGraphic(3);
                string fileName = qrCode.Key + ".jpeg";
                string filePath = folderPath + "\\" + fileName;
                qrCodeImage.Save(filePath, ImageFormat.Jpeg);
                Console.WriteLine($"QrCode created: {fileName}");
            }
        }
    }
}
