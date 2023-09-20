using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class MeniController : Controller
    {
        readonly private IMeniService _meniService;
        readonly private IMapper _mapper;

        public MeniController(IMeniService meniService, IMapper mapper)
        {
            _meniService = meniService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMeni(DateTime date)
        {
            var menu = _meniService.GetByDate(date);
            if(menu == null)
            {
                menu = new Meni { Datum = date };
            }
            var meniViewModel = new MeniViewModel();
            MapMeniToMeniVM(menu, meniViewModel);
            return Ok(meniViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllMenis()
        {
            var menues = _meniService.GetAll().ToList();
            List<MeniForCalendarViewModel> viewModel = new List<MeniForCalendarViewModel>();
            foreach(var menu in menues)
            {
                viewModel.Add(new MeniForCalendarViewModel
                {
                    MeniId = menu.MeniId,
                    Datum = menu.Datum,
                    CanOrder = !menu.Locked
                });
            }
            //var viewModel = _mapper.Map<List<MeniForCalendarViewModel>>(menis);
            return Ok(viewModel);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Cook)]
        [HttpPost]
        public IActionResult CreateOrUpdate([FromBody] MeniViewModel viewModel)
        {
            //TODO Forbid update if any order exist
            IActionResult result;
            if (!ModelState.IsValid)
            {
                result = ValidationProblem("Greška! Molimo Vas pokušajte ponovo.");
            }
            else
            {
                var menu = new Meni();
                MapMenuVMToMenu(viewModel, menu);
                if (menu.Hrana.Count() < 1)
                {
                    result = ValidationProblem("Meni mora sadrzati hranu");
                }
                else
                {
                    menu = _meniService.CreateOrUpdate(menu);
                    result = Ok(menu.MeniId);
                }
            }
            return result;
        }


        #region Mappers

        private void MapMeniToMeniVM(Meni menu, MeniViewModel viewModel)
        {
            viewModel.MenuId = menu.MeniId;
            viewModel.Date = menu.Datum;
            viewModel.Food = new List<int>();
            viewModel.CanOrder = !menu.Locked;
            if (menu.Hrana != null)
            {
                foreach (var hrana in menu.Hrana)
                {
                    viewModel.Food.Add(hrana.HranaId);
                }
            }

        }

        private void MapMenuVMToMenu(MeniViewModel viewModel, Meni menu)
        {
            menu.MeniId = viewModel.MenuId;
            menu.Datum = viewModel.Date;
            var hranaMeni = new List<HranaMeni>();
            if (viewModel.Food != null)
            {
                foreach (var hrana in viewModel.Food)
                {
                    hranaMeni.Add(new HranaMeni { HranaId = hrana });
                }
            }
            menu.Hrana = hranaMeni;
        }
        #endregion
    }
}