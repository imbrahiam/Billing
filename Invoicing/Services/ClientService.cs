
using AutoMapper;
using Invoicing.DTOs;
using Invoicing.Models;
using Invoicing.Repository;

namespace Invoicing.Services
{
    public class ClientService : IClientService<ClientDTO, ClientInsertDTO, ClientUpdateDTO>
    {
        private IClientRepository<Client> _clientRepository;
        private IMapper _mapper;

        public ClientService(IClientRepository<Client> clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public List<string> Errors { get; }
        public async Task<IEnumerable<ClientDTO>> Get()
        {
            var clients = await _clientRepository.Get();

            return clients.Select(p => _mapper.Map<ClientDTO>(p));
        }

        public async Task<ClientDTO> GetByMat(string mat)
        {
            var client = await _clientRepository.GetByMat(mat);

            if (client != null)
            {
                var clientDTO = _mapper.Map<ClientDTO>(client);

                return clientDTO;
            }

            return null;
        }

        public async Task<ClientDTO> Add(ClientInsertDTO entityInsertDTO)
        {
            var client = _mapper.Map<Client>(entityInsertDTO);

            await _clientRepository.Add(client);
            await _clientRepository.Save();

            var clientDTO = _mapper.Map<ClientDTO>(client);
            return clientDTO;
        }

        public async Task<ClientDTO> Update(string mat, ClientUpdateDTO entityUpdateDTO)
        {
            var client = await _clientRepository.GetByMat(mat);

            if (client != null)
            {
                client = _mapper.Map<ClientUpdateDTO, Client>(entityUpdateDTO, client);
                _clientRepository.Update(client);
                await _clientRepository.Save();

                var clientDTO = _mapper.Map<ClientDTO>(client);
                return clientDTO;
            }

            return null;
        }

        public async Task<ClientDTO> Delete(string mat)
        {
            var client = await _clientRepository.GetByMat(mat);

            if (client != null)
            {
                var clientDTO = _mapper.Map<ClientDTO>(client);

                _clientRepository.Delete(client);
                await _clientRepository.Save();

                return clientDTO;
            }

            return null;
        }

        public bool Validate(ClientInsertDTO entityInsertDTO)
        {
            if (_clientRepository.Search(p => p.Name == entityInsertDTO.Name).Count() > 0)
            {
                Errors.Add("Repeated client name");
                return false;
            }

            return true;
        }

        public bool Validate(ClientUpdateDTO entityUpdateDTO)
        {
            if (_clientRepository.Search(p => p.Name == entityUpdateDTO.Name && p.ClientId != entityUpdateDTO.Id).Count() > 0)
            {
                Errors.Add("Client name already taken");

                return false;
            }

            return true;
        }
    }
}
