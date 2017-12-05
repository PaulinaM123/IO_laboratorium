using System;
using System.Threading;


namespace Zadanie8
{
    class Zadanie8
    {
        delegate int DelegateType(int x);
        static DelegateType Fib_i;
        static DelegateType Fib_r;
        static DelegateType Silnia_i;
        static DelegateType Silnia_r;

        static void Main(string[] args)
        {
            Fib_i += Fibonacci_iteracyjnie;
            Fib_r += Fibonacci_rekurencyjnie;
            Silnia_i += Silnia_iteracyjnie;
            Silnia_r += Silnia_rekurencyjnie;

            int silnia_value = 6;
            int fibonacci_value = 5;

            AutoResetEvent[] events = new AutoResetEvent[4];

            for (int i = 0; i < 4; i++)
            {
                events[i] = new AutoResetEvent(false);
            }

            var result_1 = Fib_i.BeginInvoke(fibonacci_value, myAsyncCallback, new object[] { Fib_i, "Fibonacci iteracyjnie: ", events[0] });
            var result_2 = Fib_r.BeginInvoke(fibonacci_value, myAsyncCallback, new object[] { Fib_r, "Fibonacci rekurencyjnie: ", events[1] });
            var result_3 = Silnia_i.BeginInvoke(silnia_value, myAsyncCallback, new object[] { Silnia_i, "Silnia iteracyjnie: ", events[2] });
            var result_4 = Silnia_r.BeginInvoke(silnia_value, myAsyncCallback, new object[] { Silnia_r, "Silnia rekurencja: ", events[3] });


            WaitHandle.WaitAll(events);
        }
  

static int Silnia_rekurencyjnie(int x)
{
    if (x == 0)
    {
        return 1;
    }
    else
    {
        return x * Silnia_rekurencyjnie(x - 1);
    }

}

static int Silnia_iteracyjnie(int x)
{
    int result = 1;

    for (int i = 1; i <= x; i++)
    {
        result *= i;
    }
    return result;
}


static int Fibonacci_rekurencyjnie(int x)
{

    if (x == 0) return 0;
    if (x == 1) return 1;
    return Fibonacci_rekurencyjnie(x - 1) + Fibonacci_rekurencyjnie(x - 2);
}

static int Fibonacci_iteracyjnie(int x)
{
    if (x <= 1)
    {
        return x;
    }
    int fib = 1;
    int prevFib = 1;

    for (int i = 2; i < x; i++)
    {
        int temp = fib;
        fib += prevFib;
        prevFib = temp;
    }

    return fib;
}


static void myAsyncCallback(IAsyncResult stateInfo)
{
    DelegateType delegat = ((object[])(stateInfo.AsyncState))[0] as DelegateType;
    string s = ((object[])(stateInfo.AsyncState))[1] as string;
    AutoResetEvent event_1 = ((object[])(stateInfo.AsyncState))[2] as AutoResetEvent;

    Console.WriteLine(s + delegat.EndInvoke(stateInfo).ToString());
    event_1.Set();
}


    }
}

/****************WNIOSKI****************
Wybrany tryb APM: Callback
Kolejność wykonywania metod jest różna.
 */
