using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U4_W1_D5
{
    internal class Taxpayer
    {
        // definisco il genere
        public enum Gender { Male, Female }

        // proprietà
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CityOfBirth { get; set; }
        public Gender Type { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string TaxIdCode { get; set; }
        public double AnnualGrossSalary { get; set; }


        // costruttore
        public Taxpayer(string name, string surname, string cityOfBirth, Gender type, DateOnly dateOfBirth, string taxIdCode, double annualGrossSalary)
        {
            Name = name;
            Surname = surname;
            CityOfBirth = cityOfBirth;
            Type = type;
            DateOfBirth = dateOfBirth;
            TaxIdCode = taxIdCode;
            AnnualGrossSalary = annualGrossSalary;
        }

        // calcolo delle tasse
    public double CalculateTax()
        {
            double tax = 0;
            if (AnnualGrossSalary < 0)
            {
                throw new ArgumentException("Annual gross salary cannot be negative");
            }
            else if (AnnualGrossSalary >= 0 && AnnualGrossSalary <= 15000)
            {
                tax = AnnualGrossSalary * 0.23;
            }
            else if (AnnualGrossSalary > 15000 && AnnualGrossSalary <= 28000)
            {
                tax = 3450 + ((AnnualGrossSalary - 15000)  * 0.27);
            }
            else if (AnnualGrossSalary > 28000 && AnnualGrossSalary <= 55000)
            {
                tax = 6960 + ((AnnualGrossSalary - 28000) * 0.38);
            }
            else if (AnnualGrossSalary > 55000 && AnnualGrossSalary <= 75000)
            {
                tax = 17220 + ((AnnualGrossSalary - 55000) * 0.41);
            }
            else
            {
                tax = 25420 + ((AnnualGrossSalary - 75000) * 0.43);
            }
            return tax;
        }
    }
}
