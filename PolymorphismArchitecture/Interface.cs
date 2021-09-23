using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolymorphismArchitecture.Enum;
using PolymorphismArchitecture.Log;
using PolymorphismArchitecture.Serializaion;
using PolymorphismArchitecture.Storages;

namespace DzNet6
{
    class Interface
    {
        private readonly ILog _logger;
        private readonly ISerialize _serializer;
        private readonly PriceList _priceList;

        public Interface()
        {
            _priceList = new PriceList();
            _logger = new ConsoleLogger();
            //_logger = new FileLogger("bro.txt");
            _serializer = new BinarySerialization();
        }
        public void Menu()
        {
            var choice = GetCorrectMenuOption(ShowProgramMenu, 1, 7);
            Console.Clear();
            switch (choice)
            {
                case 1:
                    {
                        AddInformationCarrier();
                        Menu();
                    }
                    break;
                case 2:
                    {
                        var found = Search().ToList();
                        if (!found.Any())
                        {
                            Console.Clear();
                            Console.WriteLine("Ничего не найдено.");
                            Console.ReadKey(true);
                            Menu();
                        }
                        Remove(found);
                        Menu();
                    }
                    break;
                case 3:
                    {
                        var found = Search().ToList();
                        if (!found.Any())
                        {
                            Console.Clear();
                            Console.WriteLine("Ничего не найдено.");
                            Console.ReadKey(true);
                            Menu();
                        }
                        Edit(found);
                        Menu();
                    }
                    break;
                case 4:
                    {
                        Print();
                        Console.ReadKey(true);
                        Menu();
                    }
                    break;
                case 5:
                    {
                        new Action(Save).BeginInvoke(null, null);
                        Menu();
                    }
                    break;
                case 6:
                    {
                        new Action(Load).BeginInvoke(null, null);
                        Menu();
                    }
                    break;
                case 7:
                    {
                        var found = Search().ToList();
                        if (!found.Any())
                        {
                            Console.Clear();
                            Console.WriteLine("Ничего не найдено.");
                            Console.ReadKey(true);
                            Menu();
                        }
                        Console.Clear();
                        Print(found);
                        Console.ReadKey(true);
                        Menu();
                    }
                    break;
            }
        }

        private void Load()
        {
            _priceList.Load(_serializer);
        }
        private void Save()
        {
            _priceList.Save(_serializer);
        }
        private void Edit(Storage storage)
        {
            Console.WriteLine("\t\tИсходный обьект");
            Print(storage);
            Console.WriteLine();

            var manufacturer = GetCorrectStringInput("Введите Имя Производителя: ");
            var model = GetCorrectStringInput("Введите Модель: ");
            var name = GetCorrectStringInput("Введите имя: ");
            var count = GetCorrectInt("Введите кол-во: ", 1, 100000);
            var capacity = GetCorrectDouble("Обьем: ", 1, 100000);

            storage.Manufacturer = manufacturer;
            storage.Model = model;
            storage.Name = name;
            storage.Count = count;
            storage.Capacity = capacity;

            if (storage is Dvd dvd)
            {
                dvd.ReadSpeed = GetCorrectInt("Введите скорость считывания: ", 1, 100000);
                dvd.WriteSpeed = GetCorrectInt("Введите скорость записи: ", 1, 100000);
            }
            else
            {
                if (storage is HDD hdd)
                {
                    hdd.Speed = GetCorrectInt("Введите скорость: ", 1, 100000);
                    hdd.Size = GetCorrectInt("Введите размер: ", 1, 100000);
                }
                else
                {
                    if (storage is Flash flash)
                    {
                        flash.Speed = GetCorrectInt("Введите скорость: ", 1, 100000);
                        flash.TotalMemory = GetCorrectInt("Введите кол-во памяти: ", 1, 100000);
                    }
                }
            }
        }
        private void Edit(List<Storage> storages)
        {
            if (storages.Count() > 1)
            {
                Console.Clear();
                Print(storages);
                var choice = Console.ReadLine();
                Console.WriteLine("Сузьте кол-во носителей до 1.");
                var index = GetCorrectInt("Введите id: ", 1, storages.Count);

                Edit(storages[index - 1]);
            }
            else if (storages.Count == 1)
            {
                Edit(storages[0]);
            }
        }
        private void Remove(List<Storage> storages)
        {
            if (storages.Count() > 1)
            {
                Console.Clear();
                Print(storages);
                Console.Write("Удалить всех?(Y/N) ");
                var choice = Console.ReadLine();
                if (choice != null && (choice.Equals("Y") || choice.Equals("y")))
                {
                    _priceList.RemoveSequence(storages);
                    Console.WriteLine("Носители удалены.");
                    Console.ReadLine();
                }
                else if (choice != null && (choice.Equals("N") || choice.Equals("n")))
                {
                    Console.WriteLine("Сузьте кол-во носителей до 1.");
                    var index = GetCorrectInt("Введите id: ", 1, storages.Count);
                    if (_priceList.Remove(storages[index - 1]))
                    {
                        Console.WriteLine("Носитель удален.");
                    }
                    Console.ReadLine();
                }
            }
            else if (storages.Count == 1)
            {
                if (_priceList.Remove(storages[0]))
                {
                    Console.WriteLine("Носитель удален.");
                }
                Console.ReadLine();
            }
        }
        private IEnumerable<Storage> Search()
        {
            var names = System.Enum.GetNames(typeof(SearchCriterion));
            var choiceCriteria = GetCorrectMenuOption(ShowSearchCriterion, 1, names.Length);
            Console.Clear();
            string lookFor = GetCorrectStringInput($"{names[choiceCriteria - 1]}: ");
            SearchCriterion criterion = (SearchCriterion)System.Enum.Parse(typeof(SearchCriterion), names[choiceCriteria - 1]);
            var found = _priceList.Search(criterion, lookFor);
            return found;
        }

        private void AddInformationCarrier()
        {
            Console.WriteLine("1.DVD");
            Console.WriteLine("2.HDD");
            Console.WriteLine("3.Flash");

            var choice = GetCorrectInt("Ваш выбор: ", 1, 3);

            var manufacturer = GetCorrectStringInput("Производитель: ");
            var model = GetCorrectStringInput("Модель: ");
            var name = GetCorrectStringInput("Имя: ");
            var capacity = GetCorrectInt("Обьем: ", 1, 10000);
            var count = GetCorrectInt("Кол-во: ", 1, 10000);
            int speed;

            switch (choice)
            {
                case 1:
                    var readSpeed = GetCorrectInt("Скорость чтения: ", 1, 10000);
                    var writeSpeed = GetCorrectInt("Скорость записи: ", 1, 10000);

                    _priceList.AddDVD(manufacturer, model, name, capacity, count, readSpeed, writeSpeed);
                    break;
                case 2:
                    speed = GetCorrectInt("Скорость: ", 1, 10000);
                    var size = GetCorrectInt("Размер: ", 1, 10000);

                    _priceList.AddHDD(manufacturer, model, name, capacity, count, speed, size);
                    break;
                case 3:
                    speed = GetCorrectInt("Скорость: ", 1, 10000);
                    var totalMemory = GetCorrectInt("Всего памяти: ", 1, 10000);

                    _priceList.AddFlash(manufacturer, model, name, capacity, count, speed, totalMemory);
                    break;
            }
        }

        #region Print Methods
        private void Print(IEnumerable<Storage> st)
        {
            int i = 1;
            foreach (var storage in st)
            {
                Console.WriteLine($"\t{i++}");
                storage.Print(_logger);
                Console.WriteLine();
            }
            Console.WriteLine($"Найдено {i - 1} записей.");
        }
        private void Print(Storage storage)
        {
            Console.WriteLine(storage);
        }
        private void Print()
        {
            _priceList.Print(_logger);
            Console.WriteLine();
            Console.WriteLine($"Найдено {_priceList.Count()} записей.");
        }

        #endregion
        #region Show Methods
        private static void ShowSearchCriterion()
        {
            Console.Clear();
            Console.WriteLine("\t\tКритерий поиска");
            var names = System.Enum.GetNames(typeof(SearchCriterion));
            int i = 1;
            foreach (var name in names)
            {
                Console.WriteLine($"{i++}.{name}");
            }
        }
        private static void ShowProgramMenu()
        {
            Console.Clear();
            Console.WriteLine("\t\tМеню");
            Console.WriteLine("1.Добавить");
            Console.WriteLine("2.Удалить");
            Console.WriteLine("3.Редактировать");
            Console.WriteLine("4.Печать");
            Console.WriteLine("5.Сохранить");
            Console.WriteLine("6.Загрузить");
            Console.WriteLine("7.Найти");
        }
        #endregion
        #region Help Methods
        private static int GetCorrectMenuOption(Action menuText, int min, int max)
        {
            var menuId = 0;
            var isCorrect = false;
            Console.Clear();
            while (!isCorrect)
            {

                menuText();
                Console.WriteLine();
                Console.Write("\tНомер: ");
                if (int.TryParse(Console.ReadLine(), out menuId))
                {
                    if (menuId >= min && menuId <= max)
                    {
                        isCorrect = true;
                    }
                }
                else
                {
                    Console.Clear();
                }
            }
            return menuId;
        }
        private static string GetCorrectStringInput(string request)
        {
            var field = string.Empty;
            while (string.IsNullOrEmpty(field) || string.IsNullOrWhiteSpace(field))
            {
                Console.Write(request);
                field = Console.ReadLine();
            }
            return field;
        }
        private static int GetCorrectInt(string request, int min, int max)
        {
            var choice = 0;
            var isCorrect = false;
            while (!isCorrect)
            {
                var input = GetCorrectStringInput(request);
                if (int.TryParse(input, out choice))
                {
                    if (choice >= min && choice <= max)
                    {
                        isCorrect = true;
                    }
                }
            }
            return choice;
        }
        private static double GetCorrectDouble(string request, double min, double max)
        {
            var value = 0.0;
            var isCorrect = false;
            while (!isCorrect)
            {
                var input = GetCorrectStringInput(request);
                if (!double.TryParse(input, out value)) continue;
                if (value >= min && value <= max)
                {
                    isCorrect = true;
                }
            }
            return value;
        }
        #endregion

    }
}
