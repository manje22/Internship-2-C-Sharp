
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
                Console.WriteLine($"{user.Key} - {user.Value.Item1} - {user.Value.Item2} - {user.Value.Item3.ToString("yyyy-MM-dd")}");
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

        static int FindUserIdByName(string firstName, string lastName)
        {
            foreach (var user in users)
            {
                if (string.Equals(user.Value.Item1, firstName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(user.Value.Item2, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    return user.Key;  // Return the ID immediately if we find a match
                }
            }
            return -1;  // Return -1 if no match is found
        }

        static Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>> ChooseAccount(int userID)
        {
            var user = users[userID];
            var userAccounts = user.Item4; //dictionary with accounts
            var accountOptions = new[] { "ziro", "tekuci", "prepaid" };


            //choosing what account to work with
            var accountChoice = "";

            do
            {
                Console.WriteLine($"Opcije za racun su: {string.Join(", ", accountOptions)}");
                Console.Write("Unesite izbor: ");
                accountChoice = Console.ReadLine();

            }while (!accountOptions.Contains(accountChoice));

            var accountReturn = userAccounts[accountChoice];
            Console.WriteLine($"Odabrani racun {accountChoice}, iznos: {accountReturn.Item1.ToString()}");

            return accountReturn;
        }

        //FUNCKIJE ZA RAD S RACUNIMA
        //-----------------------------------------------------------------------//

        static void AddNewTransactionHelper(Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>> account, bool current = true)
        {
            var dateTimeOfTrans = DateTime.Now;
            var dateParsed = false;
            if (!current)
            {
                do
                {
                    Console.WriteLine("Unesite datum i vrijeme transakcije u formatu 'yyyy-MM-dd HH:mm'");
                    var inputDatetime = Console.ReadLine();
                    dateParsed = DateTime.TryParseExact(inputDatetime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeOfTrans);

                } while (!dateParsed);
                
                Console.WriteLine($"Unesen datum i vrijeme {dateTimeOfTrans}");
            }

            //Input of remaining transaction information

            //Input amount
            var amount = 0.00d;
            Console.Write("Unesite iznos transakcije: ");
            var amountInput = Console.ReadLine();
            var amountParse = double.TryParse(amountInput, out amount);
            
            while(!amountParse)
            {
                Console.WriteLine("Iznos unesen u krivom formatu, probajte opet: ");
                amountInput = Console.ReadLine();
                amountParse = double.TryParse(amountInput, out amount);
            }

            
            amount = Math.Round(amount,2); //To ensure the amount is only two decimal points

            Console.WriteLine($"Uspjesno unesen iznos: {amount}");

            //Transaction description (opis transakcije)
            var transDesc = "standardna transakcija";
            Console.Write("Zelite li dodati svoj opis transakcije? (d/n)");
            var transDescInput = Console.ReadLine().ToLower();

            if( transDescInput == "d" )
            {
                Console.Write("Unesite svoj opis: ");
                transDesc = Console.ReadLine();

                if( transDesc.Length < 0)
                {
                    do
                    {
                        Console.Write("Unijeli ste prazan opis, unesite valjan opis: ");
                        transDesc = Console.ReadLine();
                    } while (transDesc.Length > 0);
                }
                
            }

            Console.WriteLine($"Uspjesno unesen opis: {transDesc}");

            //Type (Prihod ili rashod)

            var typesOptions = Tuple.Create("prihod", "rashod");
            var typeDesc = "";
            var category = "";
            Console.WriteLine("Unesite tip transakcije");
            Console.WriteLine("1-prihod");
            Console.WriteLine("2-rashod");

            var typeInput = Console.ReadLine();

            if (typeInput != "1" && typeInput != "2")
            {
                do
                {
                    Console.Write("Niste unili valjan unos, probajte opet");
                    typeInput = Console.ReadLine();
                } while (typeInput != "1" || typeInput != "2");
            }

            switch(typeInput)
            {
                case "1":
                    Console.WriteLine("Unijeli ste opciju 1- prihod");
                    typeDesc = typesOptions.Item1;
                    break;
                case "2":
                    Console.WriteLine("Unijeli ste opciju 2- rashod");
                    typeDesc = typesOptions.Item2;
                    break;
                default:
                    Console.WriteLine("Dogodila se greska");
                    break;
            }

            category = CategoryInputFunction(typeDesc);


            //Categories
            string CategoryInputFunction(string type)
            {
                var categoriesIncome = new List<string> { "placa", "honorar", "poklon", "prijenos", "interes" };
                var categoriesExpense = new List<string> { "hrana", "prijevoz", "sport", "odjeca", "zdravlje" };
                var categoryInput = "";

                switch (type)
                {
                    case "prihod":
                        Console.WriteLine($"Opcije za prihod su: {String.Join(", ", categoriesIncome)}");
                        categoryInput = Console.ReadLine().Trim().ToLower();

                        while (!categoriesIncome.Contains(categoryInput))
                        {
                            Console.Write("Krivo uneseno, unesite odgovarajucu kategoriju: ");
                            categoryInput = Console.ReadLine().Trim().ToLower();
                        }
                        break;
                    case "rashod":
                        Console.WriteLine($"Opcije za rashod su: {String.Join(", ", categoriesExpense)}");
                        categoryInput = Console.ReadLine().Trim().ToLower();

                        while (!categoriesExpense.Contains(categoryInput))
                        {
                            Console.Write("Krivo uneseno, unesite odgovarajucu kategoriju: ");
                            categoryInput = Console.ReadLine().Trim().ToLower();
                        }
                        break;
                    default:
                        Console.WriteLine("Doslo je do greske, ide default kategorija");
                        break;
                }

                Console.WriteLine($"Uspjesno unesena katagorija {categoryInput}");
                return categoryInput;
            }

            Console.WriteLine($"Nova transakcija: {amount}-{transDesc}-{typeDesc}-{category}-{dateTimeOfTrans}");
            //Adding the values to the account now
            var newTupleValue = createNewAccountTuple(amount, transDesc, typeDesc, category, dateTimeOfTrans, account);

            account = newTupleValue;

        }

        static Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>> createNewAccountTuple(double amount, string description, string type, string category, DateTime dateTimeExec, Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>> accountTuple)
        {
            var newAmount = 0d;
            if (type == "rashod")
            {
                newAmount = accountTuple.Item1 - amount;
            }
            else
                newAmount = accountTuple.Item1 + amount;

            var tupleValue = Tuple.Create(amount, description, type, category, dateTimeExec);

            accountTuple.Item2.Add(accountTuple.Item2.Count, tupleValue);

            var newAccountTuple = Tuple.Create(newAmount, accountTuple.Item2);

            return newAccountTuple;
        }

        static void AddNewTransactionMain(Tuple<double, Dictionary<int, Tuple<double, string, string, string, DateTime>>> account) //takes the account we are working on
        {
            var choice = "";

            do
            {
                Console.WriteLine("Odabrali ste unos nove transakcije, kakvu transakciju zelite unit: ");
                Console.WriteLine("a) trenutno izvrsena transakcija");
                Console.WriteLine("b) ranije izvrsena transakcija (potrebno upisati datum i vrijeme)");

                choice = Console.ReadLine();

            } while (choice != "a" && choice != "b");

            switch (choice)
            {
                case "a":
                    Console.WriteLine("Odabrali ste dodati trenutno izvrsenu transakciju");
                    AddNewTransactionHelper(account);
                    break;
                case "b":
                    AddNewTransactionHelper(account, false);
                    break;
            }

            Console.WriteLine("Uspjesno dodana transakcija, slijedi ispis svih transakcija");

            foreach (var kvp in account.Item2)
            {
                Console.WriteLine($"{kvp.Key}-{kvp.Value.Item1}-{kvp.Value.Item2}-{kvp.Value.Item3}-{kvp.Value.Item4}-{kvp.Value.Item5}");
            }
        }

        static void AccountsMainFunction()
        {
            Console.Write("Unesite ime: ");
            var chosenFirstName = Console.ReadLine().Trim();
            Console.Write("Unesite prezime: ");
            var chosenSurName = Console.ReadLine().Trim();

            var chosenUserId = FindUserIdByName(chosenFirstName, chosenSurName);

            if (chosenUserId != -1)
            {
                var user = users[chosenUserId];
                Console.WriteLine($"Rad na racunima-korisnik : {user.Item1} {user.Item2} s ID-jem {chosenUserId}");
            }
            else
            {
                Console.WriteLine($"Nije pronađen korisnik {chosenFirstName} {chosenSurName}");
                return;
            }

            var workingAccount = ChooseAccount(chosenUserId);
            Console.WriteLine("Uspjesno odabran racun, što želite dalje?");

            var ongoing = true;
            var choice = "";
            List<string> choices = new List<string> { "1", "2", "3", "4" };

            do
            {
                Console.WriteLine("1- Unos nove transakcije");
                Console.WriteLine("2- Brisanje transakcije");
                Console.WriteLine("3 - Uredivanje transakcije");
                Console.WriteLine("4 - Pregled transakcije");
                Console.WriteLine("5 - Financijsko izvjesce");
                Console.WriteLine("6 - Za kraj rada");
                Console.Write("Unesite svoj izbor za rad s racunima: ");
                choice = Console.ReadLine().Trim();


                if (choices.Contains(choice))
                    ongoing = false;
                else
                    Console.WriteLine("Ne valjan unos, probaj opet!");

            } while (ongoing);


            switch(choice)
            {
                case "1":
                    AddNewTransactionMain(workingAccount);
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                default:
                    Console.WriteLine("Nešto se dogosdilo, povratak");
                    break;
            }


        }


        static void Main(string[] args)
        {
            //Glavni dict sa korisnicima
            //korisnik ima id-key -> value (imePrezime, dictionary)

            var uvodicDict = CreateAccountDictionary();
            var oreillyDict = CreateAccountDictionary();

            users.Add(1, Tuple.Create("Mariela", "Uvodic", new DateTime(2004,03,09), uvodicDict));
            users.Add(2, Tuple.Create("Duje", "OReilly",new DateTime (2002, 11, 20),  oreillyDict));
            users.Add(3, Tuple.Create("Katy", "Perry", new DateTime(1980, 12, 3), CreateAccountDictionary()));
            users.Add(4, Tuple.Create("Luka", "Modric", new DateTime(1982, 10, 4), CreateAccountDictionary()));





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
                        case ("2"):
                            AccountsMainFunction();
                            break;
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