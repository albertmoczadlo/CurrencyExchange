namespace JediApp.Services.Helpers
{
    public static class MenuOptionsHelper
    {
        public static int GetUserSelectionAndValidate(int allowedOptionMin, int allowedOptionMax)
        {
            while (true)
            {
                var selectedOptionText = Console.ReadLine();
                if (!int.TryParse(selectedOptionText, out int selectedOptionNumber) || selectedOptionNumber < allowedOptionMin || selectedOptionNumber > allowedOptionMax)
                {
                    Console.WriteLine($"Selected option '{selectedOptionText}' is not valid. Choose correct option: ");
                }
                else
                {
                    return selectedOptionNumber;
                }
            }
        }

        public static bool GetBackToMainMenuQuestion()
        {
            while (true)
            {
                Console.WriteLine("Go back to menu? Y/N");

                var selectedOption = Console.ReadLine();
                if (selectedOption.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else if (selectedOption.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Wrong option");
                }    
            }
        }

        public static string CheckString(string input)
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("No input! Please type something... ");
                    input = Console.ReadLine();
                }
                else
                {
                    return input;
                }
            }

        }

        public static decimal CheckDecimal(string input)
        {
            decimal output;
            string tryInput = input;
            while (true)
            {

                if (decimal.TryParse(tryInput, out output))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong data! Please insert a decimal !");
                    tryInput = Console.ReadLine();
                }
            }

            return output;
        }
    }
}
