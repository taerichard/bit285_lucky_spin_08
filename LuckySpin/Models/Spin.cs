using System;
using System.ComponentModel.DataAnnotations;
namespace LuckySpin.Models
{
    public class Spin
    {
        public long Id { get; set; }
        public Boolean IsWinning { get; set; }
    }
}
