using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1DV402.S1.L01C
{
    class Program
    {
        /// <summary>
        /// The program core.
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        private static void Main(string[] args)
        {
            Console.Title = Properties.Strings.Console_Title;
            do
            {
                double total = ReadPositiveDouble(Properties.Strings.Total_Prompt);
                uint toPay = (uint)Math.Round(total, MidpointRounding.AwayFromZero); // MidPointRounding.AwayFromZero Example: 3,5 becomes 4, 3,49 becomes 3.
                double roundingOffAmount = toPay - total;
                uint cash = ReadUint(Properties.Strings.Recieved_Prompt, toPay);
                uint change = cash - toPay;
                uint[,] denominations = new uint[7, 2] { { 500, 0 }, { 100, 0 }, { 50, 0 }, { 20, 0 }, { 10, 0 }, { 5, 0 }, { 1, 0 } }; // [Denomination, number of that denomination]
                denominations = SplitIntoDenominations(change, denominations);

                ViewReceipt(total, roundingOffAmount, toPay, cash, change, denominations);

                ViewMessage(Properties.Strings.Continue_Prompt);

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape); // http://msdn.microsoft.com/en-us/library/x3h8xffw%28v=vs.110%29.aspx
        }

        /// <summary>
        /// Reads a positive double from the user. 
        /// Needs a prompt input to send to the user. 
        /// Will continue to ask the user for a value until the user gives a positive double.
        /// </summary>
        /// <param name="prompt">Prompt sent to user, asking for a positive double</param>
        /// <returns>Returns the positive double the user inputs</returns>
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

        /// <summary>
        /// Reads a uint higher than minValue from the user. 
        /// Needs a prompt input to send to the user and a minimum value. 
        /// Will continue to ask the user for a value until the user gives a valid uint.
        /// </summary>
        /// <param name="prompt">Prompt sent to user, asking for a uint</param>
        /// <param name="minValue">The minimum value the user can put in</param>
        /// <returns>Returns the uint the user inputs</returns>
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

        /// <summary>
        /// Splits an uint into denominations.
        /// The multidimensional array needs to contain the denominations that should be used
        /// </summary>
        /// <param name="change">uint, the changethat should be returned to the customer</param>
        /// <param name="denominations">uint multidimensional array. [denomination, number of that denomination]</param>
        /// <returns>Returns a uint multidimensional array with denominations and the amount of times that denomination should be returned to the customer [denomination, number of that denomination]</returns>
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


        /// <summary>
        /// Shows a message to the user with a colored background
        /// </summary>
        /// <param name="message">The message that should be shown to the user</param>
        /// <param name="isError">Is this an error message? true = red background, false = darkgreen background</param>
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

        /// <summary>
        /// Prints out a reciept to the user
        /// </summary>
        /// <param name="subtotal">The total cost</param>
        /// <param name="roundingOffAmount">Change in subtotal when the decimals is rounded to closest zero</param>
        /// <param name="total">Total amount that should be paid after rounding of subtotal to closest zero</param>
        /// <param name="cash">The amount paid by the customer</param>
        /// <param name="change">The amount the customer should get back</param>
        /// <param name="denominations">The amount of different denominations the user should get back</param>
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
                if (denominations[i, 0] < 20) // if the currency isn't a paperbill
                {
                    noteType = Properties.Strings.Notetype_Coins;
                }
                if (denominations[i, 1] > 0) // if the denomination is used
                {
                    Console.WriteLine(" {0, 3}-{1, -12}: {2}", denominations[i, 0], noteType, denominations[i, 1]);
                }
            }
        }
    }
}
