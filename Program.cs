namespace MiniBankProject
{
    internal class Program
    {
        // Constants
        const double MinimumBalance = 100.0;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";

        // Global lists (parallel)
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<string> nationalIds = new List<string>();
        static List<double> balances = new List<double>();
        static List<string> requestStatuse = new List<string>();
      

        // Queues and Stacks
        static Queue<string> createAccountRequests = new Queue<string>();
        static Stack<string> reviewsStack = new Stack<string>();

        // Account number generator
        static int lastAccountNumber;

        // Main method
        static void Main(string[] args)
        {

            LoadAccountsInformationFromFile();
            LoadReviews();
            bool runAgain = true;
            while (runAgain) //loop until the user enter 3 to exit
            {
                try //handle the exception if the user enter invalid input
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to Mini Bank System!");
                    Console.WriteLine("1. End User");
                    Console.WriteLine("2. Admin");
                    Console.WriteLine("3. Exit");

                    Console.WriteLine("Select Option ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            EndUserMenu();
                            break;
                        case "2":
                            AdminMenu();
                            break;
                        case "0":
                            SaveAccountsInformationToFile();
                            SaveReviews();
                            runAgain = false;
                            break;

                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }


                }
                catch (Exception e)//show exception message if the user enter invalid input
                {
                    Console.WriteLine(e.Message);


                    Console.WriteLine("Invalid Choice! Try again.");
                    Console.WriteLine("Press any key  "); //ask user to press any key to continue
                    Console.ReadLine(); //read the user inputConsole.ReadLine();
                }

            }

            // End User Menu
            static void EndUserMenu()
            {
                bool runUser = true;
                while (runUser) //loop until the user enter 3 to exit
                {
                    try
                    {


                        Console.Clear();
                        Console.WriteLine("End User Menu:");
                        Console.WriteLine("1. Request Account Creation");
                        Console.WriteLine("2. Deposit Money");
                        Console.WriteLine("3. Withdraw Money");
                        Console.WriteLine("4. Check Balance");
                        Console.WriteLine("5. submit a Review");
                        Console.WriteLine("6. View account Details");
                        Console.WriteLine("0. Exit to Main Menu");

                        string userChoice = Console.ReadLine();
                        switch (userChoice)
                        {
                            case "1":
                                RequestAccountCreation();
                                break;
                            case "2":
                                DepositMoney();
                                break;
                            case "3":
                                WithdrawMoney();
                                break;
                            case "4":
                                CheckBalance();
                                break;
                            case "5":
                                SubmitReview();
                                break;
                            case "6":
                                viewAccountDetails();
                                break;
                            case "0":
                                runUser = false;
                                break;
                            default:
                                Console.WriteLine("Invalid choice, please try again.");
                                break;
                        }


                    }
                    catch (Exception e)//show exception message if the user enter invalid input
                    {
                        Console.WriteLine(e.Message);


                        Console.WriteLine("Invalid Choice! Try again.");
                        Console.WriteLine("Press any key  "); //ask user to press any key to continue
                        Console.ReadLine(); //read the user inputConsole.ReadLine();
                    }

                }
            }
            // Admin Menu//
            static void AdminMenu()
            {
                bool runAdmin = true;
                while (runAdmin)
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("Admin Menu:");
                        Console.WriteLine("1. View Account Requests");
                        Console.WriteLine("2. Approve Account Request");
                        Console.WriteLine("3. View Reviews");
                        Console.WriteLine("4. View All Account Requests");
                        Console.WriteLine("0. Exit to Main Menu");
                        string adminChoice = Console.ReadLine();
                        switch (adminChoice)
                        {
                            case "1":
                                ViewAccountRequests();
                                break;
                            case "2":
                                ApproveAccountRequest();
                                break;
                            case "3":
                                ViewReviews();
                                break;
                            case "4":
                                ViewAllAccountRequests();
                                break;
                            case "0":
                                runAdmin = false;
                                break;
                            default:
                                Console.WriteLine("Invalid choice, please try again.");
                                break;
                        }

                    }
                    catch (Exception e)//show exception message if the user enter invalid input
                    {
                        Console.WriteLine(e.Message);


                        Console.WriteLine("Invalid Choice! Try again.");
                        Console.WriteLine("Press any key  "); //ask user to press any key to continue
                        Console.ReadLine(); //read the user inputConsole.ReadLine();
                    }
                }

            }
        }
            //------------------//
            // End User UseCases
            //------------------//
            // 1. Request Account Creation
        static void RequestAccountCreation()
        {
            Console.Clear();
            Console.WriteLine("--Request Account Creation--");
            Console.WriteLine("----------------------------");

            bool isValidName = false;
            bool isValidNationalId = false;
            bool isValidinitialBalance = false;
            string userName = "";
            string nationalId = "";
            string initialBalance = "";
            try
            {

                // Get and validate name
                while (!isValidName)
                {


                    Console.Write("Enter your name: ");
                    userName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        Console.WriteLine("Name cannot be empty.");
                        isValidName = false;

                    }
                    else if (int.TryParse(userName, out int result))
                    {
                        Console.WriteLine("Name cannot be a number.");
                        isValidName = false;
                    }
                    else if (userName.Length < 3)
                    {
                        Console.WriteLine("Name must be at least 3 characters long.");
                        isValidName = false;
                    }


                    else
                    {

                        isValidName = true;

                    }


                }

                // Get and validate national ID
                while (!isValidNationalId)
                {
                    Console.Write("Enter your National ID: ");
                    nationalId = Console.ReadLine();


                    if (string.IsNullOrEmpty(nationalId))
                    {
                        Console.WriteLine("National ID cannot be empty.");
                        isValidNationalId = false;
                    }
                    else if (!int.TryParse(nationalId, out int result))
                    {
                        Console.WriteLine("Canot be string ");
                        isValidNationalId = false;
                    }
                    else
                    {

                        isValidNationalId = true;

                    }


                }
                // git initial balance

                while (!isValidinitialBalance)
                {
                    Console.WriteLine("Entur your initial balance: ");
                    initialBalance = Console.ReadLine();


                    if (string.IsNullOrEmpty(initialBalance))
                    {
                        Console.WriteLine("Initial balance cannot be empty.");
                        isValidinitialBalance = false;
                    }
                    else if (!double.TryParse(initialBalance, out double result))
                    {
                        Console.WriteLine("Initial balance must be a number.");
                        isValidinitialBalance = false;
                    }
                    else if (double.Parse(initialBalance) < MinimumBalance)
                    {
                        Console.WriteLine($"Initial balance must be at least 100.0 OMR.");
                        isValidinitialBalance = false;
                    }

                    else
                    {
                        isValidinitialBalance = true;

                    }
                }

                string request = userName + ":" + nationalId + ":" + initialBalance;
                createAccountRequests.Enqueue(request);
                Console.WriteLine("Your account request has been submitted.");
                Console.WriteLine($"Your account number is ({lastAccountNumber + 1}) Please wait for approval.");

                Console.WriteLine("Press any key to return to the end user menu.");
                Console.ReadKey();


            }

            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                isValidName = false;
                isValidNationalId = false;
                isValidinitialBalance = false;
            }

        }

        //2. Deposit Money
        static void DepositMoney()
        {
            int accountNumber = 0;
            Console.Clear();
            Console.WriteLine("-- Deposit Money --");
            bool isTrue = false;
            while (!isTrue)
            {
                Console.Write("Enter your account number: ");
                accountNumber = int.Parse(Console.ReadLine());
                if (!accountNumbers.Contains(accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    isTrue = false;

                }
                else
                {

                    Console.Write("Enter amount to deposit: ");
                    double amount = double.Parse(Console.ReadLine());
                    if (amount <= 0)
                    {
                        Console.WriteLine("Amount must be greater than zero.");
                        isTrue = false;

                    }
                    else
                    {
                        isTrue = true;
                        int index = accountNumbers.IndexOf(accountNumber);
                        balances[index] += amount;

                        Console.WriteLine("Deposit " + (amount) + " successful To account number " + (accountNumber) + " . Your new Balance is : " + balances[index]);

                    }
                }
            }

            Console.WriteLine("Press any key to return to the end user menu.");
            Console.ReadKey();


        }

        //3. Withdraw Money
        static void WithdrawMoney()
        {
         



        }
        //4. Check Balance
        static void CheckBalance()
        {
         
        }
        //5. Submit a Review
        static void SubmitReview()
        {
            
        }
        //6. View Account Details
        static void viewAccountDetails()
        {
            
        }
        //-------------------//
        // Admin UseCases // 
        //-------------------//
        //1. Approve Account Request
        static void ApproveAccountRequest()
        {
            Console.Clear();
            Console.WriteLine("Approve Account Request:");
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No account requests available.");
            }
            else
            {
                string request = createAccountRequests.Dequeue();
                //Console.WriteLine("Processing request: " + request);
                string[] splitlineOfRequest = request.Split(":");
                string userName = splitlineOfRequest[0];
                string nationalId = splitlineOfRequest[1];
                string initialBalance = splitlineOfRequest[2];


                Console.WriteLine("view account Request");
                Console.WriteLine("user name : " + userName);
                Console.WriteLine("national Id : " + nationalId);
                Console.WriteLine("initial balance : " + initialBalance);


                // Console.WriteLine("Enter national id ");
                Console.WriteLine("Do you want to approve this request? (y/n)");
                string approve = Console.ReadLine();
                if (approve.ToLower() == "y")
                {
                    Console.WriteLine("Account approved.");
                    // Add the account to the system
                    int accountNumber = ++lastAccountNumber;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Approved");


                    //transactions.Add(new Queue<string>());
                    Console.WriteLine($"Account for {userName} has been created with account number {accountNumber}.");
                }
                else
                {
                    Console.WriteLine("Account approved.");
                    // Add the account to the system
                    int accountNumber = ++lastAccountNumber;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Not Approved");
                    Console.WriteLine("Account request not approved");
                }

            }
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
        }
        //2. view account requests
        static void ViewAccountRequests()
        {
           
        }
        //3. view all account requests
        static void ViewAllAccountRequests()
        {

          
        }

        //3. View Reviews
        static void ViewReviews()
        {
           
        }

        //------------------//
        //Save and Load Methods
        //------------------//
        // save accounts information to file
        static void SaveAccountsInformationToFile()
        {
          
        }
        // save reviews to file
        static void SaveReviews()
        {
           

        }
        // load accounts information from file
        static void LoadAccountsInformationFromFile()
        {
          
        }
        // load reviews from file
        static void LoadReviews()
        {
           
        }








    }
}
