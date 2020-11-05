﻿using System;
using Microsoft.AspNetCore.Mvc;
using ProcessoSeletivo.Business.Handler;
using ProcessoSeletivo.Model;
using ProcessoSeletivo.Model.Interface;
using ProcessoSeletivo.Model.Operator;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcessoSeletivo.Controllers
{
    [Route("")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static EndpointHandler endpointHandler;
        public AccountController(IDao<Account> dao)
        {
            if (endpointHandler != null)
            {
                return;
            }
            endpointHandler = new EndpointHandler(dao);
        }

        [Route("Balance")]
        [HttpGet]
        public ActionResult<double> GetBalance(int account_id)
        {
            var account = endpointHandler.GetAccount(account_id);

            if (account != null)
            {
                return account.Balance;
            }

            return NotFound(0);
        }


        [Route("Reset")]
        [HttpPost]
        public string PostReset()
        {
            endpointHandler = null;
            return "Ok";
        }

        [Route("Event")]
        [HttpPost]
        public IActionResult PostAccountEvent([FromBody] AccountOperator account)
        {
            try
            {
                return CreatedAtAction(nameof(PostAccountEvent), endpointHandler.EventsHandler(account));
            }
            catch (Exception)
            {
                return NotFound(0);
            }
        }
    }
}
