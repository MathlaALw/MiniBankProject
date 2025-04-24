namespace MiniBankProject
{
    internal class Program
    {
        // Constants
        const double MinimumBalance = 100.0;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";
        const string RequestsFilePath = "requests.txt";

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
            LoadRequsts();
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
                            SaveRequsts();
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
               // string[] lines = File.ReadAllLines(AccountsFilePath);
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

            Console.Clear();
            Console.WriteLine("-- Withdraw Money --");
            bool isTrue = false;
            while (!isTrue)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    int accountNumber = int.Parse(Console.ReadLine());
                    if (!accountNumbers.Contains(accountNumber))
                    {
                        Console.WriteLine("Invalid account number.");
                        isTrue = false;
                    }
                    else
                    {
                        Console.Write("Enter amount to withdraw: ");
                        double amount = double.Parse(Console.ReadLine());
                        if (amount <= 0)
                        {
                            Console.WriteLine("Amount must be greater than zero.");
                            isTrue = false;
                        }
                        else
                        {
                            int index = accountNumbers.IndexOf(accountNumber);
                            if (balances[index] - amount < MinimumBalance)
                            {
                                Console.WriteLine("Cannot withdraw " + amount + " Minimum balance of " + MinimumBalance + " must be maintained.");
                                isTrue = false;
                            }
                            else
                            {
                                isTrue = true;
                                balances[index] -= amount;
                                Console.WriteLine("Withdrew " + amount + " from account number " + accountNumber + " New balance: " + balances[index]);
                                Console.WriteLine("Press any key to return to the end user menu.");
                                Console.ReadKey();

                            }
                        }
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }



        }
        //4. Check Balance
        static void CheckBalance()
        {
            Console.Clear();
            Console.WriteLine("-- Check Balance --");
            try
            {
                Console.Write("Enter your account number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!accountNumbers.Contains(accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }
                int index = accountNumbers.IndexOf(accountNumber);
                Console.WriteLine("Account Name: " + accountNames[index]);
                Console.WriteLine("Account Balance: " + balances[index]);
                Console.WriteLine("Press any key to return to the end user menu.");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        //5. Submit a Review
        static void SubmitReview()
        {
            Console.Clear();
            Console.WriteLine("Submit a Review:");
            bool isTrue = false;
            string review = "";
            while (!isTrue)
            {
                Console.WriteLine("Enter your review: ");
                review = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(review))
                {
                    Console.WriteLine("Review cannot be empty.");
                    isTrue = false;
                }
                else
                {
                    isTrue = true;
                }
            }

            reviewsStack.Push(review);
            Console.WriteLine("Your review has been submitted.");
            Console.WriteLine("Press any key to return to the end user menu.");
            Console.ReadKey();
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





                Console.WriteLine("Do you want to approve this request? (y/n/p)");
                string requestStatus = Console.ReadLine();
                if (requestStatus.ToLower() == "y")
                {
                    // Console.WriteLine("Account approved.");
                    // Add the account to the system
                    request = createAccountRequests.Dequeue();
                    int accountNumber = ++lastAccountNumber;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Approved");



                    Console.WriteLine($"Account for {userName} has been created with account number {accountNumber}.");
                }
                else if (requestStatus.ToLower() == "n")
                {
                    // Console.WriteLine("Account request not approved.");
                    // Add the account to the system
                    request = createAccountRequests.Dequeue();

                    double initialBalanceDouble = double.Parse(initialBalance);
                    int accountNumber = ++lastAccountNumber;
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Not Approved");
                }

            }
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
        }
        //2. view account requests
        static void ViewAccountRequests()
        {
            Console.Clear();
            Console.WriteLine("Request:");
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No  requests available.");
            }
            else
            {
                string request = createAccountRequests.Peek();

                string[] splitlineOfRequest = request.Split(":");
                string userName = splitlineOfRequest[0];
                string nationalId = splitlineOfRequest[1];
                string initialBalance = splitlineOfRequest[2];
                string inialRequestStatus = splitlineOfRequest[3];



                //Console.WriteLine("view account Request");
                Console.WriteLine("user name : " + userName);
                Console.WriteLine("national Id : " + nationalId);
                Console.WriteLine("initial balance : " + initialBalance);
                Console.WriteLine("request status : " + inialRequestStatus);

            }
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
        }
        //3. view all account requests
        static void ViewAllAccountRequests()
        {
            try
            {
                Console.Clear();
            Console.WriteLine("\n--- All Accounts ---");
            for (int i = 0; i < accountNumbers.Count; i++)
            {
                //Console.WriteLine($"{accountNumbers[i]} - {accountNames[i]} - Balance: {balances[i]} - ");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Account Number: " + accountNumbers[i]);
                Console.WriteLine("Account Name: " + accountNames[i]);
                Console.WriteLine("Account Balance: " + balances[i]);
                Console.WriteLine("Account Status: " + requestStatuse[i]);
                Console.WriteLine("--------------------------------------------------");
            }
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //3. View Reviews   
        static void ViewReviews()
        {
            Console.Clear();
            Console.WriteLine("Reviews:");
            if (reviewsStack.Count == 0)
            {
                Console.WriteLine("No reviews available.");
            }
            else
            {
                foreach (string review in reviewsStack)
                {
                    Console.WriteLine(review);
                }
            }
            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
        }

        //------------------//
        //Save and Load Methods
        //------------------//
        // save accounts information to file
        static void SaveAccountsInformationToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string dataLine = $"{accountNumbers[i]},{accountNames[i]},{balances[i]}";
                        writer.WriteLine(dataLine);
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }
        // save reviews to file
        static void SaveReviews()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ReviewsFilePath))
                {
                    foreach (string review in reviewsStack)
                    {
                        writer.WriteLine(review);
                    }
                }
                Console.WriteLine("Reviews saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }

        }
        // load accounts information from file
        static void LoadAccountsInformationFromFile()
        {
            try
            {

                if (!File.Exists(AccountsFilePath))
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }


                using (StreamReader reader = new StreamReader(AccountsFilePath, true))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                    // string[] lines = content.Split(":");

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {


                        string[] lines = File.ReadAllLines(AccountsFilePath);

                        int accountNum = int.Parse(lines[0]);
                        accountNumbers.Add(accountNum);
                        accountNames.Add(lines[1]);
                        nationalIds.Add(lines[2]);
                        balances.Add(Convert.ToDouble(lines[3]));
                        requestStatuse.Add(lines[4]);

                        if (accountNum > lastAccountNumber)
                            lastAccountNumber = accountNum;

                        string valu = reader.ReadToEnd();
                        Console.WriteLine(valu);
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }
        }
        // load reviews from file
        static void LoadReviews()
        {
            try
            {
                if (!File.Exists(ReviewsFilePath))
                {
                    Console.WriteLine("No saved reviews found.");
                    return;
                }
                using (StreamReader reader = new StreamReader(ReviewsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        reviewsStack.Push(line);
                    }
                }
                Console.WriteLine("Reviews loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }
        }

        // save requests to file
        static void SaveRequsts()
        {
            try
            {

                if (!File.Exists(RequestsFilePath))
                {
                    Console.WriteLine("No saved requests found.");

                }
                if (createAccountRequests.Count == 0)
                {
                    Console.WriteLine("No requests to save.");

                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(RequestsFilePath))
                    {
                        //foreach (string request in createAccountRequests)
                        //{
                        //    writer.WriteLine(request);
                        //}
                        string request = createAccountRequests.Peek();
                        for (int i = 0; i < createAccountRequests.Count; i++)
                        {
                            //string request = createAccountRequests.Peek();
                            string[] splitlineOfRequest = request.Split(":");
                            string accountNum = splitlineOfRequest[0];
                            string userName = splitlineOfRequest[1];
                            string initialBalance = splitlineOfRequest[2];
                            string inialRequestStatus = splitlineOfRequest[3];


                            string requestInOneLine = accountNum + ":" + userName + ":" + initialBalance + ":" + inialRequestStatus;
                            writer.WriteLine(requestInOneLine);
                        }
                        request = createAccountRequests.Dequeue();
                    }
                    Console.WriteLine("Requests saved successfully.");
                }
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }

        }
        // load requests from file
        static void LoadRequsts()
        {
            try
            {
                if (!File.Exists(RequestsFilePath))
                {
                    Console.WriteLine("No saved requests found.");
                    return;
                }
                using (StreamReader reader = new StreamReader(RequestsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        createAccountRequests.Enqueue(line);
                        string[] splitlineOfRequest = line.Split(":");
                        string userName = splitlineOfRequest[0];
                        string nationalId = splitlineOfRequest[1];
                        string initialBalance = splitlineOfRequest[2];
                        string inialRequestStatus = splitlineOfRequest[3];
                        //Console.WriteLine("view account Request");
                        Console.WriteLine("user name : " + userName);
                        Console.WriteLine("national Id : " + nationalId);
                        Console.WriteLine("initial balance : " + initialBalance);
                        Console.WriteLine("request status : " + inialRequestStatus);

                    }
                }
                Console.WriteLine("Requests loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }

        }






    }
}
