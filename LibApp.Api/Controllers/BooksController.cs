﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Books;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]   
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new Details.Query{Id = id}));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Add.Command command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/rent")]
        public async Task<IActionResult> Rent(int id, Rent.Command command)
        {
            return Ok(await _mediator.Send(new Rent.Command() { BookId = id, UserName = command.UserName })) ;;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id, Rent.Command command)
        {
            return Ok(await _mediator.Send(new Return.Command() { BookId = id, UserName = command.UserName }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBook(int id, Update.Command command)
        {
            command.BookId = id;
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/newCopy")]
        public async Task<ActionResult> NewCopy(int id, AddCopy.Command command)
        {
            command.BookId = id;
            return Ok(await _mediator.Send(command));
        }
    }
}