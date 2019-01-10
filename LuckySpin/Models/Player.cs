using System;
using System.ComponentModel.DataAnnotations;
namespace LuckySpin.Models
{
    public class Player
    {
        private Decimal _balance = 0;

        [Required(ErrorMessage ="Please enter your Name")]
        public string FirstName { get; set; }
        [Range(1,9, ErrorMessage = "Choose a number")]
        public int Luck { get; set; }
        [Display(Prompt = "Starting Balance in $$")]
        [DataType(DataType.Currency)]
        [Range(3.0, 10.0, ErrorMessage = "Amount must be between $3 and $10.")]
        public Decimal StartingBalance { get; set; }

        public Decimal Balance
        {
            get { return _balance; }
        }
        public void AddCredit(Decimal b)
        {
            _balance += b;
        }
        public bool ChargeSpin()
        {
            if (_balance >= 0.50m)
            {
                _balance -= 0.50m;
                return true;
            }
            return false;
        }
        public void CollectWinnings()
        {
            _balance += 1.00m;
        }
    }
}