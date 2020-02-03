using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        private Repository repository;
        Random random = new Random();

        /***
         * Controller Constructor
         */
        public SpinnerController(Repository r)
        {
            repository = r;
        }

        /***
         * Entry Page Action
         **/

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel info)
        {
            if (!ModelState.IsValid) { return View(); }

            //Set up Repository
            repository.CurrentPlayer = new Player
            {
                FirstName = info.FirstName,
                Luck = info.Luck,
                Balance = info.StartingBalance
            };
            repository.ClearSpins();
           
            return RedirectToAction("SpinIt");
        }

        /***
         * Game play through one Spin
         **/  
         [HttpGet]      
         public IActionResult SpinIt()
        {
            // Call constructor to create a new SpinItViewModel for this spin
            // Use the player information in the repository
            SpinItViewModel spinItVM = new SpinItViewModel() {
                FirstName = repository.CurrentPlayer.FirstName,
                Luck = repository.CurrentPlayer.Luck,
                Balance = repository.CurrentPlayer.Balance
            };

            //Check if enough balance to play, if not drop out to LuckList
            if (!spinItVM.ChargeSpin())
            {
                return RedirectToAction("LuckList");
            }
            // Check for Winnings
            if (spinItVM.Winner) { spinItVM.CollectWinnings(); }

            //Update Player Balance
            repository.CurrentPlayer.Balance = spinItVM.Balance;

            //Store the Spin in the Repository
            Spin spin = new Spin()
            {
                IsWinning = spinItVM.Winner
            };
            repository.AddSpin(spin);

            return View("SpinIt", spinItVM);
        }

        /***
         * ListSpins Action
         **/
         [HttpGet]
         public IActionResult LuckList()
        {
            return View(repository.PlayerSpins);
        }

    }
}

