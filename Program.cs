using static FileManager.Methods;

namespace FileManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dirPath_1 = @"c:\Otus\TestDir1";
            string dirPath_2 = @"c:\Otus\TestDir2";

            DirectoryInfo testDir_1 = new DirectoryInfo(dirPath_1);
            DirectoryInfo testDir_2 = new DirectoryInfo(dirPath_2);

            if (!testDir_1.Exists)
                testDir_1.Create();

            if (!testDir_2.Exists)
                testDir_2.Create();

            CreateFiles(dirPath_1, 10);
            CreateFiles(dirPath_2, 10);
            try
            {
                ActionsWithFiles(dirPath_1, WriteToFile);
                ActionsWithFiles(dirPath_2, WriteToFile);

                ActionsWithFiles(dirPath_1, WriteToFile, DateTime.Now);
                ActionsWithFiles(dirPath_2, WriteToFile, DateTime.Now);

                ActionsWithFiles(dirPath_1, PrintInfo);
                ActionsWithFiles(dirPath_2, PrintInfo);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oшибка: {ex.Message}");
            }
        }
    }
}
