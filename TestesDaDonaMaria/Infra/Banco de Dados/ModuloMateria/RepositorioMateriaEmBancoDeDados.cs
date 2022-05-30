using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestesDaDonaMaria.Dominio;
using TestesDaDonaMaria.Dominio.ModuloMateria;

namespace TestesDaDonaMaria.Infra.Banco_de_Dados.ModuloMateria
{
    public class RepositorioMateriaEmBancoDeDados : IRepositorioMateria
    {
        private const string enderecoBanco =
            @"Data Source=(LocalDB)\MSSqlLocalDB;" +
             "Initial Catalog=TestesDaDonaMariaDb;" +
             "Integrated Security=True;" +
             "Pooling=False";

        private const string sqlInserir =
        @"INSET INTO [LISTADISCIPLINAS]
                    (
	                  [NOME]
                    )
                      VALUES
                    (
                      @NOME
                    );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
                @"UPDATE [LISTADISCIPLINAS]
                        SET
	                        [NOME] = @NOME;
                        WHERE
                            [NUMERO] = @NUMERO";

        private const string sqlExcluir =
            @"DELETE FROM [LISTADISCIPLINAS]
                        WHERE
                            [NUMERO] = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT
                            [NUMERO], 
	                        [NOME]
                        FROM
                        VALUES
                            [LISTADISCIPLINAS]";

        private const string sqlSelecionarPorNumero =
            @"SELECT
                            [NUMERO], 
	                        [NOME]
                        FROM
                        VALUES
                            [LISTADISCIPLINAS]";

        public ValidationResult Inserir(Materia novaMateria)
        {
            var validador = new ValidadorMateria();

            var resultadoValidacao = validador.Validate(novaMateria);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoBanco);

            ConfigurarParametrosMateria(novaMateria, comandoInsercao);

            conexaoBanco.Open();

            var id = comandoInsercao.ExecuteScalar();

            novaMateria.Numero = (int)id;

            novaMateria.Numero = Convert.ToInt32(id);

            conexaoBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Materia materia)
        {
            var validador = new ValidadorMateria();

            var resultadoValidacao = validador.Validate(materia);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoBanco);

            ConfigurarParametrosMateria(materia, comandoEdicao);

            conexaoBanco.Open();

            var id = comandoEdicao.ExecuteNonQuery();

            materia.Numero = id;

            materia.Numero = Convert.ToInt32(id);

            conexaoBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Materia materia)
        {
            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", materia.Numero);

            conexaoBanco.Open();

            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos > 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoBanco.Close();

            return resultadoValidacao;
        }
        public List<Materia> SelecionarTodos()
        {
            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoBanco);

            conexaoBanco.Open();

            SqlDataReader leitorMateria = comandoSelecao.ExecuteReader();

            List<Materia> materias = new List<Materia>();

            while (leitorMateria.Read())
            {
                Materia materia = ConverterParaMateria(leitorMateria);

                materias.Add(materia);
            }

            conexaoBanco.Close();

            return materias;
        }
        public Materia SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoBanco.Open();

            SqlDataReader leitorMateria = comandoSelecao.ExecuteReader();

            Materia materia = null;
            if (leitorMateria.Read())
            {
                materia = ConverterParaMateria(leitorMateria);
            }

            conexaoBanco.Close();

            return materia;
        }
        private static Materia ConverterParaMateria(SqlDataReader leitorMateria)
        {
            int numero = Convert.ToInt32(leitorMateria["NUMERO"]);
            string nome = Convert.ToString(leitorMateria["NOME"]);

            var materia = new Materia()
            {
                Numero = numero,
                Nome = nome
            };
            return materia;
        }

        private void ConfigurarParametrosMateria(Materia novaMateria, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", novaMateria.Numero);
            comando.Parameters.AddWithValue("NOME", novaMateria.Nome);
        }
    }


}
