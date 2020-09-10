using BigFilesSort.Models;
using System;

namespace BigFilesSort.ProgramConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите номер операции (1-сортировка; 2-создание): ");
            var op = Console.ReadLine();
            if (op == "1")
            {
                Console.Write("Введите путь к файлу: ");
                var path = Console.ReadLine();
                var time = DateTime.Now;
                FileSorter.Sort(path, 100_000_000, 100_000_000);
                Console.Write($"Завершено за {(DateTime.Now - time).TotalSeconds} сек.!");
            }
            else
            {
                Console.Write("Введите путь к файлу: ");
                var path = Console.ReadLine();
                var time = DateTime.Now;
                FileGenerator.CreateRandomIntFile(path, 150_000_000);
                Console.Write($"Завершено за {(DateTime.Now - time).TotalSeconds} сек.!");
            }
            Console.ReadKey();
        }
    }
}
