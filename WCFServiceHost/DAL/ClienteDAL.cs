using System.Collections.Generic;
using System.Data.SQLite;
using WCFServiceHost.Model;

namespace WCFServiceHost.DAL
{
    public class ClienteDAL
    {
        public bool Inserir(Cliente cliente, EnderecoCliente endereco)
        {
            using (var conn = DBHelper.GetConnection())
            {
                var cmd = conn.CreateCommand();
                var tx = conn.BeginTransaction();

                try
                {
                    cmd.CommandText = "INSERT INTO Cliente (CPF, Nome, RG, DataExpedicao, OrgaoExpedicao, UFExpedicao, DataNascimento, Sexo, EstadoCivil) " +
                                      "VALUES (@CPF, @Nome, @RG, @DataExpedicao, @OrgaoExpedicao, @UFExpedicao, @DataNascimento, @Sexo, @EstadoCivil)";
                    cmd.Parameters.AddWithValue("@CPF", cliente.CPF);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@RG", cliente.RG);
                    cmd.Parameters.AddWithValue("@DataExpedicao", cliente.DataExpedicao);
                    cmd.Parameters.AddWithValue("@OrgaoExpedicao", cliente.OrgaoExpedicao);
                    cmd.Parameters.AddWithValue("@UFExpedicao", cliente.UFExpedicao);
                    cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
                    cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                    cmd.Parameters.AddWithValue("@EstadoCivil", cliente.EstadoCivil);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT last_insert_rowid()";
                    var clienteId = (long)cmd.ExecuteScalar();

                    cmd.CommandText = "INSERT INTO EnderecoCliente (ClienteId, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, UF) " +
                                      "VALUES (@ClienteId, @CEP, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @UF)";
                    cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                    cmd.Parameters.AddWithValue("@CEP", endereco.CEP);
                    cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                    cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                    cmd.Parameters.AddWithValue("@Complemento", endereco.Complemento);
                    cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                    cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                    cmd.Parameters.AddWithValue("@UF", endereco.UF);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }

        public List<Cliente> Listar()
        {
            var lista = new List<Cliente>();

            using (var conn = DBHelper.GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Cliente";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Cliente
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Nome = reader["Nome"].ToString(),
                            CPF = reader["CPF"].ToString(),
                            RG = reader["RG"].ToString(),
                            DataExpedicao = reader["DataExpedicao"].ToString(),
                            OrgaoExpedicao = reader["OrgaoExpedicao"].ToString(),
                            UFExpedicao = reader["UFExpedicao"].ToString(),
                            DataNascimento = reader["DataNascimento"].ToString(),
                            Sexo = reader["Sexo"].ToString(),
                            EstadoCivil = reader["EstadoCivil"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public bool Editar(Cliente cliente, EnderecoCliente endereco)
        {
            using (var conn = DBHelper.GetConnection())
            {
                var cmd = conn.CreateCommand();
                var tx = conn.BeginTransaction();

                try
                {
                    cmd.CommandText = "UPDATE Cliente SET CPF = @CPF, Nome = @Nome, RG = @RG, DataExpedicao = @DataExpedicao, " +
                                      "OrgaoExpedicao = @OrgaoExpedicao, UFExpedicao = @UFExpedicao, DataNascimento = @DataNascimento, " +
                                      "Sexo = @Sexo, EstadoCivil = @EstadoCivil WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    cmd.Parameters.AddWithValue("@CPF", cliente.CPF);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@RG", cliente.RG);
                    cmd.Parameters.AddWithValue("@DataExpedicao", cliente.DataExpedicao);
                    cmd.Parameters.AddWithValue("@OrgaoExpedicao", cliente.OrgaoExpedicao);
                    cmd.Parameters.AddWithValue("@UFExpedicao", cliente.UFExpedicao);
                    cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
                    cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                    cmd.Parameters.AddWithValue("@EstadoCivil", cliente.EstadoCivil);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE EnderecoCliente SET CEP = @CEP, Logradouro = @Logradouro, Numero = @Numero, " +
                                      "Complemento = @Complemento, Bairro = @Bairro, Cidade = @Cidade, UF = @UF " +
                                      "WHERE ClienteId = @ClienteId";
                    cmd.Parameters.AddWithValue("@ClienteId", cliente.Id);
                    cmd.Parameters.AddWithValue("@CEP", endereco.CEP);
                    cmd.Parameters.AddWithValue("@Logradouro", endereco.Logradouro);
                    cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                    cmd.Parameters.AddWithValue("@Complemento", endereco.Complemento);
                    cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                    cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                    cmd.Parameters.AddWithValue("@UF", endereco.UF);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }

        public bool Excluir(int clienteId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                var cmd = conn.CreateCommand();
                var tx = conn.BeginTransaction();

                try
                {
                    cmd.CommandText = "DELETE FROM EnderecoCliente WHERE ClienteId = @ClienteId";
                    cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Cliente WHERE Id = @ClienteId";
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }
    }
}
