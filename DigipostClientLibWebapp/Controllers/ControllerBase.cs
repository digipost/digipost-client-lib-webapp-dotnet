using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DigipostService _digipostService;

        protected ControllerBase(DigipostService digipostService)
        {
            _digipostService = digipostService;
        }

        protected ControllerBase()
        {
            _digipostService = new DigipostService();
        }

        protected DigipostService GetDigipostService()
        {
            return _digipostService ?? new DigipostService();
        }
    }
}