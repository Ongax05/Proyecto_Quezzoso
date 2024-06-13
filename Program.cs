using Proyecto_Quezzoso.Files;
using System.Text;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.Write("Enter master folder path : ");
            string MasterFolderPath = Console.ReadLine();
            MasterFolderPath = MasterFolderPath.Replace("\"", "");

            if (!Directory.Exists(MasterFolderPath))
            {
                Console.Clear();
                Console.WriteLine($"Master folder does not exist");
                Console.ReadLine();
            }
            else
            {
                Console.Clear();
                File_Editor.RenameFiles(MasterFolderPath);
                break;
            }
        }
    }
}