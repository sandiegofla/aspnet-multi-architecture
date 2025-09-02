using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly string _connectionString = @"Data Source=C:\Users\Sandi\Desktop\Projeto GTI Solution\ModeloProjeto (1)\ModeloProjeto\BancoDeDados\bd_ProjetoGTIsolution.db";

        [HttpGet]
        public ActionResult<List<ClienteEndereco>> Get()
        {
            var clientes = new List<ClienteEndereco>();

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT c.Id, c.Nome, c.CPF, c.RG, c.DataExpedicao, c.OrgaoExpedicao, c.UFExpedicao, 
                                        c.DataNascimento, c.Sexo, c.EstadoCivil, 
                                        e.CEP, e.Logradouro, e.Numero, e.Complemento, e.Bairro, e.Cidade, e.UF
                                 FROM Cliente c
                                 INNER JOIN EnderecoCliente e ON c.Id = e.ClienteId";

                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new ClienteEndereco
                        {
                            Cliente = new Cliente
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                CPF = reader.GetString(2),
                                RG = reader.GetString(3),
                                DataExpedicao = reader.GetString(4),
                                OrgaoExpedicao = reader.GetString(5),
                                UFExpedicao = reader.GetString(6),
                                DataNascimento = reader.GetString(7),
                                Sexo = reader.GetString(8),
                                EstadoCivil = reader.GetString(9),
                            },
                            Endereco = new EnderecoCliente
                            {
                                CEP = reader.GetString(10),
                                Logradouro = reader.GetString(11),
                                Numero = reader.GetString(12),
                                Complemento = reader.GetString(13),
                                Bairro = reader.GetString(14),
                                Cidade = reader.GetString(15),
                                UF = reader.GetString(16)
                            }
                        });
                    }
                }
            }

            return Ok(clientes);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ClienteEndereco clienteEndereco)
        {
            if (clienteEndereco == null || clienteEndereco.Cliente == null || clienteEndereco.Endereco == null)
                return BadRequest("Dados inválidos.");

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Inserir Cliente
                        var cmdCliente = conn.CreateCommand();
                        cmdCliente.Transaction = transaction;
                        cmdCliente.CommandText = @"INSERT INTO Cliente (CPF, Nome, RG, DataExpedicao, OrgaoExpedicao, UFExpedicao, DataNascimento, Sexo, EstadoCivil)
                                                   VALUES (@CPF, @Nome, @RG, @DataExpedicao, @OrgaoExpedicao, @UFExpedicao, @DataNascimento, @Sexo, @EstadoCivil)";
                        cmdCliente.Parameters.AddWithValue("@CPF", clienteEndereco.Cliente.CPF);
                        cmdCliente.Parameters.AddWithValue("@Nome", clienteEndereco.Cliente.Nome);
                        cmdCliente.Parameters.AddWithValue("@RG", clienteEndereco.Cliente.RG);
                        cmdCliente.Parameters.AddWithValue("@DataExpedicao", clienteEndereco.Cliente.DataExpedicao);
                        cmdCliente.Parameters.AddWithValue("@OrgaoExpedicao", clienteEndereco.Cliente.OrgaoExpedicao);
                        cmdCliente.Parameters.AddWithValue("@UFExpedicao", clienteEndereco.Cliente.UFExpedicao);
                        cmdCliente.Parameters.AddWithValue("@DataNascimento", clienteEndereco.Cliente.DataNascimento);
                        cmdCliente.Parameters.AddWithValue("@Sexo", clienteEndereco.Cliente.Sexo);
                        cmdCliente.Parameters.AddWithValue("@EstadoCivil", clienteEndereco.Cliente.EstadoCivil);
                        cmdCliente.ExecuteNonQuery();

                        // Recuperar o ID gerado
                        cmdCliente.CommandText = "SELECT last_insert_rowid()";
                        cmdCliente.Parameters.Clear();
                        var clienteId = (long)cmdCliente.ExecuteScalar();

                        // Inserir Endereço
                        var cmdEndereco = conn.CreateCommand();
                        cmdEndereco.Transaction = transaction;
                        cmdEndereco.CommandText = @"INSERT INTO EnderecoCliente (ClienteId, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, UF)
                                                     VALUES (@ClienteId, @CEP, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @UF)";
                        cmdEndereco.Parameters.AddWithValue("@ClienteId", clienteId);
                        cmdEndereco.Parameters.AddWithValue("@CEP", clienteEndereco.Endereco.CEP);
                        cmdEndereco.Parameters.AddWithValue("@Logradouro", clienteEndereco.Endereco.Logradouro);
                        cmdEndereco.Parameters.AddWithValue("@Numero", clienteEndereco.Endereco.Numero);
                        cmdEndereco.Parameters.AddWithValue("@Complemento", clienteEndereco.Endereco.Complemento);
                        cmdEndereco.Parameters.AddWithValue("@Bairro", clienteEndereco.Endereco.Bairro);
                        cmdEndereco.Parameters.AddWithValue("@Cidade", clienteEndereco.Endereco.Cidade);
                        cmdEndereco.Parameters.AddWithValue("@UF", clienteEndereco.Endereco.UF);
                        cmdEndereco.ExecuteNonQuery();

                        transaction.Commit();
                        return Ok(new { message = "Cliente cadastrado com sucesso!" });
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(500, "Erro ao cadastrar cliente.");
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ClienteEndereco clienteEndereco)
        {
            if (clienteEndereco == null || clienteEndereco.Cliente == null || clienteEndereco.Endereco == null)
                return BadRequest("Dados inválidos.");

            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Atualizar Cliente
                        var cmdCliente = conn.CreateCommand();
                        cmdCliente.Transaction = transaction;
                        cmdCliente.CommandText = @"UPDATE Cliente
                                           SET CPF = @CPF, Nome = @Nome, RG = @RG, DataExpedicao = @DataExpedicao, 
                                               OrgaoExpedicao = @OrgaoExpedicao, UFExpedicao = @UFExpedicao, 
                                               DataNascimento = @DataNascimento, Sexo = @Sexo, EstadoCivil = @EstadoCivil
                                           WHERE Id = @Id";
                        cmdCliente.Parameters.AddWithValue("@CPF", clienteEndereco.Cliente.CPF);
                        cmdCliente.Parameters.AddWithValue("@Nome", clienteEndereco.Cliente.Nome);
                        cmdCliente.Parameters.AddWithValue("@RG", clienteEndereco.Cliente.RG);
                        cmdCliente.Parameters.AddWithValue("@DataExpedicao", clienteEndereco.Cliente.DataExpedicao);
                        cmdCliente.Parameters.AddWithValue("@OrgaoExpedicao", clienteEndereco.Cliente.OrgaoExpedicao);
                        cmdCliente.Parameters.AddWithValue("@UFExpedicao", clienteEndereco.Cliente.UFExpedicao);
                        cmdCliente.Parameters.AddWithValue("@DataNascimento", clienteEndereco.Cliente.DataNascimento);
                        cmdCliente.Parameters.AddWithValue("@Sexo", clienteEndereco.Cliente.Sexo);
                        cmdCliente.Parameters.AddWithValue("@EstadoCivil", clienteEndereco.Cliente.EstadoCivil);
                        cmdCliente.Parameters.AddWithValue("@Id", id);
                        cmdCliente.ExecuteNonQuery();

                        // Atualizar Endereço
                        var cmdEndereco = conn.CreateCommand();
                        cmdEndereco.Transaction = transaction;
                        cmdEndereco.CommandText = @"UPDATE EnderecoCliente
                                             SET CEP = @CEP, Logradouro = @Logradouro, Numero = @Numero, 
                                                 Complemento = @Complemento, Bairro = @Bairro, Cidade = @Cidade, UF = @UF
                                             WHERE ClienteId = @ClienteId";
                        cmdEndereco.Parameters.AddWithValue("@CEP", clienteEndereco.Endereco.CEP);
                        cmdEndereco.Parameters.AddWithValue("@Logradouro", clienteEndereco.Endereco.Logradouro);
                        cmdEndereco.Parameters.AddWithValue("@Numero", clienteEndereco.Endereco.Numero);
                        cmdEndereco.Parameters.AddWithValue("@Complemento", clienteEndereco.Endereco.Complemento);
                        cmdEndereco.Parameters.AddWithValue("@Bairro", clienteEndereco.Endereco.Bairro);
                        cmdEndereco.Parameters.AddWithValue("@Cidade", clienteEndereco.Endereco.Cidade);
                        cmdEndereco.Parameters.AddWithValue("@UF", clienteEndereco.Endereco.UF);
                        cmdEndereco.Parameters.AddWithValue("@ClienteId", id);
                        cmdEndereco.ExecuteNonQuery();

                        transaction.Commit();
                        return Ok(new { message = "Cliente atualizado com sucesso!" });
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(500, "Erro ao atualizar cliente.");
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Deletar Endereço primeiro (chave estrangeira)
                        var cmdEndereco = conn.CreateCommand();
                        cmdEndereco.Transaction = transaction;
                        cmdEndereco.CommandText = @"DELETE FROM EnderecoCliente WHERE ClienteId = @ClienteId";
                        cmdEndereco.Parameters.AddWithValue("@ClienteId", id);
                        cmdEndereco.ExecuteNonQuery();

                        // Depois deletar Cliente
                        var cmdCliente = conn.CreateCommand();
                        cmdCliente.Transaction = transaction;
                        cmdCliente.CommandText = @"DELETE FROM Cliente WHERE Id = @Id";
                        cmdCliente.Parameters.AddWithValue("@Id", id);
                        cmdCliente.ExecuteNonQuery();

                        transaction.Commit();
                        return Ok(new { message = "Cliente removido com sucesso!" });
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(500, "Erro ao excluir cliente.");
                    }
                }
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ClienteEndereco> GetById(int id)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                string query = @"SELECT c.Id, c.Nome, c.CPF, c.RG, c.DataExpedicao, c.OrgaoExpedicao, c.UFExpedicao, 
                                c.DataNascimento, c.Sexo, c.EstadoCivil, 
                                e.CEP, e.Logradouro, e.Numero, e.Complemento, e.Bairro, e.Cidade, e.UF
                         FROM Cliente c
                         INNER JOIN EnderecoCliente e ON c.Id = e.ClienteId
                         WHERE c.Id = @Id";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var clienteEndereco = new ClienteEndereco
                            {
                                Cliente = new Cliente
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    CPF = reader.GetString(2),
                                    RG = reader.GetString(3),
                                    DataExpedicao = reader.GetString(4),
                                    OrgaoExpedicao = reader.GetString(5),
                                    UFExpedicao = reader.GetString(6),
                                    DataNascimento = reader.GetString(7),
                                    Sexo = reader.GetString(8),
                                    EstadoCivil = reader.GetString(9)
                                },
                                Endereco = new EnderecoCliente
                                {
                                    CEP = reader.GetString(10),
                                    Logradouro = reader.GetString(11),
                                    Numero = reader.GetString(12),
                                    Complemento = reader.GetString(13),
                                    Bairro = reader.GetString(14),
                                    Cidade = reader.GetString(15),
                                    UF = reader.GetString(16)
                                }
                            };

                            return Ok(clienteEndereco);
                        }
                        else
                        {
                            return NotFound(new { message = "Cliente não encontrado." });
                        }
                    }
                }
            }
        }
    }
}
