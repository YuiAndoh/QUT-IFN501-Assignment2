using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Assignment2_Deliverable2_n10153900_YuiAndoh
{
    
    class Passenger
    {
        private string[] _Firstname;
        private string[] _Lastname;
        private int[] _SeatNumber;
        private int[] _SecurityNumber;
        private DateTime[] _IssuingTime;
        readonly int SeatsInOneRow = 4;
        


        // Arrays of all attributes of passengers
        public Passenger(int NumberOfAllSeats)
        {
            _Firstname = new string[NumberOfAllSeats];
            _Lastname = new string[NumberOfAllSeats];
            _SeatNumber = new int[NumberOfAllSeats];
            _SecurityNumber = new int[NumberOfAllSeats];
            _IssuingTime = new DateTime[NumberOfAllSeats];
        }



        // Properties
        public string[] FName
        {
            get { return _Firstname; }
            set { _Firstname = value; }
        }

        public string[] LName
        {
            get { return _Lastname; }
            set { _Lastname = value; }
        }

        public int[] SeatNumber
        {
            get { return _SeatNumber; }
            set { _SeatNumber = value; }
        }

        public int[] SecurityNumber
        {
            get { return _SecurityNumber; }
            set { _SecurityNumber = value; }
        }

        public DateTime[] IssuingTime
        {
            get { return _IssuingTime; }
            set { _IssuingTime = value; }
        }



        // Methods

        // Get the time of boarding pass issue
        public DateTime GetCurrentTime(out DateTime CurrentTime)
        {
            CurrentTime = DateTime.Now;
            return CurrentTime;
        }


        // Generate a random number as a security number
        public int GenerateSN(out int SN)
        {
            Random Randomium = new Random();
            SN = Randomium.Next(30001, 999999); // The mininum is inclusive, the maximum is exclusive
            return SN;
        }


        // Prompt a user for name input
        public static string GetName(string what)
        {
            Write("Please enter the {0} of passenger >> ", what);
            string outPut = ReadLine();

            if (outPut.Length > 5)
                outPut = outPut.Remove(5); // Trims the name into first five characters if the name length is more than five

            return outPut;
        }


        // Prompt a user for seat number input
        public static string GetInput(string sentence)
        {
            Write("{0}", sentence);
            string outPut = ReadLine();
            return outPut;
        }


        // Displays a seat map that shows available seats
        public void SeatMapDisplay(int NumberOfAllSeats)
        {
            int NumberOfRow = NumberOfAllSeats / SeatsInOneRow;
            if (NumberOfAllSeats % SeatsInOneRow > 0)
                NumberOfRow++;

            WriteLine();
            WriteLine("-------- SEAT MAP --------");
            WriteLine("           Front");
            for (int i = 0; i < NumberOfRow; i++)
            {
                AvailabilityDisplay(i*SeatsInOneRow + 1, NumberOfAllSeats);
            }
            WriteLine("           Back");
            WriteLine();
        }


        // Displays a seat row with the availability of each seat
        public void AvailabilityDisplay(int j, int NumberOfAllSeats)
        {
            for (int i = j; i < j + SeatsInOneRow; ++i)
            {
                // Shows N/A when the seat is taken or when the number of all seats are less than the number of seats displayed on the seat map
                if (_Firstname[i - 1] != null || (NumberOfAllSeats < j + SeatsInOneRow && i > NumberOfAllSeats))
                    Write("  [NA]");

                // Shows seat numbers when they are available
                else
                    Write("  [{0}]", i.ToString("00"));
            }
            WriteLine();
        }


        // Counts and shows the number of available seats
        public int NumberOfAvailableSeats(out int AvailableSeats, int NumberOfAllSeats)
        {
            AvailableSeats = 0;
            for (int i = 0; i < NumberOfAllSeats; i++)
            {
                // AvailableSeats increments when the array of the first name doesn't have a value in index i
                if (_Firstname[i] == null)
                    AvailableSeats++;
            }
            return AvailableSeats;
        }


        // Checks user input for seat number (invalid input)
        public int CheckUserInput(ref bool CheckUserInput, ref int PreferredSeat, int NumberOfAllSeats)
        {
            // Shows an error message when the user input non-number items, a number greater than total seats and negative numbers
            while (CheckUserInput == false || PreferredSeat > NumberOfAllSeats || PreferredSeat <= 0)
                CheckUserInput = int.TryParse(GetInput("Your input is invalid. Please try again >> "), out PreferredSeat);

            return PreferredSeat;
        }


        // Checks user input for seat number (seat availability)
        public int CheckAvailability(ref int PreferredSeat, ref bool Available, ref bool CheckUserInput)
        {
            Available = false;

            if (_Firstname[PreferredSeat - 1] == null)
                Available = true;
            else
                CheckUserInput = int.TryParse(GetInput("I'm sorry, but the seat is already taken. Please try again >> "), out PreferredSeat);

            return PreferredSeat;
        }


        // Displays a flight ticket in a tableau format
        public void TicketDisplay(string Flight, string Depart, string Arrive, int Gate, DateTime DepTime, int PreferredSeat)
        {
            WriteLine();
            WriteLine("-------------------------------- BOARDING PASS --------------------------------");
            WriteLine("    ABC Airlines                                                      {0}", _SecurityNumber[PreferredSeat - 1]);
            WriteLine();
            WriteLine("       Flight: {0}                Seat: {1}", Flight, PreferredSeat);
            WriteLine("       Depart: {0}                 Gate: {1}", Depart, Gate);
            WriteLine("       Arrive: {0}                 Departure Time: {1}", Arrive, DepTime.ToShortTimeString());
            WriteLine("         {0} / {1}", _Firstname[PreferredSeat - 1], _Lastname[PreferredSeat - 1]);
            WriteLine("                                          Date of Issue: {0}", _IssuingTime[PreferredSeat - 1]);
            WriteLine(" Please be at the boarding gate at least 30 minutes before your departure time."); 
            WriteLine("--------------------------------------------------------------------------------");
            WriteLine();
        }


        // Displays a list of passengers who issued boarding passes
        public void ListDisplay(int NumberOfAllSeats)
        {
            for (int i = 0; i < NumberOfAllSeats; i++)
            {
                if (_Firstname[i] != null)
                {
                    Write("Seat #{0}", (i + 1) + " : " + _Firstname[i] + " " + _Lastname[i]);
                    Write("     Security #{0}", _SecurityNumber[i].ToString());
                    WriteLine("     Issued at {0}", _IssuingTime[i].ToString("HH:mm:ss"));
                }   
            }
            WriteLine();
        }                
    }
}
