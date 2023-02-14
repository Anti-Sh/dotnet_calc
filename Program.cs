using System;
using System.Linq;
using System.Text.RegularExpressions;

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

            var splitedInput = inputRow.Split(mathOperations);
            if ( splitedInput.Length <= 1) 
            {
                throw new Exception("Введено только одно число!");
            }

            var numbers = inputRow.Split(mathOperations).Select(x => ValidateDecimal(x.Trim())).ToList(); // Извлечение чисел из строки и преобразование их в Decimal
            decimal result = numbers[0]; // Итеративный процесс начинается с результата, равному первому операнду

            var operations = regMathOperations.Matches(inputRow).Cast<Match>().Select(match => Convert.ToChar(match.Value)).ToList(); // Извлечение операторов из введенной пользователем строки

            int counter = 0;
            do
            {
                counter += 1;
                int indexOfMultiplicate = operations.IndexOf('*') == -1 ? 10000 : operations.IndexOf('*');
                int indexOfDivide = operations.IndexOf('/') == -1 ? 10000 : operations.IndexOf('/');
                int operationIndex = 0;
                if (indexOfMultiplicate < indexOfDivide && indexOfMultiplicate != -1)
                    operationIndex = indexOfMultiplicate;
                else if (indexOfDivide < indexOfMultiplicate && indexOfDivide != -1)
                    operationIndex = indexOfDivide;

                var temp = doCalc(numbers[operationIndex], numbers[operationIndex + 1], operations[operationIndex]);
                Console.WriteLine($"\nШаг {counter}: \t{numbers[operationIndex]} {operations[operationIndex]} {numbers[operationIndex + 1]} = {temp}"); // Вывод результата выполнения итерации
                numbers[operationIndex] = temp;
                operations.RemoveAt(operationIndex);
                numbers.RemoveAt(operationIndex + 1);

            } while(operations.Count > 0);
            
            /*for (int i = 0; i < operations.Count; i++)
            {
                decimal tmp = result; // Хранение первого операнда итерации
                result = doCalc(result, numbers[i + 1], Convert.ToChar(operations[i])); // Выполнение действия
                Console.WriteLine($"\nШаг {i + 1}: \t{tmp} {operations[i]} {numbers[i+1]} = {result}"); // Вывод результата выполнения итерации
            }*/

            Console.WriteLine($"\nРезультат вычисления:\t{numbers[0]}");
        }

        static decimal ValidateDecimal(string input)
        {
            if(!decimal.TryParse(input, out decimal result))
            {
                throw new Exception("Не удалось распознать число!");
            }
            return result;
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
