namespace Proyecto_Quezzoso.Files
{
    public static class File_Editor
    {
        /// <summary>
        /// Renombra los archivos con el nombre de su carpeta contenedora y un codigo manteniendo su extensión original (incluye los archivos de las sub carpetas).
        /// </summary>
        /// <param name="Master_Folder">La ruta completa de la carpeta raiz.</param>
        public static void RenameFiles(string Master_Folder)
        {

            List<Folder_Group> Folder_Groups = GetFolderGroups(Master_Folder);

            int ProcessedGroups = 0;

            foreach (Folder_Group FolderGroup in Folder_Groups)
            {
                Console.Clear();
                    
                int ProcessedCorrectly = 0;
                int TotalFiles = FolderGroup.File_Paths.Count;

                for (int i = 0; i < TotalFiles; i++)
                {
                    string FileExtension = Path.GetExtension(FolderGroup.File_Paths[i]);

                    string Directory = Path.GetDirectoryName(FolderGroup.File_Paths[i]);

                    string DirectoryName = Path.GetFileName(Directory);

                    string NewFilePath = Path.Combine(Directory, $"{DirectoryName}_{i + 1:D3}" + FileExtension);

                    try
                    {
                        File.Move(FolderGroup.File_Paths[i], NewFilePath);
                        Console.WriteLine("Archivo renombrado con éxito a: " + NewFilePath + "\n");
                        ProcessedCorrectly++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError al renombrar el archivo: " + NewFilePath + $"\tException ({ex})\n");
                    }
                }

                ProcessedGroups++;

                ConsoleWait();

                if (ProcessedCorrectly == TotalFiles)
                {
                    Console.WriteLine($"({ProcessedCorrectly}/{TotalFiles}) Archivos procesados con exito (⌐■_■)ᕗ");
                }
                else
                {
                    Console.WriteLine($"({ProcessedCorrectly}/{TotalFiles}) Archivos procesados {ProcessedCorrectly}/{TotalFiles} (҂◡_◡) ᕤ");
                }

                ConsoleWait();
            }

            Console.WriteLine($"({ProcessedGroups}/{Folder_Groups.Count}) Carpetas procesadas con exito (⌐■_■)ᕗ");
        }

        private static List<string> GetFolderPaths(string Master_Folder)
        {
            List<string> FolderPaths = [.. Directory.GetDirectories(Master_Folder, "*", SearchOption.AllDirectories)];
            FolderPaths.Add(Master_Folder);

            return FolderPaths;
        }
        private static List<Folder_Group> GetFolderGroups(string Master_Folder)
        {
            List<Folder_Group> Folder_Groups = [];

            List<string> FolderPaths = GetFolderPaths(Master_Folder);

            foreach (string FolderPath in FolderPaths)
            {
                List<string> FileNames = [.. Directory.GetFiles(FolderPath)];

                Folder_Groups.Add(new(FolderPath, FileNames));
            }

            return Folder_Groups;
        }
        private static void ConsoleWait ()
        {
            Console.ReadLine();
            Console.Clear();
        }
    }

    public record Folder_Group
    {
        public Folder_Group(string folderName, List<string> filePaths)
        {
            Folder_Name = folderName;
            File_Paths = filePaths;
        }
        public string Folder_Name { get; init; }
        public List<string> File_Paths { get; set; }
    }
}