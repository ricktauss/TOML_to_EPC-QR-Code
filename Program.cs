using QRCoder;
using TomlToEpcQrCode;

internal class Program
{
    private static void Main(string[] args)
    {
        Helper helper = new Helper();

        try
        {
            //Input information
            string path = Directory.GetCurrentDirectory();
            Console.WriteLine($"Searching for .toml files in {path} ...");
            string[] files = Directory.GetFiles(path, "*.toml", SearchOption.AllDirectories);

            if (files.Length <= 0)
                throw new FileNotFoundException("Create TOML files in \"Payment\"-Folder according to template in repository or ReadMe-File");

            Console.WriteLine($"{files.Length} Files found:");
            Array.ForEach(files, file => Console.WriteLine(file));

            Dictionary<string, QRCode> qrCodes = new Dictionary<string, QRCode>();
    
            foreach (string file in files)
            {
                qrCodes = helper.tomlToEpcQrCode(file);
                helper.saveQrCodesAsJpeg(qrCodes, path + "\\QR-Codes");
            }

            Console.WriteLine("Program finished ... Press any key to continue");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n" + ex + "\n");
            Console.WriteLine("Program failed ... Press any key to continue");
            Console.ReadKey();
        }
    }
}