using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Module2_SelfAssessment
{
    /// This class builds one CPU-Bound method and uses it in three separate
    /// ways -- Task.Run, Parallel.For and Parallel.ForEach
    /// The Parallel methods use advanced features. It's fine if you solve the task with a more straightforward usage.
    public class ParallelMethodManager
    {

        /// <summary>
        /// A method to kick off the mining
        /// Uncomment the method(s) you'd like to run
        /// </summary>
        public void Execute()
        {
            // 1.
            MineWithTask();
            // 2. MineWithParallelForLoop();
            // 3. MineWithParallelForEachLoop();
        }

        /// <summary>
        /// 1. Use Task.Run to kick off the mining operation
        /// </summary>
        public void MineWithTask()
        {
            double coinCount;
            Console.WriteLine($"Primary Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            var listMiningTask = Task.Run(() => MineCoinsWithListMultiplication(5, out coinCount));
            Console.WriteLine($"Working on some other task on thread {Thread.CurrentThread.ManagedThreadId} while the mining code runs.");
            listMiningTask.Wait();
            Console.WriteLine(listMiningTask.Result);
        }

        /// <summary>
        /// 2. Use some of the advanced Parallel.For loop features to run the mining code.
        /// Use an out parameter for the MineCoinsWithListMultiplication method, to total up the coins
        /// </summary>
        public void MineWithParallelForLoop()
        {
            int totalCoins = 0;
            Console.WriteLine($"Start at {DateTime.UtcNow}{Environment.NewLine}");

            var result = Parallel.For<int>(0, 4,
                () => 0, 
                (i, loop, subtotal) => {
                    // the MineCoinsWithListMultiplication has an out parameter. Let's use that to create a running total 
                    double coinCount;
                    // inject some randomness into the method
                    Random rand = new Random();
                    MineCoinsWithListMultiplication(i + rand.Next(1,4), out coinCount);
                    subtotal += (int)coinCount;
                    return subtotal;
                }, 
                (x) => Interlocked.Add(ref totalCoins, x)
                );

            Console.WriteLine($"Finish at {DateTime.UtcNow}{Environment.NewLine}");
            Console.WriteLine($"Total number of coins mined was {totalCoins}");
        }

        /// <summary>
        /// 3. Use advanced Parallel.ForEach loop functions to aggregate the string result 
        /// from each instance of the MineCoinsWithListMultiplication
        /// </summary>
        public void MineWithParallelForEachLoop()
        {
            var resultString = string.Empty;
            var numberArray = new int[] {3, 2, 4, 1, 2};

            var result = Parallel.ForEach<int, string>(numberArray,
                () => string.Empty,
                (j, loop, interimString) => 
                {
                    double coinCount;
                    interimString = MineCoinsWithListMultiplication(j, out coinCount);
                    return interimString;
                },
                (x) => resultString += x
            );

            Console.WriteLine(resultString);
        }

 
        /// <summary>
        /// A CPU-Bound method that creates two lists of random integers, then multiplies each element
        /// </summary>
        public string MineCoinsWithListMultiplication(int multiplier, out double coinCount)
        {
            Console.WriteLine($"Mining on thread: {Thread.CurrentThread.ManagedThreadId} with a multiplier of {multiplier}");
            var result = $"Started mining at {DateTime.UtcNow}{Environment.NewLine}";
            coinCount = 0;
            var listOne = new List<int>();
            var listTwo = new List<int>();
            var rand = new Random();
            for (int i = 0; i < 10000 * multiplier; i++)
            {
                listOne.Add(rand.Next(int.MaxValue));
                listTwo.Add(rand.Next(int.MaxValue));
            }

            foreach (var firstInt in listOne)
            {
                foreach (var secondInt in listTwo)
                {
                    long bigInt = firstInt * secondInt;
                }
                coinCount += .001;
            }
            result += $"Finished mining at {DateTime.UtcNow}{Environment.NewLine}";
            result += $"Mined {(int)coinCount} Async Coins on thread {Thread.CurrentThread.ManagedThreadId}{Environment.NewLine} ";
            return result;
        }


    }
}
