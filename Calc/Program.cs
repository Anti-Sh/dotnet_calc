using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calc
{
    internal class Program
    {
        static char[] mathOperations = new char[] { '+', '-', '/', '*' };
        static Regex regMathOperations = new Regex(@"[+/*-]{1}");

        static void Main(string[] args)
        {
            Console.WriteLine("Калькулятор поддерживает ввод как целых чисел, так и чисел с плавающей точкой. \n" +
                "Число с плавающей точкой вводится в формате \"3,636\"\n" +
                "Количество операндов не ограничено, вычисления производятся в порядке записи (приоритет операторов равный)");

            do
            {
                Calculation();
            }
            while (ContinueCalc());

        }

        static void Calculation() // Основной метод, отвечающий за чтение/выполнение расчета/вывод
        {
            Console.WriteLine("\n\nВведите выражение для расчета (Например, A+B или A + B) ");
            string inputRow = Console.ReadLine().Replace('.', ','); // Считывание примера из консоли и исправление неправильного ввода числа с плавающей точкой

            decimal[] numbers = inputRow.Split(mathOperations).Select(x => decimal.Parse(x.Trim())).ToArray(); // Извлечение чисел из строки и преобразование их в Decimal
            decimal result = numbers[0]; // Итеративный процесс начинается с результата, равному первому операнду

            var operations = regMathOperations.Matches(inputRow); // Извлечение операторов из введенной пользователем строки
            for (int i = 0; i < operations.Count; i++)
            {
                decimal tmp = result; // Хранение первого операнда итерации
                result = doCalc(result, numbers[i + 1], Convert.ToChar(operations[i].Value)); // Выполнение действия
                Console.WriteLine($"\nШаг {i + 1}: \t{tmp} {operations[i].Value} {numbers[i+1]} = {result}"); // Вывод результата выполнения итерации
            }

            Console.WriteLine($"\nРезультат вычисления:\t{result}");
        }

        static decimal doCalc(decimal a, decimal b, char op) // Выполнение действия
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '/':
                    return a / b;
                case '*':
                    return a * b;
                default:
                    return a;
            }
        }

        static bool ContinueCalc() // Метод, определяющий, нужно ли продолжать выполнение программы
        {
            Console.WriteLine("\nВы хотите продолжить? (Y или N): ");
            return Console.ReadLine().ToUpper() == "Y";
        }

    }
}
