using static U4_W1_D5.Taxpayer;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace U4_W1_D5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string name = AskForValidNameOrSurnameOrCity("Inserisci il nome:", "Nome non valido. Assicurati che ogni parola inizi con una lettera maiuscola seguita solo da lettere minuscole.");
            string surname = AskForValidNameOrSurnameOrCity("Inserisci il cognome:", "Cognome non valido. Assicurati che ogni parola inizi con una lettera maiuscola seguita solo da lettere minuscole.");
            string cityOfBirth = AskForValidNameOrSurnameOrCity("Inserisci la città di nascita:", "Città non valida. Assicurati che ogni parola inizi con una lettera maiuscola seguita solo da lettere minuscole.");
            Taxpayer.Gender gender = AskForGender();
            DateOnly dateOfBirth = DateOfBirthControl();          
            string taxIdCode = AskForValidTaxIdCode("Inserisci il codice fiscale:");
            double annualGrossSalary = AskForValidDouble("Inserisci il salario annuo lordo:");

            Taxpayer taxpayer = new Taxpayer(name, surname, cityOfBirth, gender, dateOfBirth, taxIdCode, annualGrossSalary);
            double tax = taxpayer.CalculateTax();

            Console.WriteLine("==================================================");
            Console.WriteLine($"Nome: {taxpayer.Name}");
            Console.WriteLine($"Cognome: {taxpayer.Surname}");
            Console.WriteLine($"Città di nascita: {taxpayer.CityOfBirth}");
            Console.WriteLine($"Data di nascita: {taxpayer.DateOfBirth}");
            Console.WriteLine($"Genere: {taxpayer.Type}");
            Console.WriteLine($"Codice fiscale: {taxpayer.TaxIdCode}");
            Console.WriteLine($"Salario annuo lordo: {taxpayer.AnnualGrossSalary}");
            Console.WriteLine($"Tasse: {tax}");
        }

        // utilizzo private per nascondere il metodo all'esterno della classe Program
        // utilizzo static per non dover creare un'istanza di Program per chiamare il metodo

        // controllo se l'input inserito è valido
        private static string AskForValidInput(string prompt, Func<string, bool> validator, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (validator(input))
                {
                    Console.WriteLine("Input valido.");
                    return input;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }

        // controllo per il nome, cognome e città di nascita
        private static string AskForValidNameOrSurnameOrCity(string prompt, string errorMessage)
        {
            return AskForValidInput(prompt, input => Regex.IsMatch(input, @"^([A-Z][a-zàèéìòùÀÈÉÌÒÙäöüÄÖÜâêîôûÂÊÎÔÛ']+)(\s[A-Z][a-zàèéìòùÀÈÉÌÒÙäöüÄÖÜâêîôûÂÊÎÔÛ']+)*$"), errorMessage);
        }

        // controllo per la data di nascita
        private static DateOnly DateOfBirthControl()
        {
            return DateOnly.ParseExact(AskForValidInput("Inserisci la data di nascita (yyyy-mm-dd):", input =>
            {
                // Prova a convertire l'input in una data
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    // Controlla se la data è nel futuro
                    if (parsedDate > DateTime.Now)
                    {
                        Console.WriteLine("La data di nascita non può essere nel futuro.");
                        return false;
                    }
                    return true; // La data è valida e non nel futuro
                }
                return false; // La conversione della data non è riuscita
            }, "Data non valida, per favore inserisci la data nel formato corretto (yyyy-mm-dd) e assicurati che il giorno e il mese siano validi."), "yyyy-MM-dd");
        }

        // controllo per il genere
        private static Taxpayer.Gender AskForGender()
        {
            return (Taxpayer.Gender)Enum.Parse(typeof(Taxpayer.Gender), AskForValidInput("Scelta del genere: 1 per Maschio, 2 per Femmina", input => input == "1" || input == "2", "Scelta non valida, per favore seleziona 1 per Maschio o 2 per Femmina"));
        }

        // controllo per l'ultima lettera del codice fiscale
        private static bool IsValidTaxIdCode(string taxIdCode)
        {
            if (!Regex.IsMatch(taxIdCode, @"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$"))
            {
                return false;
            }

            // Calcolo del carattere di controllo
            int s = 0;
            string evenChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int[] oddCharsValues = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
            for (int i = 0; i < 15; i++)
            {
                char c = taxIdCode[i];
                int charValue = (i % 2 == 0) ? oddCharsValues[c - '0'] : evenChars.IndexOf(c);
                s += charValue;
            }
            int checkCharValue = s % 26;
            char expectedCheckChar = (char)('A' + checkCharValue);

            return taxIdCode[15] == expectedCheckChar;
        }

        // controllo per il codice fiscale
        private static string AskForValidTaxIdCode(string prompt)
        {
            return AskForValidInput(prompt, IsValidTaxIdCode, "Codice fiscale non valido. Assicurati di inserire un codice fiscale nel formato corretto e che l'ultima lettera sia congruente.");
        }

        // controllo per il RAL
        private static double AskForValidDouble(string prompt)
        {
            return double.Parse(AskForValidInput(prompt, input => double.TryParse(input, out double value) && value >= 0, "Valore non valido. Assicurati di inserire un numero non negativo."));
        }
    }
}
