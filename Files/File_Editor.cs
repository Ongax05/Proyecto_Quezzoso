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

                    HashSet<int> UsedNumbers = GetUsedNumbers(Directory, DirectoryName);

                    int NextNumber = 1;
                    while (UsedNumbers.Contains(NextNumber))
                    {
                        NextNumber++;
                    }

                    string NewFilePath = Path.Combine(Directory, $"{DirectoryName}_{NextNumber:D3}" + FileExtension);

                    // Marcar como usado
                    UsedNumbers.Add(NextNumber);

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
        private static void ConsoleWait()
        {
            Console.ReadLine();
            Console.Clear();
        }

        private static HashSet<int> GetUsedNumbers(string DirectoryPath, string DirectoryName)
        {
            HashSet<int> Numbers = new();

            var Files = Directory.GetFiles(DirectoryPath);

            foreach (var File in Files)
            {
                string Name = Path.GetFileNameWithoutExtension(File);

                if (Name.StartsWith(DirectoryName + "_"))
                {
                    string NumberPart = Name.Replace(DirectoryName + "_", "");

                    if (int.TryParse(NumberPart, out int Num))
                    {
                        Numbers.Add(Num);
                    }
                }
            }

            return Numbers;
        }

        public static void DetectDuplicateFiles(string MasterFolder)
        {
            Console.WriteLine("\nBuscando archivos duplicados por tamaño...\n");

            Dictionary<long, List<string>> FilesBySize = new();

            var AllFiles = Directory.GetFiles(MasterFolder, "*", SearchOption.AllDirectories);

            foreach (var FilePath in AllFiles)
            {
                long Size = new FileInfo(FilePath).Length;

                if (!FilesBySize.ContainsKey(Size))
                {
                    FilesBySize[Size] = new List<string>();
                }

                FilesBySize[Size].Add(FilePath);
            }

            bool FoundDuplicates = false;

            foreach (var Group in FilesBySize)
            {
                if (Group.Value.Count > 1)
                {
                    FoundDuplicates = true;

                    Console.WriteLine($"Archivos con tamaño {Group.Key} bytes:");

                    foreach (var File in Group.Value)
                    {
                        Console.WriteLine(File);
                    }

                    Console.WriteLine();
                }
            }

            if (!FoundDuplicates)
            {
                Console.WriteLine("No se encontraron archivos duplicados por tamaño.");
            }
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