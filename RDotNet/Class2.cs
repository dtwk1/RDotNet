using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet
{
    class Class2
    {
        /// <summary>
        /// https://github.com/gooldan/rdotnet/blob/master/statisticsconsoleapplication/statisticsconsoleapplication/Program.cs
        /// </summary>
        /// <param name="args"></param>
        static void Method()
        {
            REngine engine = REngine.GetInstance();
            // REngine requires explicit initialization.
            engine.Initialize();
            string exit = "n";
            while (!exit.Equals("y"))
            {
                Console.Write("Введите значения: ");
                string input = Console.ReadLine();
                if (input.Length == 0)
                {
                    double[] a = { 2, 5, 7, 3.4, 7 };
                    // создание объекта R.NET из объекта c#
                    NumericVector group1 = engine.CreateNumericVector(a);
                    // именование созданного объекта в R
                    engine.SetSymbol("a", group1);

                    Console.WriteLine("Initial string: " + string.Join("; ", a));
                }
                else
                {
                    engine.Evaluate("a <- c(" + input + ")");
                }
                // представление результата в виде числового массива 
                // массив -- всегда, даже если результат -- одно число
                // результат сохраняется в объект и выводится в консоль, ЕСЛИ он выводится в консоль в R
                NumericVector a1 = engine.Evaluate("mean(a)").AsNumeric();

                // представление результата в виде символьного массива
                // сохранение в объект и вывод в консоль
                CharacterVector a2T = engine.Evaluate("mean(a, na.rm = T)").AsCharacter();

                // сохранение в объект c# и сохранение в объект в R, нет вывода в консоль
                NumericVector a3 = engine.Evaluate("s <- mean(a) + 2").AsNumeric();

                // результат -- вектор, поэтому доступ по индексу a2T[0]
                Console.WriteLine("Mean: " + a2T[0]);

                Console.Write("Выйти? (y/n)");
                exit = Console.ReadLine();
            }
            
        }
    }
}
