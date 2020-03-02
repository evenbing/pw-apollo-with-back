using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.DataTransferObjects.Transactions;
using PW.Entities;
using PW.Services.Exceptions;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;        

        public TransactionsController(ITransactionService transactionService,
            IMapper mapper)
        {            
            _transactionService = transactionService;            
            _mapper = mapper;
        }
                
        [HttpGet]
        public async Task<ActionResult<TransactionDto>> GetAllForCurrentUser()
        {
            var email = HttpContext.User.Identity.Name;
            var transactions = await _transactionService.GetTransactionsOrderedByDateAsync(email);
            var result = transactions.Select(x => _mapper.Map<TransactionDto>(x));

            return Ok(result);
        }
                        
        [HttpPost]
        public async Task<ActionResult> Create([FromBody]CreateTransactionDto createTransactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payeeEmail = HttpContext.User.Identity.Name;            
            try
            {
                await _transactionService.CreateTransactionAsync(payeeEmail, createTransactionDto);
            }
            catch (PWException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }           

            return Ok();
        }
    }
}
