using MovieTheaterRating.Data;
using MovieTheaterRating.Entity;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MovieTheaterRating.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Init database...");
            MovieTheaterRatingContext context = new MovieTheaterRatingContext();

            //***** UNCOMMENT THESE OUT TO POPULATE DATABASE *********//
            context.Database.Initialize(true);
            if (context != null)
                context.Dispose();
            //AddManyRecords(); //UNCOMMENT THIS OUT IF WANNA TRY WITH BULK INSERT
            Console.WriteLine("Done...");


            //Console.WriteLine("==== Calling TestProcessTAskAsync() first ====");
            //TestProcessTAskAsync();
            //Console.WriteLine("press any key to exit");

            //Console.WriteLine("==== Calling LongRunningMethod() second ====");
            //TestAsyncAwaitMethods();
            //Console.WriteLine("press any key to exit");

            //Console.WriteLine("==== Calling TestLoop() third ====");
            //TestLoop();
            //Console.WriteLine("press any key to exit");

            //Console.WriteLine("==== Calling AsyncAwaitDemo class fourth ====");
            //var demo = new AsyncAwaitDemo();
            //demo.Test();

            //Console.WriteLine("==== Calling AsyncAwaitDemo2 class fourth ====");
            //var demo2 = new AsyncAwaitDemo2();
            //demo2.TestLoop();

            Console.ReadLine();
        }

        //=== 1st example with async await ===//
        private async static void TestProcessTAskAsync()
        {

            int ret = await ProcessTaskAsync();
            Console.WriteLine("Example1: Ret = " + ret);
        }

        private static async Task<int> DelayAndReturnAsync(int val)
        {
            await Task.Delay(TimeSpan.FromSeconds(val)); //delay for 3 seconds then return the value
            return val;
        }

        private static async Task<int> ProcessTaskAsync() //process the Async Task above
        {
            Console.WriteLine("Example1: start long running method...");
            var taskA = await DelayAndReturnAsync(3);
            int num = taskA + 1;
            Console.WriteLine("Example1: end long running method...");
            return num;
        }

        //=== 2nd example with async await ===//
        private async static void TestAsyncAwaitMethods()
        {
            int ret = await LongRunningMethod();
            Console.WriteLine("Example2: Ret = " + ret);
        }

        public async static Task<int> LongRunningMethod()
        {
            Console.WriteLine("Example2: Start long running method...");
            await Task.Delay(5000);
            Console.WriteLine("Example2: End Long Running method...");
            return 1;
        }

        //=== 3rd example with async await ===//
        private static async void TestLoop()
        {
            for (var i = 0; i <= 10; i++)
            {
                await TestAsync(i);
            }
        }

        private static Task TestAsync(int i)
        {
            return Task.Run(() => TaskToDo(i));
        }

        private static async void TaskToDo(int i)
        {
            await Task.Delay(1000);
            Console.WriteLine(i);
        }

        private static void AddTestRecords(List<Movie> reports)
        {
            for (int i = 0; i <= 560000; i++)
            {
                Movie movie = new Movie()
                {
                    Id = i,
                    Title = "title" + i

                };
                reports.Add(movie);
            }
        }

        static void AddManyRecords()
        {
            List<Movie> reports = new List<Movie>();
            Stopwatch sw = Stopwatch.StartNew();
            var scopeOptions = new TransactionOptions();
            scopeOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            scopeOptions.Timeout = TimeSpan.MaxValue;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, scopeOptions))
            {
                MovieTheaterRatingContext db = null;
                try
                {
                    db = new MovieTheaterRatingContext();
                    db.Configuration.AutoDetectChangesEnabled = false;
                    int count = 0;
                    AddTestRecords(reports);
                    foreach (var rep in reports)
                    {
                        ++count;
                        db = AddToContext(db, rep, count, 100000, true);
                    }
                    db.SaveChanges();

                }
                finally
                {
                    if (db != null)
                        db.Dispose();
                }
                scope.Complete();
            }
            sw.Stop();
            Console.WriteLine("{0} milliseconds", sw.ElapsedMilliseconds);
        }

        private static MovieTheaterRatingContext AddToContext(MovieTheaterRatingContext db, Movie entity, int count, int commitCount, bool recreatedContext)
        {
            db.Set<Movie>().Add(entity);

            if (count % commitCount == 0)
            {
                db.SaveChanges();
                if (recreatedContext)
                {
                    db.Dispose();
                    db = new MovieTheaterRatingContext();
                    db.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return db;
        }
    }

    public class AsyncAwaitDemo
    {
        public async void Test()
        {
            await DoStuff();
        }

        private Task DoStuff()
        {
            return Task.Run(() => { LongRunningOperation(); });
        }

        private async void LongRunningOperation()
        {
            for (var counter = 0; counter <= 10; counter++)
            {
                await Task.Delay(1000);
                Console.WriteLine("COUNTER DEMO1 = " + counter);
            }
        }
    }

    public class AsyncAwaitDemo2
    {
        public async void TestLoop()
        {
            for (var i = 0; i <= 10; i++)
            {
                await DoStuff(i);
            }
        }

        private  Task DoStuff(int i)
        {
            return Task.Run(() => LongRunningOperation(i));
        }

        private  async void LongRunningOperation(int i)
        {
            await Task.Delay(1000);
            Console.WriteLine("COUNTER DEMO2 = " + i);
        }
    }
}
