using System.Data;

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
        static List<string> userTypes = new List<string>();


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
            while (runAgain)
            {
                try //handle the exception if the user enter invalid input
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to Mini Bank System!");
                    Console.WriteLine("1. create New account");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("0. Exit");

                    Console.WriteLine("Select Option ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            CreateNewAccount();
                            break;
                        case "2":
                            Login();
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
                    
                    Console.WriteLine("1. Deposit Money");
                    Console.WriteLine("2. Withdraw Money");
                    Console.WriteLine("3. Check Balance");
                    Console.WriteLine("4. submit a Review");
                    Console.WriteLine("5. View account Details");
                    Console.WriteLine("6. Transfer Between Accounts");
                    Console.WriteLine("0. Exit to Main Menu");

                    string userChoice = Console.ReadLine();
                    switch (userChoice)
                    {
                     
                        case "1":
                            DepositMoney();
                            break;
                        case "2":
                            WithdrawMoney();
                            break;
                        case "3":
                            CheckBalance();
                            break;
                        case "4":
                            SubmitReview();
                            break;
                        //case "5":
                        //    viewAccountDetails();
                        //    break;
                        case "6":
                            TransferBetweenAccounts();
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
                    Console.WriteLine("5. Delete Account");
                    Console.WriteLine("6. Search Account");
                    Console.WriteLine("7. Show Total Bank Balance");
                    Console.WriteLine("8. Show Top 3 Richest Customers");
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
                        case "5":
                            DeleteAccount();
                            break;
                        case "6":
                            SearchAccount();
                            break;
                        case "7":
                            ShowTotalBankBalance();
                            break;
                        case "8":
                            ShowTop3RichestCustomers();
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
        static void CreateNewAccount()
        {
            Console.Clear();
            Console.WriteLine("-- Create New Account --");
            Console.WriteLine("-------------------------");

            bool isValidName = false;
            bool isValidNationalId = false;
            bool isValidInitialBalance = false;
            bool isValidUserType = false;
            string userName = "";
            string nationalId = "";
            double initialBalance = 0;
            string userType = "";

            try
            {

                // Validate Name
                while (!isValidName)
                {
                    Console.Write("Enter your name: ");
                    userName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        Console.WriteLine("Name cannot be empty.");
                        isValidName = false;
                    }
                    else if (userName.Length < 3)
                    {
                        Console.WriteLine("Name must be at least 3 characters long.");
                        isValidName = false;
                    }
                    else if (int.TryParse(userName, out int result))
                    {
                        Console.WriteLine("Name cannot be a number.");
                        isValidName = false;
                    }
                    else
                        isValidName = true;
                }

                // Validate National ID
                while (!isValidNationalId)
                {
                    Console.Write("Enter your National ID: ");
                    nationalId = Console.ReadLine();

                    // git national id from the file
                    string[] lines = File.ReadAllLines(AccountsFilePath);
                    foreach (string line in lines)
                    {
                        string fileNationalId = "";
                        string[] parts = line.Split(':');
                        if (parts.Length >= 3)
                        {
                            //nationalIds.Add(parts[2]);
                             fileNationalId = parts[2];
                          
                        }
                        if (fileNationalId == nationalId)
                        {

                            Console.WriteLine("This National ID already exists in the system. Cannot create duplicate account.");
                            isValidNationalId = false;
                            break;
                        }
                        else if (string.IsNullOrWhiteSpace(nationalId))
                        {
                            Console.WriteLine("National ID cannot be empty.");
                            isValidNationalId = false;
                        }
                        else if (!double.TryParse(nationalId, out double result))
                        {
                            Console.WriteLine("National ID must be numbers only.");
                            isValidNationalId = false;
                        }
                        else
                            isValidNationalId = true;
                        }
                   
                }


                while (!isValidUserType)
                {
                    Console.Write("Enter your user type (user/admin): ");
                    userType = Console.ReadLine();

                    if (userType != "user" && userType != "admin")
                        Console.WriteLine("User type must be either 'user' or 'admin'.");
                    else
                    {
                        isValidUserType = true;
                        if (userType == "admin")
                        {
                            lastAccountNumber = GetTheLastAccountNumberFromAccountFile();
                           //string status = "panding"; // Default status

                            // Save the admin information to the file
                            
                            accountNames.Add(userName);
                            nationalIds.Add(nationalId);
                            balances.Add(initialBalance);
                            requestStatuse.Add("panding");
                            userTypes.Add(userType);
                         
                            string adminAccountLine = userName + ":" + nationalId + ":" + "0.0" + ":" + "panding" + ":" + "admin";
                            createAccountRequests.Enqueue(adminAccountLine);
                            Console.WriteLine("Admin account created successfully!");
                            Console.WriteLine($"Your new account number is: {lastAccountNumber + 1}");
                            Console.WriteLine("Press any key to return to the menu...");
                            Console.ReadKey();

                        }
                        if (userType == "user")
                        {
                        

                            // Validate Initial Balance
                            while (!isValidInitialBalance)
                            {
                                Console.Write("Enter your initial balance: ");
                                string balanceInput = Console.ReadLine();

                                if (!double.TryParse(balanceInput, out initialBalance))
                                    Console.WriteLine("Please enter a valid number for balance.");
                                else if (initialBalance < MinimumBalance)
                                    Console.WriteLine($"Initial balance must be at least {MinimumBalance} OMR.");
                                else
                                    isValidInitialBalance = true;
                            }

                            // Get last account number
                            lastAccountNumber = GetTheLastAccountNumberFromAccountFile();
                            int newAccountNumber = lastAccountNumber + 1;

                          // string status = "panding"; // Default status

                            // Add to lists 

                            accountNames.Add(userName);
                            nationalIds.Add(nationalId);
                            balances.Add(initialBalance);
                            requestStatuse.Add("panding");
                            userTypes.Add(userType);



                            string request = userName + ":" + nationalId + ":" + initialBalance + ":" + "panding" + ":" + "user";
                            createAccountRequests.Enqueue(request);

                            Console.WriteLine("\nAccount created successfully!");
                           // Console.WriteLine(request);
                            Console.WriteLine($"Your new account number is: {newAccountNumber}");


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        static void Login()
        {
            Console.Clear();
            Console.WriteLine("-- Login --");
            Console.Write("Enter your National ID: ");
            string inputNationalId = Console.ReadLine();
            

            Console.Write("Enter your user type (admin/user): ");
            string inputUserType = Console.ReadLine().ToLower();

            bool found = false;

            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines(AccountsFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 6)
                {
                    string fileNationalId = parts[2];
                    string fileUserType = parts[5];
                    string accountStatus = parts[4]; // Check if the account is Approved

                    if (fileNationalId == inputNationalId && fileUserType == inputUserType)
                    {
                        if (accountStatus != "Approved")
                        {
                            Console.WriteLine("\nYour account is not approved yet. Please wait for approval.");
                            Console.WriteLine("Press any key to return to the main menu.");
                            Console.ReadKey();
                            return;
                        }

                        found = true;
                        if (fileUserType == "admin")
                        {
                            AdminMenu();
                        }
                        else if (fileUserType == "user")
                        {
                            EndUserMenu();
                        }
                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("\nInvalid National ID or User Type. Please try again.");
                Console.WriteLine("Press any key to return to the main menu.");
                Console.ReadKey();
            }
        }
        //------------------//
        // End User UseCases
        //------------------//
        // 1. Request Account Creation

        //static void RequestAccountCreation()
        //{
        //    Console.Clear();
        //    Console.WriteLine("--Request Account Creation--");
        //    Console.WriteLine("----------------------------");

        //    bool isValidName = false;
        //    bool isValidNationalId = false;
        //    bool isValidinitialBalance = false;
        //    string userName = "";
        //    string nationalId = "";
        //    string initialBalance = "";
        //    try
        //    {

        //        // Get and validate name
        //        while (!isValidName)
        //        {


        //            Console.Write("Enter your name: ");
        //            userName = Console.ReadLine();

        //            if (string.IsNullOrWhiteSpace(userName))
        //            {
        //                Console.WriteLine("Name cannot be empty.");
        //                isValidName = false;

        //            }
        //            else if (int.TryParse(userName, out int result))
        //            {
        //                Console.WriteLine("Name cannot be a number.");
        //                isValidName = false;
        //            }
        //            else if (userName.Length < 3)
        //            {
        //                Console.WriteLine("Name must be at least 3 characters long.");
        //                isValidName = false;
        //            }


        //            else
        //            {

        //                isValidName = true;

        //            }


        //        }

        //        // Get and validate national ID
        //        while (!isValidNationalId)
        //        {
        //            Console.Write("Enter your National ID: ");
        //            nationalId = Console.ReadLine();


        //            if (string.IsNullOrEmpty(nationalId))
        //            {
        //                Console.WriteLine("National ID cannot be empty.");
        //                isValidNationalId = false;
        //            }
        //            else if (!int.TryParse(nationalId, out int result))
        //            {
        //                Console.WriteLine("Canot be string ");
        //                isValidNationalId = false;
        //            }
        //            else
        //            {

        //                isValidNationalId = true;

        //            }


        //        }
        //        // git initial balance

        //        while (!isValidinitialBalance)
        //        {
        //            Console.WriteLine("Entur your initial balance: ");
        //            initialBalance = Console.ReadLine();


        //            if (string.IsNullOrEmpty(initialBalance))
        //            {
        //                Console.WriteLine("Initial balance cannot be empty.");
        //                isValidinitialBalance = false;
        //            }
        //            else if (!double.TryParse(initialBalance, out double result))
        //            {
        //                Console.WriteLine("Initial balance must be a number.");
        //                isValidinitialBalance = false;
        //            }
        //            else if (double.Parse(initialBalance) < MinimumBalance)
        //            {
        //                Console.WriteLine($"Initial balance must be at least 100.0 OMR.");
        //                isValidinitialBalance = false;
        //            }

        //            else
        //            {
        //                isValidinitialBalance = true;

        //            }
        //        }
        //        string initialRequestStatus = "pending";
        //        string request = userName + ":" + nationalId + ":" + initialBalance + ":" + initialRequestStatus; ;
        //        createAccountRequests.Enqueue(request);



        //        //git the last account number from the file
        //        int lastAcc = GitTheLastAccountNumberFromAccountFile();
        //        Console.WriteLine("Your account request has been submitted. Please wait for approval.");
        //        Console.WriteLine("Your account number is (" + (lastAcc + 1) + ") .");

        //        Console.WriteLine("Press any key to return to the end user menu.");
        //        Console.ReadKey();


        //    }

        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        //        isValidName = false;
        //        isValidNationalId = false;
        //        isValidinitialBalance = false;
        //    }
        //    Console.ReadLine();
        //}

        //1. Deposit Money
        static void DepositMoney()
        {
            Console.Clear();
            Console.WriteLine("-- Deposit Money --");

            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    int enteredAccountNumber = int.Parse(Console.ReadLine());

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status == "Approved")
                                {
                                    Console.WriteLine("Account is approved.");
                                    Console.Write("Enter amount to deposit: ");
                                    double amount = double.Parse(Console.ReadLine());

                                    if (amount <= 0)
                                    {
                                        Console.WriteLine("Amount must be greater than zero.");
                                        break;
                                    }

                                    double newBalance = balance + amount;
                                    parts[3] = newBalance.ToString(); // Update balance part

                                    lines[i] = string.Join(":", parts);

                                    File.WriteAllLines(AccountsFilePath, lines);

                                    Console.WriteLine($"Deposited {amount} successfully!");
                                    Console.WriteLine($"New Balance: {newBalance}");
                                    isSuccess = true;
                                    break;
                                }
                                else if (status == "Not Approved")
                                {
                                    Console.WriteLine("Account is not approved yet.");
                                    break;
                                }
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account is not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }

        //2. Withdraw Money
        static void WithdrawMoney()
        {
            Console.Clear();
            Console.WriteLine("-- Withdraw Money --");

            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    if (!int.TryParse(Console.ReadLine(), out int enteredAccountNumber))
                    {
                        Console.WriteLine("Invalid account number. Please enter numbers only.");
                        continue;
                    }

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status != "Approved")
                                {
                                    Console.WriteLine("Account is not approved yet. Cannot withdraw money.");
                                    break;
                                }

                                Console.Write("Enter amount to withdraw: ");
                                if (!double.TryParse(Console.ReadLine(), out double amount))
                                {
                                    Console.WriteLine("Invalid amount. Please enter numbers only.");
                                    break;
                                }

                                if (amount <= 0)
                                {
                                    Console.WriteLine("Amount must be greater than zero.");
                                    break;
                                }

                                if (balance - amount < MinimumBalance)
                                {
                                    Console.WriteLine($"Cannot withdraw {amount}. Minimum balance of {MinimumBalance} must be maintained.");
                                    break;
                                }

                                double newBalance = balance - amount;
                                parts[3] = newBalance.ToString();

                                // Manual way to rebuild the line
                                lines[i] = parts[0] + ":" + parts[1] + ":" + parts[2] + ":" + parts[3] + ":" + parts[4];

                                File.WriteAllLines(AccountsFilePath, lines);

                                Console.WriteLine($"Withdrew {amount} successfully!");
                                Console.WriteLine($"New Balance: {newBalance}");
                                isSuccess = true;
                                break;
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account number not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }
        //3. Check Balance
        static void CheckBalance()
        {
            Console.Clear();
            Console.WriteLine("-- Check Balance --");

            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    if (!int.TryParse(Console.ReadLine(), out int enteredAccountNumber))
                    {
                        Console.WriteLine("Invalid account number. Please enter numbers only.");
                        continue;
                    }

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status != "Approved")
                                {
                                    Console.WriteLine("Account is not approved yet. Cannot check balance.");
                                    break;
                                }

                                Console.WriteLine($"Account Number: {fileAccountNumber}");
                                Console.WriteLine($"Account Name: {name}");
                                Console.WriteLine($"Balance: {balance:F2}"); // Formatted to 2 decimal places
                                isSuccess = true;
                                break;
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account number not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }
        //4. Submit a Review
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


        //5. Transfer Between Accounts
        static void TransferBetweenAccounts()
        {
            Console.Clear();
            Console.WriteLine("-- Transfer Between Accounts --");
            bool isSuccess = false;
            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    int enteredAccountNumber = int.Parse(Console.ReadLine());
                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }
                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];
                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;
                                if (status != "Approved")
                                {
                                    Console.WriteLine("Account is not approved yet. Cannot transfer money.");
                                    break;
                                }
                                Console.Write("Enter the recipients account number: ");
                                int recipientAccountNumber = int.Parse(Console.ReadLine());
                                // Check if recipient account exists
                                bool recipientFound = false;
                                for (int j = 0; j < lines.Count; j++)
                                {
                                    string[] recipientParts = lines[j].Split(':');
                                    if (recipientParts.Length >= 5)
                                    {
                                        int fileRecipientAccountNumber = int.Parse(recipientParts[0]);
                                        if (fileRecipientAccountNumber == recipientAccountNumber)
                                        {
                                            recipientFound = true;
                                            break;
                                        }
                                    }
                                }
                                if (!recipientFound)
                                {
                                    Console.WriteLine("Recipient account not found.");
                                    break;
                                }
                                Console.Write("Enter amount to transfer: ");
                                double amount = double.Parse(Console.ReadLine());
                                if (amount <= 0)
                                {
                                    Console.WriteLine("Amount must be greater than zero.");
                                    break;
                                }
                                if (balance - amount < MinimumBalance)
                                {
                                    Console.WriteLine($"Cannot transfer {amount}. Minimum balance of {MinimumBalance} must be maintained.");
                                    break;
                                }
                                // Update balances
                                double newBalanceSender = balance - amount;
                                double newBalanceRecipient = 0;
                                // Update sender balance
                                parts[3] = newBalanceSender.ToString();
                                lines[i] = string.Join(":", parts);
                                // Update recipient balance
                                for (int j = 0; j < lines.Count; j++)
                                {
                                    string[] recipientParts = lines[j].Split(':');
                                    if (recipientParts.Length >= 5)
                                    {
                                        int fileRecipientAccountNumber = int.Parse(recipientParts[0]);
                                        if (fileRecipientAccountNumber == recipientAccountNumber)
                                        {
                                            newBalanceRecipient = double.Parse(recipientParts[3]) + amount;
                                            recipientParts[3] = newBalanceRecipient.ToString();
                                            lines[j] = string.Join(":", recipientParts);
                                            break;
                                        }
                                    }
                                }
                                File.WriteAllLines(AccountsFilePath, lines);
                                Console.WriteLine($"Transferred {amount} successfully!");
                                Console.WriteLine($"New Balance: {newBalanceSender}");
                                Console.WriteLine($"Recipient New Balance: {newBalanceRecipient}");
                                isSuccess = true;
                                break;
                            }
                        }

                    }
                    if (!accountFound)
                    {
                        Console.WriteLine("Account number not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                Console.WriteLine("Press any key to return to the End User Menu.");
                Console.ReadKey();
            }

        }



//5. View Account Details
//static void viewAccountDetails()
//{

//}


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
                
                // git the first request from the queue
                string request = createAccountRequests.Dequeue();

                string[] splitlineOfRequest = request.Split(":");
                if (splitlineOfRequest.Length < 5)
                {
                    Console.WriteLine("Invalid request format.");
                    return;
                }

                string userName = splitlineOfRequest[0];
                string nationalId = splitlineOfRequest[1];
                string initialBalance = splitlineOfRequest[2];
                string inialRequestStatus = splitlineOfRequest[3];
                string userType = splitlineOfRequest[4];


                Console.WriteLine("view account Request");
                Console.WriteLine("user name : " + userName);
                Console.WriteLine("national Id : " + nationalId);
                Console.WriteLine("initial balance : " + initialBalance);
                Console.WriteLine("request status : " + inialRequestStatus);
                Console.WriteLine("User Type : " + userType);



               // request = createAccountRequests.Dequeue(); // remove from memory
                Console.WriteLine("Do you want to approve this request? (y/n)");
                string requestStatus = Console.ReadLine();

                if (requestStatus.ToLower() == "y")
                {
                  
                  


                    Console.WriteLine("Account request approved.");
                    // Add the account to the system
                    string initialRequestStatus = "Approved";
                    int accountNumber = GitTheLastAccountNumberFromAccountFile() + 1;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Approved");
                    userTypes.Add(userType);
                    Console.WriteLine($"Account for {userName} has been created with account number {accountNumber}.{initialRequestStatus}");

                    string requestToRemove = request; // remove from memory
                    string first = GitTheFirstRequestInFileAndDelete().ToString(); // remove from file

                    Console.WriteLine($"Account for {userName} has been created with account number {accountNumber}.");

                }
                else
                {

                    //// Add the account to the system
                    //string initialRequestStatus = "Not Approved";
                    int accountNumber = GitTheLastAccountNumberFromAccountFile() + 1;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    //accountNumbers.Add(accountNumber);
                    string initialRequestStatus = "Not Approved";
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Not Approved");
                    userTypes.Add(userType);
                    Console.WriteLine("Account Request NotApproved");

                    //delete the request from the file
                    string requestToRemove = request; // remove from memory
                    string first = GitTheFirstRequestInFileAndDelete().ToString(); // remove from file





                }

            }

            Console.WriteLine("Press any key to return to the admin menu.");
            Console.ReadKey();
        }
        //2. view account requests
        static void ViewAccountRequests()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Request:");
                if (createAccountRequests.Count == 0)
                {
                    Console.WriteLine("No requests available.");

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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //3. view all account requests
        static void ViewAllAccountRequests()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\n--- All Accounts in File ---");
                // vie all account requests from the file
                Console.WriteLine("--------------------------------------------------");

           


                using (StreamReader reader = new StreamReader(AccountsFilePath, true))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                 

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {


                        string[] lines = File.ReadAllLines(AccountsFilePath);

                        string[] strings = line.Split(':');
                        int accountNum = int.Parse(strings[0]);
                        string accountName = strings[1];
                        string nationalId = strings[2];
                        double balance = double.Parse(strings[3]);
                        string requestStatus = strings[4];
                        string userType = strings[5];
                        string inialRequestStatus = strings[6];
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine("Account Number: " + accountNum);
                        Console.WriteLine("Account Name: " + accountName);
                        Console.WriteLine("National ID: " + nationalId);
                        Console.WriteLine("Account Balance: " + balance);
                        Console.WriteLine("Account Status: " + requestStatus);
                        Console.WriteLine("User Type: " + userType);
                        Console.WriteLine("Request Status: " + inialRequestStatus);
                        Console.WriteLine("--------------------------------------------------");


                        if (accountNum > lastAccountNumber)
                            lastAccountNumber = accountNum;

                        string valu = reader.ReadToEnd();
                        Console.WriteLine(valu);
                    }

                    // value from queue
                   Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("Account Requests in Queue:");
                    foreach (string request in createAccountRequests)
                    {
                        Console.WriteLine(request);
                    }

                }
                Console.WriteLine("Press any key to return to the admin menu.");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //4. View Reviews   
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

        // 5.Delete Account
        static void DeleteAccount()
        {
            Console.Clear();
            Console.WriteLine("-- Delete Account --");
            bool isSuccess = false;

            while (!isSuccess)
            {
                try
                {
                    Console.Write("Enter your account number: ");
                    int enteredAccountNumber = int.Parse(Console.ReadLine());

                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }

                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5)
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                if (status == "Approved")
                                {
                                    //Console.WriteLine("Account is approved.");
                                    Console.Write("Are you sure you want to delete this account? (y/n): ");
                                    string confirmation = Console.ReadLine();
                                    if (confirmation.ToLower() == "y")
                                    {
                                        lines.RemoveAt(i);
                                        File.WriteAllLines(AccountsFilePath, lines);
                                        Console.WriteLine("Account deleted successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Account deletion canceled.");
                                    }

                                    isSuccess = true;
                                    break;
                                }
                                else if (status == "Not Approved")
                                {
                                    Console.WriteLine("Account is not approved yet.");
                                    break;
                                }
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("Account is not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }

        //6. Search Account

        static void SearchAccount()
        {
            Console.Clear();
            Console.WriteLine("-- Search Account --");
            try
            {
                Console.Write("Enter your account number: ");
                int enteredAccountNumber = int.Parse(Console.ReadLine());
                bool accountFound = false;
                if (!File.Exists(AccountsFilePath))
                {
                    Console.WriteLine("Accounts file not found.");
                    return;
                }
                List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                foreach (string line in lines)
                {
                   
                    string[] parts = line.Split(':');
                    if (parts.Length >=5)
                    {
                        int fileAccountNumber = int.Parse(parts[0]);
                        string name = parts[1];
                        string nationalId = parts[2];
                        double balance = double.Parse(parts[3]);
                        string status = parts[4];
                        if (fileAccountNumber == enteredAccountNumber)
                        {
                            accountFound = true;
                            Console.WriteLine("Account Number: " + fileAccountNumber);
                            Console.WriteLine("Account Name: " + name);
                            Console.WriteLine("National ID: " + nationalId);
                            Console.WriteLine("Account Balance: " + balance);
                            Console.WriteLine("Account Status: " + status);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        
        }
        //7. Show Total Bank Balance
        static void ShowTotalBankBalance()
        {
            Console.Clear();
            Console.WriteLine("-- Total Bank Balance --");
            double totalBalance = 0;
            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                return;
            }
            List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 5)
                {
                    double balance = double.Parse(parts[3]);
                    totalBalance += balance;
                }
            }
            Console.WriteLine("Total Bank Balance: " + totalBalance);

            Console.WriteLine("Press any key to return to the End User Menu.");
            Console.ReadKey();
        }
      
        //8. Show Top 3 Richest Customers
        static void ShowTop3RichestCustomers()
        {
            Console.Clear();
            Console.WriteLine("-- Top 3 Richest Customers --");

            if (!File.Exists(AccountsFilePath))
            {
                Console.WriteLine("Accounts file not found.");
                return;
            }

            List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();

            List<string> accountNumbers = new List<string>();
            List<double> balances = new List<double>();

            // Loop through each line and extract account numbers and balances
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 5)
                {
                    string accountNumber = parts[0];
                    if (double.TryParse(parts[3], out double balance))
                    {
                        accountNumbers.Add(accountNumber);
                        balances.Add(balance);
                    }
                }
            }

            // Find top 3 richest 
            for (int i = 0; i < 3; i++)
            {
                double maxBalance = double.MinValue; // Initialize to the smallest value
                int maxIndex = -1;

                for (int j = 0; j < balances.Count; j++)
                {
                    if (balances[j] > maxBalance)
                    {
                        maxBalance = balances[j];
                        maxIndex = j;
                    }
                }

                if (maxIndex != -1)
                {
                    string accountNumber = accountNumbers[maxIndex];
                    string accountName = GetAccountName(accountNumber); 
                    Console.WriteLine($"Account Number: {accountNumber}, Account Name: {accountName}, Balance: {balances[maxIndex]} OMR ");

                    // Remove the richest customer from the list to display the next richest
                    balances.RemoveAt(maxIndex);
                    accountNumbers.RemoveAt(maxIndex);
                }
            }
            Console.WriteLine("Press any key to return to the End User Menu.");
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
                using (StreamWriter writer = new StreamWriter(AccountsFilePath, true))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string dataLine = accountNumbers[i] + ":" + accountNames[i] + ":" + nationalIds[i] + ":" + balances[i] + ":" + requestStatuse[i]+ ":"+ userTypes[i];
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
                using (StreamWriter writer = new StreamWriter(ReviewsFilePath, true))
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
                        userTypes.Add(lines[5]);

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
                        //string request = createAccountRequests.Peek();
                        for (int i = 0; i < createAccountRequests.Count; i++)
                        {
                            string request = createAccountRequests.Dequeue();
                            string[] splitlineOfRequest = request.Split(":");
                            string accountNum = splitlineOfRequest[0];
                            string userName = splitlineOfRequest[1];
                            string initialBalance = splitlineOfRequest[2];
                            string inialRequestStatus = splitlineOfRequest[3];


                            string requestInOneLine = accountNum + ":" + userName + ":" + initialBalance + ":" + inialRequestStatus;
                            writer.WriteLine(requestInOneLine);
                        }
                        //request = createAccountRequests.Dequeue();
                    }
                    Console.WriteLine("Requests saved successfully.");
                }
            }
            catch
            {
                Console.WriteLine("Error saving request file.");
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

                    }
                }
                Console.WriteLine("Requests loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading Request file.");
            }

        }
        // Additional Methods
        // get the last account number from the file
        static int GitTheLastAccountNumberFromAccountFile()
        {
            int lastAccNumber = 0;
            string lastLine = null;

            if (File.Exists(AccountsFilePath))
            {
                string[] lines = File.ReadAllLines(AccountsFilePath);

                // Find last non-empty line
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        lastLine = lines[i];
                        break;
                    }
                }

                if (lastLine != null)
                {
                    string[] parts = lastLine.Split(':');

                    if (parts.Length > 0 && int.TryParse(parts[0], out lastAccNumber))
                    {
                        return lastAccNumber;
                    }

                }
                else
                {
                    // if File is empty
                    return lastAccNumber = 0;
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }

            return 0;
        }

        // get the first request in the file and remove it from the file
        static string GitTheFirstRequestInFileAndDelete()
        {
            
                if (File.Exists(RequestsFilePath))
                {
                List<string> lines = File.ReadAllLines(RequestsFilePath).ToList(); //read all lines from file then store in list .

                if (lines.Count > 0) // check if the file is not empty
                {
                        string firstRequest = lines[0]; // get the first line

                        lines.RemoveAt(0); // remove the first line

                        File.WriteAllLines(RequestsFilePath, lines); // overwrite the file without first line

                        return firstRequest;
                    }
                    else
                    {
                        return "File is empty"; // file is empty
                    }
                }
                else
                {
                   
                    return "Requests file does not exist."; // file does not exist


            }

               


            
        }


        static int GetTheLastAccountNumberFromAccountFile()
        {
            if (!File.Exists(AccountsFilePath))
                return 0;

            string lastLine = File.ReadLines(AccountsFilePath).LastOrDefault();

            if (string.IsNullOrEmpty(lastLine))
                return 0;

            string[] parts = lastLine.Split(':');

            if (parts.Length > 0 && int.TryParse(parts[0], out int lastAcc))
                return lastAcc;

            return 0;
        }


        //static bool IsNationalIdExist(string nationalId)
        //{
        //    if (nationalIds.Contains(nationalId))
        //        return true;

        //    foreach (string request in createAccountRequests)
        //    {
        //        string[] parts = request.Split(':');
        //        if (parts.Length >= 2 && parts[1] == nationalId)
        //            return true;
        //    }

        //    return false;
        //}

        static string GetAccountName(string accountNumber)
        {
            string[] lines = File.ReadAllLines(AccountsFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length >= 2 && parts[0] == accountNumber)
                {
                    return parts[1];
                }
            }
            return null;
        }


    }
}
