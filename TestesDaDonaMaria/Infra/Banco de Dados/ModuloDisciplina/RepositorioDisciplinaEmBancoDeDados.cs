using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestesDaDonaMaria.Dominio;
using TestesDaDonaMaria.Dominio.ModuloDisciplina;

namespace TestesDaDonaMaria.Infra.Banco_de_Dados.ModuloDisciplina
{
    public class RepositorioDisciplinaEmBancoDeDados : IRepositorioDisciplina
    {
        private const string enderecoBanco =
                @"Data Source=(LocalDB)\MSSqlLocalDB;" +
                 "Initial Catalog=TestesDaDonaMariaDb;" +
                 "Integrated Security=True;" +
                 "Pooling=False";

        #region Sql Queries
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
        #endregion

        public ValidationResult Inserir(Disciplina novaDisciplina)
        {
            var validador = new ValidadorDisciplina();

            var resultadoValidacao = validador.Validate(novaDisciplina);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoBanco);

            ConfigurarParametrosDisciplina(novaDisciplina, comandoInsercao);

            conexaoBanco.Open();

            var id = comandoInsercao.ExecuteScalar();

            novaDisciplina.Numero = (int)id;

            novaDisciplina.Numero = Convert.ToInt32(id);

            conexaoBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Disciplina disciplina)
        {
            var validador = new ValidadorDisciplina();

            var resultadoValidacao = validador.Validate(disciplina);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoBanco);

            ConfigurarParametrosDisciplina(disciplina, comandoEdicao);

            conexaoBanco.Open();

            var id = comandoEdicao.ExecuteNonQuery();

            disciplina.Numero = id;

            disciplina.Numero = Convert.ToInt32(id);

            conexaoBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Disciplina disciplina)
        {
            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", disciplina.Numero);

            conexaoBanco.Open();

            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos > 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoBanco.Close();

            return resultadoValidacao;
        }

        public List<Disciplina> SelecionarTodos()
        {
            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoBanco);

            conexaoBanco.Open();

            SqlDataReader leitorDisciplina = comandoSelecao.ExecuteReader();

            List<Disciplina> disciplinas = new List<Disciplina>();

            while (leitorDisciplina.Read())
            {
                Materia disciplina = ConverterParaDisciplina(leitorDisciplina);

                disciplinas.Add(disciplina);
            }

            conexaoBanco.Close();

            return disciplinas;
        }

        public Disciplina SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoBanco.Open();

            SqlDataReader leitorDisciplina = comandoSelecao.ExecuteReader();

            Disciplina disciplina = null;
            if (leitorDisciplina.Read())
            {
                disciplina = ConverterParaDisciplina(leitorDisciplina);
            }

            conexaoBanco.Close();

            return disciplina;
        }

        private static Disciplina ConverterParaDisciplina(SqlDataReader leitorDisciplina)
        {
            int numero = Convert.ToInt32(leitorDisciplina["NUMERO"]);
            string nome = Convert.ToString(leitorDisciplina["NOME"]);

            var disciplina = new Disciplina()
            {
                Numero = numero,
                Nome = nome
            };
            return disciplina;
        }

        private static void ConfigurarParametrosDisciplina(Disciplina novaDisciplina, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", novaDisciplina.Numero);
            comando.Parameters.AddWithValue("NOME", novaDisciplina.Nome);
        }
    }
}
