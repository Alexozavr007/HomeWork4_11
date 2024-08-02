using System;
using System.Text;
using System.Threading;

// Обмеження: Не можна використовувати об'єкти блокування структурного типу
// Працюючи з об'єктом класу - Monitor.

// Lock - не працює коректно з об'єктами блокування структурних типів,
// Допускається використання об'єктів блокування тільки типів посилань.

namespace monitor {
    class Program {

        static int counter = 0;

        static object block = 0; // block - не повинен бути структурним.

        static void Function() {
            Console.WriteLine($"Поток стартує {Thread.CurrentThread.GetHashCode()}");
            for (int i = 0; i < 50; ++i) {
                // Встановлюється блокування кожен (50!) разів у новий object (boxing).
                Monitor.Enter((object)block); // boxing створює новий об'єкт (50 об'єктів).

                // Виконання деякої роботи потоком ...
                Console.WriteLine(++counter);

                // Спроба зняти блокування з об'єкта, який не є об'єктом блокування.
                Monitor.Exit((object)block); // boxing створює абсолютно новий об'єкт.
            }
            Console.WriteLine($"Поток закінчився {Thread.CurrentThread.GetHashCode()}");

        }

        static void Main() {
            Console.OutputEncoding = Encoding.Unicode;
            Thread[] threads = { new Thread(Function), new Thread(Function), new Thread(Function) };

            foreach (Thread thread in threads) {
                thread.Start();
                thread.Join();
            }
            // Delay
            Console.ReadKey();
        }
    }
}