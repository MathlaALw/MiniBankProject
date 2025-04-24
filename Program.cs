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
           
        }

        //2. Deposit Money
        static void DepositMoney()
        {
            

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
