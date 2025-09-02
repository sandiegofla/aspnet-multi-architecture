using System.Collections.Generic;
using System.ServiceModel;
using WCFServiceHost.Model;

namespace WCFServiceHost.Services
{
    [ServiceContract]
    public interface IClienteService
    {
        [OperationContract]
        bool CadastrarCliente(Cliente cliente, EnderecoCliente endereco);

        [OperationContract]
        bool EditarCliente(Cliente cliente, EnderecoCliente endereco);

        [OperationContract]
        bool ExcluirCliente(int clienteId);

        [OperationContract]
        List<Cliente> ListarClientes();
    }
}
