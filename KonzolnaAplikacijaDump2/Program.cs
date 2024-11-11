using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolnaAplikacijaDump2
{
    internal class Program
    {
        static Dictionary<int, string> users = new Dictionary<int, string>;
        static void AddUser()
        {

        }

        static void RemoveUser()
        {

        }

        static void EditUser()
        {

        }

        static void ViewUsers()
        {

        }
        static void UsersMainFunction()
        {
            
            Console.WriteLine("Odaberite opciju za rad s korisnicima: ");
            Console.WriteLine("1 - Unos novog korisnika");
            Console.WriteLine("2 - Brisanje korisnika");
            Console.WriteLine("3 - Uređivanje korisnika");
            Console.WriteLine("4 - Pregled korisnika");

            var choice = -1;
            var success = false;

            do
            {
                Console.Write("Unesite svoj odabir: ");
                success = int.TryParse(Console.ReadLine(), out choice);
            } while (!success);
            


        }
        static void Main(string[] args)
        {
            //Glavni dict sa korisnicima
            //korisnik ima id-key -> value (imePrezime, dictionary)

            Console.WriteLine("1 - Korisnici");
            Console.WriteLine("2 - Računi");
            Console.WriteLine("3 - Izlaz iz aplikacije");


            var choice = "0";
            do
            {
                Console.Write("Unesite izbor: ");
                choice = Console.ReadLine();

                if (choice == "1" || choice == "2" || choice == "3")
                    break;
                else
                {

                }
            } while (true);

            switch (choice)
            {
                case ("1"):
                    UsersMainFunction();
                    break;
            }
        }
    }
}
