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
        //static List<string> pandingRequestList = new List<string>();


        // Queues and Stacks
        static Queue<string> createAccountRequests = new Queue<string>();
        static Stack<string> reviewsStack = new Stack<string>();

        // Account number generator
        static int lastAccountNumber;
        // static int firstRequestInFile;

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
                    //Console.Clear();
                    Console.WriteLine("Welcome to Mini Bank System!");
                    Console.WriteLine("1. End User");
                    Console.WriteLine("2. Admin");
                    Console.WriteLine("0. Exit");

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
                string initialRequestStatus = "pending";
                string request = userName + ":" + nationalId + ":" + initialBalance + ":" + initialRequestStatus; ;
                createAccountRequests.Enqueue(request);



                //git the last account number from the file
                int lastAcc = GitTheLastAccountNumberFromAccountFile();
                Console.WriteLine("Your account request has been submitted. Please wait for approval.");
                Console.WriteLine("Your account number is (" + (lastAcc + 1) + ") .");

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
            Console.ReadLine();
        }

        //2. Deposit Money
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

                    // Read all accounts
                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5) // the line of file have 5 parts
                        {
                           
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;

                                Console.Write("Enter amount to deposit: ");
                                double amount = double.Parse(Console.ReadLine());

                                if (amount <= 0)
                                {
                                    Console.WriteLine("Amount must be greater than zero.");
                                    break;
                                }

                                double newBalance = balance + amount;
                                parts[3] = newBalance.ToString(); // Update balance part

                                // add the new balance to the file 
                                lines[i] = parts[0] + ":" + parts[1] + ":" + parts[2] + ":" + parts[3] + ":" + parts[4];

                                // Write back updated file
                                File.WriteAllLines(AccountsFilePath, lines);

                                Console.WriteLine($"Deposited {amount} successfully!");
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

        //3. Withdraw Money
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
                    int enteredAccountNumber = int.Parse(Console.ReadLine());
                    if (!File.Exists(AccountsFilePath))
                    {
                        Console.WriteLine("Accounts file not found.");
                        return;
                    }
                    // Read all accounts
                    List<string> lines = File.ReadAllLines(AccountsFilePath).ToList();
                    bool accountFound = false;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length >= 5) // the line of file have 5 parts
                        {
                            int fileAccountNumber = int.Parse(parts[0]);
                            string name = parts[1];
                            string nationalId = parts[2];
                            double balance = double.Parse(parts[3]);
                            string status = parts[4];

                            if (fileAccountNumber == enteredAccountNumber)
                            {
                                accountFound = true;
                                Console.Write("Enter amount to withdraw: ");
                                double amount = double.Parse(Console.ReadLine());
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
                                parts[3] = newBalance.ToString(); // Update balance part

                                // add the new balance to the file 
                                lines[i] = parts[0] + ":" + parts[1] + ":" + parts[2] + ":" + parts[3] + ":" + parts[4];
                                // Write back updated file
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
                string request = createAccountRequests.Peek();

                string[] splitlineOfRequest = request.Split(":");
                string userName = splitlineOfRequest[0];
                string nationalId = splitlineOfRequest[1];
                string initialBalance = splitlineOfRequest[2];
                string inialRequestStatus = splitlineOfRequest[3];


                Console.WriteLine("view account Request");
                Console.WriteLine("user name : " + userName);
                Console.WriteLine("national Id : " + nationalId);
                Console.WriteLine("initial balance : " + initialBalance);
                Console.WriteLine("request status : " + inialRequestStatus);



                request = createAccountRequests.Dequeue(); // remove from memory
                Console.WriteLine("Do you want to approve this request? (y/n)");
                string requestStatus = Console.ReadLine();
                if (requestStatus.ToLower() == "y")
                {

                    // Add the account to the system

                    int accountNumber = GitTheLastAccountNumberFromAccountFile() + 1;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Approved");
                    Console.WriteLine("Account Request Approved");

                    //delete the request from the file
                    
                    string requestToRemove = request; // remove from memory
                    string first = GitTheFirstRequestInFileAndDelete().ToString(); // remove from file






                    Console.WriteLine($"Account for {userName} has been created with account number {accountNumber}.");
                }
                else
                {

                    // Add the account to the system
                    
                    int accountNumber = GitTheLastAccountNumberFromAccountFile() + 1;
                    double initialBalanceDouble = double.Parse(initialBalance);
                    //accountNumbers.Add(accountNumber);
                    accountNames.Add(userName);
                    nationalIds.Add(nationalId);
                    balances.Add(initialBalanceDouble);
                    requestStatuse.Add("Not Approved");
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
                Console.WriteLine("\n--- All Accounts ---");
                // vie all account requests from the file
                Console.WriteLine("--------------------------------------------------");

                foreach (string request in createAccountRequests)
                {
                    string[] splitlineOfRequest = request.Split(":");
                    string accountNum = splitlineOfRequest[0];
                    string userName = splitlineOfRequest[1];
                    string initialBalance = splitlineOfRequest[2];
                    string inialRequestStatus = splitlineOfRequest[3];
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("accountNum: " + accountNum);
                    Console.WriteLine("userName: " + userName);
                    Console.WriteLine("initialBalance: " + initialBalance);
                    Console.WriteLine("inialRequestStatus: " + inialRequestStatus);
                }
                //    for (int i = 0; i < accountNumbers.Count; i++)
                //{
                //    //Console.WriteLine($"{accountNumbers[i]} - {accountNames[i]} - Balance: {balances[i]} - ");
                //    Console.WriteLine("--------------------------------------------------");
                //    Console.WriteLine("Account Number: " + accountNumbers[i]);
                //    Console.WriteLine("Account Name: " + accountNames[i]);
                //    Console.WriteLine("Account Balance: " + balances[i]);
                //    Console.WriteLine("Account Status: " + requestStatuse[i]);
                //    Console.WriteLine("--------------------------------------------------");
                //}
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
                using (StreamWriter writer = new StreamWriter(AccountsFilePath, true))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string dataLine = accountNumbers[i] + ":" + accountNames[i] + ":" + nationalIds[i] + ":" + balances[i] + ":" + requestStatuse[i];
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

        static int GitAccountNumberFromFile()
        {
            int accountNum = 0;
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
                    if (parts.Length > 0 && int.TryParse(parts[0], out accountNum))
                    {
                        return accountNum;
                    }
                }
                else
                {
                    // if File is empty
                    return accountNum = 0;
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
            return 0;
        }

        //static int GitAccountNumberIndexFromFile()
        //{

        //    //git the account number from the file
        //    int accountNumber = GitAccountNumberFromFile();
        //    if (File.Exists(AccountsFilePath))
        //    {
        //        string[] lines = File.ReadAllLines(AccountsFilePath);
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            string[] parts = lines[i].Split(':');
        //            if (parts.Length > 0 && int.TryParse(parts[0], out int accountNum))
        //            {
        //                if (accountNum == accountNumber)
        //                {
        //                    return accountNumber;
        //                }
        //            }
        //        }
        //    }


        //}
    }
}
