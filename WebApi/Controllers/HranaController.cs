using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebApi.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Authorize]
    //[ApiController]
    public class HranaController : Controller
    {
        readonly IHranaService _hranaService;
        readonly IFileService _fileService;
        readonly private IMapper _mapper;
        const int MAX_IMAGE_SIZE = 2000000;
        readonly string[] IMAGE_FORMATS = { "jpeg", "png", "gif", "bmp" };

        public HranaController(IHranaService hranaService, IMapper mapper, IFileService fileService)
        {
            _hranaService = hranaService;
            _mapper = mapper;
            _fileService = fileService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var food = _hranaService.GetAll().ToList();
            var viewModel = food.Select(o => new HranaViewModel
            {
                HranaId = o.HranaId,
                Stalna = o.Stalna,
                Naziv = o.Naziv,
                Prilozi = o.Prilozi.Select(o1 => new PrilogViewModel
                {
                    PrilogId = o1.PrilogId,
                    Varijanta = o1.Varijanta
                }).ToList(),
                Rating = o.Ocjene.Count() > 0 ? o.Ocjene.Select(o1 => o1.Vrijednost).Average() : 0,
                NumberOfComments = o.Komentari.Count(),
                Image = o.Image
            });
            return Ok(viewModel);
        }


        [HttpGet]
        public IActionResult GetAllSideDishes()
        {
            var prilozi = _hranaService.GetAllSideDishes().ToList();
            var viewModel = _mapper.Map<List<PrilogViewModel>>(prilozi);
            return Ok(viewModel);
        }

        [HttpPost]
        public IActionResult Rate([FromBody] RateViewModel viewModel)
        {
            IActionResult result;
            if (!ModelState.IsValid)
            {
                result = ValidationProblem("Nevalidan zahtjev.");
            }
            else
            {
                _hranaService.SetRate(Convert.ToInt32(User.Identity.Name), viewModel.FoodId, viewModel.Mark);
                result = Ok();
            }

            return result;
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Cook)]
        [HttpPost]
        public IActionResult CreateOrUpdate([FromBody] HranaViewModel viewModel)
        {
            IActionResult result;
            if (!ModelState.IsValid)
            {
                result = ValidationProblem("Naziv ne moze biti prazan.");
            }
            else
            {
                //var hrana = _mapper.Map<Hrana>(viewModel);
                var food = new Hrana();
                MapFoodVMToFood(viewModel, food);
                var foodId = _hranaService.CreateOrUpdate(food).HranaId;
                result = Ok(foodId);
            }

            return result;
        }

        [HttpPost]//DisableRequestSizeLimit
        public async Task<IActionResult> SetComment([FromForm] CommentViewModel viewModel)
        {
            IActionResult result = null;
            if (!ModelState.IsValid)
            {
                result = ValidationProblem("Nevalidan zahtjev.");
            }
            else
            {
                var imageName = "";
                if (viewModel.ImageBase64 != null)
                {
                    if (viewModel.ImageBase64.Length > MAX_IMAGE_SIZE)
                    {
                        result = ValidationProblem("Prevelika slika.");
                    }
                    else
                    {
                        var imageParts = viewModel.ImageBase64.Split(',');
                        var format = imageParts[0].Replace("data:image/", "").Replace(";base64", "");
                        if (!IMAGE_FORMATS.Contains(format))
                        {
                            result = ValidationProblem("Format slike nije podržan.");
                        }
                        else
                        {
                            imageName = $"{Path.GetRandomFileName()}.{format}";
                            await _fileService.SaveImage(imageParts[1], imageName);
                        }
                    }

                }

                _hranaService.SetComment(Convert.ToInt32(User.Identity.Name), viewModel.FoodId, viewModel.Content, imageName);

                if (result == null)
                {
                    result = Ok();
                }
            }

            return result;
        }

        [HttpGet]
        public IActionResult GetComments(int foodId)
        {
            var comments = _hranaService.GetComments(foodId);
            var viewModel = comments.Select(o => MapCommentToCommentVM(o)).ToList();
            return Ok(viewModel);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Cook)]
        [HttpPost]
        public IActionResult CreateSideDish([FromBody] PrilogViewModel viewModel)
        {
            IActionResult result;
            if (!ModelState.IsValid)
            {
                result = ValidationProblem("Naziv ne moze biti prazan.");
            }
            else
            {
                var sideDishId = _hranaService.CreateSideDish(viewModel.Naziv);
                result = Ok(sideDishId);
            }
            return result;
        }

        #region Mappers

        private CommentViewModel MapCommentToCommentVM(Komentar comment)
        {
            var commentVM = new CommentViewModel();
            MapCommentToCommentVM(comment, commentVM);
            return commentVM;
        }

        private void MapCommentToCommentVM(Komentar comment, CommentViewModel viewModel)
        {
            viewModel.User = comment.User.Email;
            viewModel.Time = comment.Time;
            viewModel.Content = comment.Comment;
            viewModel.Image = comment.Slika;

        }

        private void MapFoodVMToFood(HranaViewModel viewModel, Hrana food)
        {
            food.Naziv = viewModel.Naziv;
            food.Stalna = viewModel.Stalna;
            food.HranaId = viewModel.HranaId;

            food.Prilozi = viewModel.Prilozi.Select(o => new HranaPrilog
            {
                PrilogId = o.PrilogId,
                Varijanta = o.Varijanta
            }).ToList();

        }
        


        #endregion
    }
}