using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Assignment2_Deliverable2_n10153900_YuiAndoh
{
    class Program
    {
        static void Main(string[] args)
        {
            int NumberOfAllSeats = 40;
            int ErrorCounter;     // Counts the number of errors to limit the trial to three times
            int MaxTrial = 3;     // Number of trials users can make at once
            int Continue;         // Switch case label for the choice of a passenger/airline/exit
            int Options;          // Switch case label for airline options
            bool CheckUserInput;

            

            // Create an array  whose length is 40
            Passenger NewPassenger = new Passenger(NumberOfAllSeats);

            do
            {
                ErrorCounter = 0; // Initialize and reset the counter

                WriteLine("WELCOME TO ABC AIRLINES BOARDING PASS SYSTEM!");
                WriteLine();
                WriteLine("Are you a passenger or an airline assistant?");
                WriteLine("1. Passenger");
                WriteLine("2. Airline Staff");
                WriteLine("3. Exit");
                Write(">> ");

                CheckUserInput = int.TryParse(ReadLine(), out Continue);


                // Switch sections for the choice of a passenger/airline/exit
                switch (Continue)
                {
                    // For passengers
                    case 1:
                        Clear();
                        IssueBoardingPass(NewPassenger, NumberOfAllSeats);
                        break;

                    // For airline staffs
                    case 2:
                        Clear();
                        do
                        {
                            WriteLine("The program goes back to the main menu if your input is invalid for three times.");
                            WriteLine("What do you want to do?");
                            WriteLine();
                            WriteLine("1. Issue a boarding pass");
                            WriteLine("2. See available seats");
                            WriteLine("3. See a list of passengers");
                            WriteLine("4. Go back to the main menu");
                            Write(">> ");

                            CheckUserInput = int.TryParse(ReadLine(), out Options);
                            Clear();


                            // Switch sections for airline options
                            switch (Options)
                            {
                                // Issue boarding passes
                                case 1:
                                    IssueBoardingPass(NewPassenger, NumberOfAllSeats);
                                    break;

                                // View a map of available seats
                                case 2:
                                    NewPassenger.SeatMapDisplay(NumberOfAllSeats);
                                    Write("Press Enter to go back to the main menu...");
                                    ReadKey();
                                    Clear();
                                    break;

                                // View the list of boarding passengers
                                case 3:
                                    NewPassenger.ListDisplay(NumberOfAllSeats);
                                    Write("Press Enter to go back to the main menu...");
                                    ReadKey();
                                    Clear();
                                    break;

                                // Go back to the main menu
                                case 4:
                                    break;

                                // Displays an error message and asks another input
                                default:
                                    ErrorCounter++;
                                    WriteLine("Your input is invalid. Please try again.");
                                    WriteLine();
                                    break;
                            }
                        } while (ErrorCounter < MaxTrial && Options != 1 && Options != 2 && Options != 3 && Options != 4);
                        // Automatically goes back to the main menu if user input invalid value for three times

                        break;

                    // Terminate the program
                    case 3:
                        break;

                    // Displays an error message and asks another input
                    default:
                        Clear();
                        WriteLine("Your input is invalid. Please try again.");
                        WriteLine();
                        break;
                }

            } while (Continue != 3); // Terminates when user input is 3 (Exit)

        }



        // Method to issue a boarding pass
        private static void IssueBoardingPass(Passenger NewPassenger, int NumberOfAllSeats)
        {
            string Flight = "QF61";  // Flight number
            string Depart = "BNE";   // Departure
            string Arrive = "TYO";   // Arrival
            int Gate = 83;           // Departure gate
            DateTime DepTime = new DateTime(2019, 6, 2, 9, 35, 00); // Departure time

            bool CheckUserInput;
            bool Available = false; // Checks the availability of the selected seat



            Write("How many passengers are traveling? >> ");
            CheckUserInput = int.TryParse(ReadLine(), out int NumberOfPassengers);

            NewPassenger.CheckUserInput(ref CheckUserInput, ref NumberOfPassengers, NumberOfAllSeats);

            int SeatsNeeded = NumberOfPassengers;
            // SeatNeeded is the number of seats that are required for passengers in one invoice.
            // It decrements one by one in every iteration when the user input one passenger information.


            for (int i = 0; i < NumberOfPassengers; i++)
            {
                NewPassenger.SeatMapDisplay(NumberOfAllSeats);
                NewPassenger.NumberOfAvailableSeats(out int AvailableSeats, NumberOfAllSeats);

                if (AvailableSeats >= SeatsNeeded)
                {
                    CheckUserInput = int.TryParse(Passenger.GetInput("Please select a seat >> "), out int PreferredSeat);

                    do
                    {
                        NewPassenger.CheckUserInput(ref CheckUserInput, ref PreferredSeat, NumberOfAllSeats);
                        NewPassenger.CheckAvailability(ref PreferredSeat, ref Available, ref CheckUserInput);
                    }
                    while (CheckUserInput == false || Available == false || PreferredSeat > NumberOfAllSeats);

                    // Ask passenger's first name and show an error message if the user input nothing
                    do
                    {
                        NewPassenger.FName[PreferredSeat - 1] = Passenger.GetName("first name");
                        if (NewPassenger.FName[PreferredSeat - 1].Length == 0)
                            WriteLine("Your input is invalid. Please try again.");
                    }
                    while (NewPassenger.FName[PreferredSeat - 1].Length == 0);

                    // Ask passenger's last name and show an error message if the user input nothing
                    do
                    {
                        NewPassenger.LName[PreferredSeat - 1] = Passenger.GetName("last name");
                        if (NewPassenger.LName[PreferredSeat - 1].Length == 0)
                            WriteLine("Your input is invalid. Please try again.");
                    }
                    while (NewPassenger.LName[PreferredSeat - 1].Length == 0);

                    // Assign a security number
                    NewPassenger.SecurityNumber[PreferredSeat - 1] = NewPassenger.GenerateSN(out NewPassenger.SecurityNumber[PreferredSeat - 1]);

                    // Get the time when the boarding pass is issued
                    NewPassenger.IssuingTime[PreferredSeat - 1] = NewPassenger.GetCurrentTime(out NewPassenger.IssuingTime[PreferredSeat - 1]);
                    WriteLine();

                    NewPassenger.TicketDisplay(Flight, Depart, Arrive, Gate, DepTime, PreferredSeat);

                    SeatsNeeded--; // Decrements as the user took one seat above

                    // Shows different messages depending on the number of passengers in one invoice
                    if (i == NumberOfPassengers - 1)
                        WriteLine("Press Enter to go back to the main menu...");
                    else
                        WriteLine("Press Enter to continue to the next passenger...");

                    ReadKey(); // Waits for user input to show the issued boarding pass
                }

                else
                {
                    WriteLine("I'm sorry, but the number of available seats is currently {0}. Please call for an assistance.", AvailableSeats);
                    i = NumberOfPassengers; // Immediately breaks the for loop and goes back to main menu since there are not enough seats for user input
                    ReadKey();
                }
                    
                Clear();
            }
        }
    }
}
