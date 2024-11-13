
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonzolnaAplikacijaDump2
{
    internal class Program
    {
        static Dictionary<int, Tuple<string, string, DateTime, Dictionary<string, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>>>>> users = new Dictionary<int, Tuple<string, string, DateTime, Dictionary<string, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>>>>>();
        static void DisplayAccountsTest(int userId)
        {
            if(users.TryGetValue(userId, out var user))
            {
                var firstName = user.Item1;
                var lastName = user.Item2;
                var dob = user.Item3;
                var accounts = user.Item4;

                Console.WriteLine($"Account balances for {firstName} {lastName}, DOB: {dob.ToString("yyyy-MM-dd")}");

                foreach (var account in accounts)
                {
                    string accountName = account.Key;
                    double balance = account.Value.Item1; // Balance is the first item in the tuple
                    Console.WriteLine($"  - {accountName} balance: {balance:C}");
                }
            }

            else
            {
                Console.WriteLine("User not found.");
            }
        }
        static Dictionary<string, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>>> CreateAccountDictionary()
        {
            Dictionary<string, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>>> accounts = new Dictionary<string, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>>>();

            var tekuciTransactions = new Dictionary<int, Tuple<double, string, string, string, DateTime>>();
            

            var ziroTransactions = new Dictionary<int, Tuple<double, string, string, string, DateTime>>();
            

            var prepaidTransactions = new Dictionary<int, Tuple<double, string, string, string, DateTime>>();
            

            accounts.Add("tekuci", Tuple.Create(100.00, tekuciTransactions));
            accounts.Add("ziro", Tuple.Create(0.00, ziroTransactions));
            accounts.Add("prepaid", Tuple.Create(0.00, prepaidTransactions));
            return accounts;
        }

        static void AddUser()
        {
            var nextId = users.Count + 1;
            var firstName = "";
            var surName = "";
            var dob = DateTime.MinValue;

            //Unos imena
            do
            {
                Console.Write("Unesite ime");
                firstName = Console.ReadLine();
            } while (firstName.Length < 1);

            //Surname input
            do
            {
                Console.Write("Unesite prezime");
                surName = Console.ReadLine();
            } while (surName.Length < 1);

            //DOB input
            var validDate = false;
            while (!validDate)
            {
                Console.WriteLine("Unesite datum rodenja (YYYY-MM-DD): ");
                var dateInput = Console.ReadLine();

                validDate = DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);

                if (!validDate)
                {
                    Console.WriteLine("Krivo unesen datum probajte opet: ");
                }
            }

            Console.WriteLine($"ime: {firstName}, prezime: {surName}, dob: {dob.ToString("yyyy-MM-dd")}");
            var userAccountsInit = CreateAccountDictionary();
            users.Add(nextId, Tuple.Create(firstName, surName, dob, userAccountsInit));

            DisplayAccountsTest(users.Count);

        }

        static void RemoveUserById()
        {
            var chosenID = -1;
            var parseSuccess = false;

            while (!parseSuccess)
            {
                Console.Write("Unesite id kojeg zelite izbrisati: ");
                parseSuccess = int.TryParse(Console.ReadLine(), out chosenID);
            }

            if (users.TryGetValue(chosenID, out var user))
            {
                users.Remove(chosenID);
            }
            else
            {
                Console.WriteLine($"Korisnik id {chosenID} ne postoji");
            }

            Console.WriteLine(users.Count);
        }

        static void RemoveUserByName()
        {
            var chosenFirstName = "";
            var chosenSurName = "";
            var idRemove = -1;
            var removeSuccess = false;
            do
            {
                Console.Write("Unesite ime: ");
                chosenFirstName = Console.ReadLine();
                Console.Write("Unesite prezime: ");
                chosenSurName = Console.ReadLine();

                foreach (var el in users)
                {
                    if(el.Value.Item2 == chosenSurName)
                    {
                        if (el.Value.Item1 == chosenFirstName)
                        {
                            idRemove = el.Key;
                            removeSuccess = true;
                        }
                    }
                }

                if (!removeSuccess)
                {
                    Console.WriteLine("Ne postoji korisnik tog imena, probajte opet");
                }

            } while (!removeSuccess);

            if (idRemove != -1)
            {
                Console.WriteLine($"Brisanje korisnika {chosenFirstName} {chosenSurName}");
                users.Remove(idRemove);
            }

            Console.WriteLine("Preostali korisnici:");
            foreach (var kvp in users)
            {
                Console.WriteLine($"ID: {kvp.Key}, Name: {kvp.Value.Item1} {kvp.Value.Item2}, DOB: {kvp.Value.Item3}");
            }
        }

        static void RemoveUser()
        {
            var choice = "";

            while (true)
            {
                Console.WriteLine("Odaberite opciju za brisanje: ");
                Console.WriteLine("a) po id-u");
                Console.WriteLine("b) po imenu i prezimenu");

                Console.Write("Unesite izbor");
                choice = Console.ReadLine();

                if(choice == "a" || choice == "b")
                {
                    Console.WriteLine($"Odabrana opcija {choice}");
                    break;
                }

            }

            switch (choice)
            {
                case "a":
                    Console.WriteLine("Odabran izbor: brisanje korisnika po id-u");
                    RemoveUserById();
                    break;
                case "b":
                    RemoveUserByName();
                    break;
                default:
                    Console.WriteLine("Greška, povratak na glavi izbornik");
                    break;
            }
        }

        static void EditUser()
        {
            var foundUser = false;
            var userID = -1;
            do
            {
                Console.WriteLine("Unesite id korisnika sta zelite uredivat: ");
                var chosenID = Console.ReadLine();

                if(int.TryParse(chosenID, out userID))
                {
                    foundUser = users.TryGetValue(userID, out var user);
                    if (foundUser)
                    {
                        Console.WriteLine("Pronaden korisnik, sigurno želite uređivati korisnika? (d/n)");
                        var editChoice = Console.ReadLine();

                        if (editChoice.ToLower() == "d")
                        {
                            Console.WriteLine("Zelite uredivati prvo ime (f) ili prezime(l) (f/l)");
                            var nameChoice = Console.ReadLine();

                            if (nameChoice.ToLower() == "f")
                            {
                                Console.Write("Unesi novo ime: ");
                                var newFirstName = Console.ReadLine();
                                users[userID] = Tuple.Create(newFirstName, user.Item2, user.Item3, user.Item4);
                            }

                            if (nameChoice.ToLower() == "l")
                            {
                                Console.Write("Unesi novo prezime: ");
                                var newLastName = Console.ReadLine();
                                users[userID] = Tuple.Create(user.Item1, newLastName, user.Item3, user.Item4);
                            }

     

                            Console.WriteLine($"Novi podaci: {users[userID].Item1} {users[userID].Item2}");
                        }
                        else
                            Console.WriteLine("Izlazak iz uredivanja, povratak na pocetak");
                    }
                }
            } while (!foundUser);
        }

        static void PrintSurnameSorted()
        {

            foreach (KeyValuePair<int, Tuple<string, string, DateTime, Dictionary<string, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>>>>> user in users.OrderBy(key => key.Value.Item2))
            {
                Console.WriteLine($"{user.Key} - {user.Value.Item1} - {user.Value.Item2} - {user.Value.Item3}");
            }

        }

        static void Print30Older()
        {
            Console.WriteLine("Odabrali ste ispis korisnika koji imaju više od 30 godina");
            foreach (var user in users)
            {
                if(DateTime.Today.Year - user.Value.Item3.Year >= 30)
                    Console.WriteLine($"{user.Key} - {user.Value.Item1} - {user.Value.Item2} - {user.Value.Item3.Date.ToString("yyyy-MM-dd")}");
            }
        }

        static void ViewUsers()
        {
            Console.WriteLine("Odabrali ste opciju za pregled korisnika");
            var viewing = true;
            while (viewing)
            {
                Console.WriteLine("a) Ispis po prezimenu");
                Console.WriteLine("b) Ispis svih onih koji imaju više od 30 godina");
                Console.WriteLine("c) Ispis svih onih koji imaju barem jedan račun u minusu");

                var input = Console.ReadLine();
                if (input.ToLower() == "a" || input.ToLower() == "b" || input.ToLower() == "c")
                {
                    switch (input)
                    {
                        case "a":
                            PrintSurnameSorted();
                            break;
                        case "b":
                            Print30Older();
                            break;
                        case "c":
                            //PrintNegativeBalance();
                            break;
                    }

                    viewing = false;
                }
            }
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

           
            switch (choice)
            {
                case 1:
                    AddUser();
                    break;
                case 2:
                    RemoveUser();
                    break;
                case 3:
                    EditUser();
                    break;
                case 4:
                    ViewUsers();
                    break;
                default:
                    Console.WriteLine("Dogodila se greska, povratak na glavni izbornik");
                    break;
            }

        }

        static void AccountsMainFunction()
        {
            var foundUser = false;

            //uzmi logiku iz brisanje po imenu i prezimenu
            
     

        }


        static void Main(string[] args)
        {
            //Glavni dict sa korisnicima
            //korisnik ima id-key -> value (imePrezime, dictionary)

            var uvodicDict = CreateAccountDictionary();
            var oreillyDict = CreateAccountDictionary();

            users.Add(1, Tuple.Create("Mariela", "Uvodić", new DateTime(2004,03,09), uvodicDict));
            users.Add(2, Tuple.Create("Duje", "OReilly",new DateTime (2002, 11, 20),  oreillyDict));
            users.Add(3, Tuple.Create("Katy", "Perry", new DateTime(1980, 12, 3), CreateAccountDictionary()));
            users.Add(4, Tuple.Create("Luka", "Modrić", new DateTime(1982, 10, 4), CreateAccountDictionary()));





            var choice = "0";
            var onGoing = true;
            do
            {
                Console.WriteLine("1 - Korisnici");
                Console.WriteLine("2 - Računi");
                Console.WriteLine("3 - Izlaz iz aplikacije");

                Console.Write("Unesite izbor: ");
                choice = Console.ReadLine();

                if (choice == "1" || choice == "2" || choice == "3")
                    switch (choice)
                    {
                        case ("1"):
                            UsersMainFunction();
                            break;
                        //case ("2"):
                        //    AccountsMainFunction();
                        //    break;
                        case ("3"):
                            onGoing = false;
                            break;
                        default:
                            Console.WriteLine("Default slucaj");
                            break;
                    }
                else
                {
                    Console.WriteLine("Ne valjan unos, probajte opet!");
                }
            } while (onGoing);

        }
    }
}