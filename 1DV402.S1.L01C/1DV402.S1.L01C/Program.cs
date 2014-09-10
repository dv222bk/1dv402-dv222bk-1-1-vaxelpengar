using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1DV402.S1.L01C
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = Properties.Strings.Console_Title;
            do
            {
                double total = ReadPositiveDouble(Properties.Strings.Total_Prompt);
                uint toPay = (uint)Math.Round(total, MidpointRounding.AwayFromZero);
                double roundingOffAmount = toPay - total;
                uint cash = ReadUint(Properties.Strings.Recieved_Prompt, toPay);
                uint change = cash - toPay;
                uint[,] denominations = new uint[7, 2] { { 500, 0 }, { 100, 0 }, { 50, 0 }, { 20, 0 }, { 10, 0 }, { 5, 0 }, { 1, 0 } };
                denominations = SplitIntoDenominations(change, denominations);

                ViewReceipt(total, roundingOffAmount, toPay, cash, change, denominations);

                ViewMessage(Properties.Strings.Continue_Prompt);

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape); // http://msdn.microsoft.com/en-us/library/x3h8xffw%28v=vs.110%29.aspx
        }

        private static double ReadPositiveDouble(string prompt)
        {
            double value;
            string answer;
            while (true)
            {
                Console.Write(prompt);
                answer = Console.ReadLine();
                try
                {
                    value = double.Parse(answer);
                    if (Math.Round(value, MidpointRounding.AwayFromZero) <= 0)
                    {
                        throw new Exception();
                    }
                    return value;
                }
                catch (FormatException)
                {
                    ViewMessage(String.Format(Properties.Strings.Invalid_Value_Error, answer), true);
                }
                catch
                {
                    ViewMessage(String.Format(Properties.Strings.Low_Value_Error, answer), true);
                }
            }
        }

        private static uint ReadUint(string prompt, uint minValue)
        {
            uint value;
            string answer;
            while (true)
            {
                Console.Write(prompt);
                answer = Console.ReadLine();
                try
                {
                    value = uint.Parse(answer);
                    if (value < minValue)
                    {
                        throw new Exception();
                    }
                    return value;
                }
                catch (FormatException)
                {
                    ViewMessage(String.Format(Properties.Strings.Invalid_Value_Error, answer), true);
                }
                catch
                {
                    ViewMessage(String.Format(Properties.Strings.Low_Value_Error, answer), true);
                }
            }
        }

        private static uint[,] SplitIntoDenominations(uint change, uint[,] denominations)
        {
            for (int i = 0; i < denominations.GetLength(0); i++)
            {
                if (change / denominations[i, 0] >= 1)
                {
                    denominations[i, 1] = change / denominations[i, 0];
                    change %= denominations[i, 0];
                }
            }
            return denominations;
        }

        private static void ViewMessage(string message, bool isError = false)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            if (!isError)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(message);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void ViewReceipt(double subtotal, double roundingOffAmount,
            uint total, uint cash, uint change, uint[,] denominations)
        {
            Console.WriteLine();
            Console.WriteLine(Properties.Strings.Reciept_String);
            Console.WriteLine(Properties.Strings.Reciept_Line);
            Console.WriteLine(String.Format(Properties.Strings.Reciept_SubTotal, subtotal));
            Console.WriteLine(String.Format(Properties.Strings.Reciept_RoundingOffAmount, roundingOffAmount));
            Console.WriteLine(String.Format(Properties.Strings.Reciept_Total, total));
            Console.WriteLine(String.Format(Properties.Strings.Reciept_Cash, cash));
            Console.WriteLine(String.Format(Properties.Strings.Reciept_Change, change));
            Console.WriteLine(Properties.Strings.Reciept_Line);
            Console.WriteLine();

            string noteType = Properties.Strings.Notetype_Bills;

            for (int i = 0; i < denominations.GetLength(0); i++)
            {
                if (denominations[i, 0] < 20)
                {
                    noteType = Properties.Strings.Notetype_Coins;
                }
                if (denominations[i, 1] > 0)
                {
                    Console.WriteLine(" {0, 3}-{1, -12}: {2}", denominations[i, 0], noteType, denominations[i, 1]);
                }
            }
        }
    }
}
