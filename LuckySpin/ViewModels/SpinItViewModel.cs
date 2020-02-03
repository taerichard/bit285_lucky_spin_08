using System;
namespace LuckySpin.ViewModels
{
    public class SpinItViewModel
    {
        
        /*
         * Instance variables and constants
         */
        private const decimal costForOnePlay = 0.50m;
        private const decimal winningSpinValue = 1.00m;
        private System.Random random = new System.Random();

        private decimal _balance;
        private int _a, _b, _c;
        private int _luck;

        /*
         * Constructor
         */
        public SpinItViewModel()
        {
            _a = random.Next(1, 9);
            _b = random.Next(1, 9);
            _c = random.Next(1, 9);
        }

        /*
         * Simple Properties - used only to shuttle data, no instance variable
         */
        public string FirstName { get; set; }

        /*
         * Complex Properties - used in the game logic, connected with an instance variable
         */

        //Read-Write Properties
        public int Luck
        {
            get { return _luck; }
            set { _luck = value; }
        }
        public decimal Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
        //Read-Only Properties
        public int A
        {
            get { return _a; }
        }
        public int B
        {
            get { return _b; }
        }
        public int C
        {
            get { return _c; }
        }
        public bool Winner
        {
            get { return (_a == _luck || _b == _luck || _c == _luck); }
        }

        /*
         * Game Play Methods 
         */
        public bool ChargeSpin()
        {
            if (_balance >= costForOnePlay)
            {
                _balance -= costForOnePlay;
                return true;
            }
            return false;
        }
        public void CollectWinnings()
        {
            _balance += winningSpinValue;
        }
        
    }
}
