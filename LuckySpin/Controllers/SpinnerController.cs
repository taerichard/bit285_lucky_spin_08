using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        private LuckySpinContext _dbc;
        /***
         * Controller Constructor
         */
        public SpinnerController(LuckySpinContext luckySpinContext)
        {
            _dbc = luckySpinContext;
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

            //Save Player in Repository
            Player player = new Player
            {
                FirstName = info.FirstName,
                Luck = info.Luck,
                Balance = info.StartingBalance
            };
            _dbc.Players.Add(player);
            _dbc.SaveChanges();
           
            return RedirectToAction("SpinIt", new { id = player.Id } );
        }

        /***
         * Game play through one Spin
         **/  
         [HttpGet]      
         public IActionResult SpinIt(long id)
        {
            //** Gets the Player belonging to the given id
            //TODO: Modify the code to use the SingleOrDefault Lamda Extension method
            //TODO: Then Include the Players Spins collection

            Player player = _dbc.Players
                .Include(p => p.Spins)
                .SingleOrDefault(x => x.Id == id);

            // Populates a new SpinItViewModel for this spin
            // using the player information
            SpinItViewModel spinItVM = new SpinItViewModel() {
                FirstName = player.FirstName,
                Luck = player.Luck,
                Balance = player.Balance
            };

            //Checks if enough balance to play, if not drop out to LuckList
            if (!spinItVM.ChargeSpin())
            {
                return RedirectToAction("LuckList", new { id = player.Id });
            }
            // Checks for Winnings
            if (spinItVM.Winner) { spinItVM.CollectWinnings(); }

            //** Updates Player Balance
            player.Balance = spinItVM.Balance;
        
            Spin spin = new Spin()
            {
                IsWinning = spinItVM.Winner
            };
            //** Adds the Spin to the Database Context
            //TODO: Modify the next line to use the player's Spins collection instead
            _dbc.Spins.Add(spin);
            //**** Saves all the changes to the Database at once
            _dbc.SaveChanges();

            return View("SpinIt", spinItVM);
        }

        /***
         * ListSpins Action
         **/
         [HttpGet]
         public IActionResult LuckList(long id)
        {
            //Gets the Player belonging to the given id
            //TODO: Modify the code to use the SingleOrDefault Lamda Extension method
            //TODO: Then Include the Players Spins collection
            Player player = _dbc.Players.Find(id);
            //Gets the list of Spins from the Context
            //TODO: Modify the next line to get the list of the player's Spins instead of all the Spins
            IEnumerable<Spin> spins = _dbc.Spins;
            // Hack in some detail about the player
            ViewBag.Player = player;

            return View(spins);
        }

    }
}

