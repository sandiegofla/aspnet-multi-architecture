using System.Collections.Generic;
using WCFServiceHost.Model;
using WCFServiceHost.DAL;
using WCFServiceHost.Services;

namespace WebForm.Services
{
    public class ClienteService : IClienteService
    {
        public bool CadastrarCliente(Cliente cliente, EnderecoCliente endereco)
        {
            return new ClienteDAL().Inserir(cliente, endereco);
        }

        public bool EditarCliente(Cliente cliente, EnderecoCliente endereco)
        {
            return new ClienteDAL().Editar(cliente, endereco);
        }

        public bool ExcluirCliente(int clienteId)
        {
            return new ClienteDAL().Excluir(clienteId);
        }

        public List<Cliente> ListarClientes()
        {
            return new ClienteDAL().Listar();
        }
    }
}
