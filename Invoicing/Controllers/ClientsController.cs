using FluentValidation;
using Invoicing.DTOs;
using Invoicing.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        IClientService<ClientDTO, ClientInsertDTO, ClientUpdateDTO> _clientService;
        IValidator<ClientInsertDTO> _clientInsertValidator;

        // Greedy aqui
        IValidator<ClientUpdateDTO> _clientUpdateValidator; 
        public ClientsController([FromKeyedServices("ClientService")] IClientService<ClientDTO, ClientInsertDTO, ClientUpdateDTO> clientService,
            IValidator<ClientInsertDTO> clientInsertValidator,
            IValidator<ClientUpdateDTO> clientUpdateValidator)
        {
            _clientService = clientService;
            _clientInsertValidator = clientInsertValidator;
            _clientUpdateValidator = clientUpdateValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<ClientDTO>> Get() => await _clientService.Get();

        [HttpGet("{mat}")]
        public async Task<ActionResult<ClientDTO>> GetByMat(string mat)
        {
            var clientDTO = await _clientService.GetByMat(mat);
            return clientDTO == null ? NotFound() : Ok(clientDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDTO>> Add(ClientInsertDTO clientInsertDTO)
        {
            var validationResult = await _clientInsertValidator.ValidateAsync(clientInsertDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_clientService.Validate(clientInsertDTO))
            {
                return BadRequest(_clientService.Errors);
            }

            var clientDTO = await _clientService.Add(clientInsertDTO);

            return CreatedAtAction(nameof(GetByMat), new { mat = clientDTO.Mat }, clientDTO);
        }

        [HttpPut("{mat}")]
        public async Task<ActionResult<ClientDTO>> Update(string mat, ClientUpdateDTO clientUpdateDTO)
        {
            var validationResult = await _clientUpdateValidator.ValidateAsync(clientUpdateDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_clientService.Validate(clientUpdateDTO))
            {
                return BadRequest(_clientService.Errors);
            }

            var clientDTO = await _clientService.Update(mat, clientUpdateDTO);

            return clientDTO == null ? NotFound() : Ok(clientDTO);
        }

        [HttpDelete("{mat}")]
        public async Task<ActionResult<ClientDTO>> Delete(string mat)
        {
            var clientDTO = await _clientService.Delete(mat);

            return clientDTO == null ? NotFound() : Ok(clientDTO);
        }
    }
}
