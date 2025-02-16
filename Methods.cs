using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace FileManager
{
    public static class Methods
    {
        public delegate void Action(FileInfo file, DateTime time);
        public static void CreateFiles(string dirPath, int numOfFile)
        {
            for (int i = 1; i <= numOfFile; i++)
            {
                using (File.Create(Path.Combine(dirPath, $"File{i}.txt"))) { }
            }
        }
        public static void ActionsWithFiles(string dirPath, Action action, DateTime time = default)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    try
                    {
                        action(file, time);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        throw new UnauthorizedAccessException($"Нет прав на доступ к файлу {file.FullName}", ex);
                    }
                    catch (IOException ex)
                    {
                        throw new IOException($"Ошибка при работе с файлом {file.FullName}: {ex.Message}", ex);
                    }
                }
            }
        }
        public static void PrintInfo(FileInfo file, DateTime time = default)
        {
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                string lineForPrint = $"{file.Name}:";
                string tempLine;
                while ((tempLine = sr.ReadLine()) != null)
                {
                    lineForPrint += $" {tempLine}";
                }
                Console.WriteLine(lineForPrint);
            }
        }

        public static void WriteToFile(FileInfo file, DateTime time)
        {
            FileSecurity fileSecurity = file.GetAccessControl();
            AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));

            bool hasRight = false;
            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.AccessControlType == AccessControlType.Allow)
                {
                    hasRight = true;
                    break;
                }
            }
            if (!hasRight)
            {
                throw new UnauthorizedAccessException();
            }

            using (StreamWriter sw = new StreamWriter(file.FullName, true, Encoding.UTF8))
            {
                if (time == default)
                {
                    sw.WriteLine(file.Name);
                }
                else
                {
                    sw.WriteLine(time);
                }
            }
        }
    }
}
