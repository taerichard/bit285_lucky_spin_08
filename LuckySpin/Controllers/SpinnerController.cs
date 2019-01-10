using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;

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
        public IActionResult Index(Player player)
        {
            if (ModelState.IsValid) {
                //Save the current player in the repository
                repository.CurrentPlayer = player;
                repository.CurrentPlayer.AddCredit(player.StartingBalance);
                return RedirectToAction("SpinIt");
            }

            return View();
        }

        /***
         * Spin Action
         **/  
         [HttpGet]      
         public IActionResult SpinIt() //Remove input, use the repository
        {
            //Check if enough balance to play, if not drop out to LuckList
            if (!repository.CurrentPlayer.ChargeSpin())
            {
                return RedirectToAction("LuckList");
            }

            //Create the current Spin
            Spin spin = new Spin
            {
                Luck = repository.CurrentPlayer.Luck,
                A = random.Next(1, 10),
                B = random.Next(1, 10),
                C = random.Next(1, 10)
            };
            spin.IsWinning = (spin.A == spin.Luck || spin.B == spin.Luck || spin.C == spin.Luck);

            //Add to Spin Repository
            repository.AddSpin(spin);

            //Prepare the View
            if (spin.IsWinning)
            {
                ViewBag.Display = "block";
                repository.CurrentPlayer.CollectWinnings();
            }
            else
                ViewBag.Display = "none";

            ViewBag.FirstName = repository.CurrentPlayer.FirstName;
            ViewBag.Balance = repository.CurrentPlayer.Balance;

            return View("SpinIt", spin);
        }

        /***
         * ListSpins Action
         **/

         public IActionResult LuckList()
        {
            ViewBag.Balance = 0;
            return View(repository.PlayerSpins);
        }

    }
}

