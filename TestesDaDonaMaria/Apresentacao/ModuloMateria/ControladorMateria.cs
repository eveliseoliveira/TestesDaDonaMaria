using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestesDaDonaMaria.Apresentacao.Compartilhado;
using TestesDaDonaMaria.Dominio;
using TestesDaDonaMaria.Dominio.ModuloDisciplina;
using TestesDaDonaMaria.Dominio.ModuloMateria;
using TestesDaDonaMaria.Infra;
using TestesDaDonaMaria.Infra.Banco_de_Dados.ModuloDisciplina;
using TestesDaDonaMaria.Infra.Banco_de_Dados.ModuloMateria;

namespace TestesDaDonaMaria.Apresentacao.ModuloMateria
{
    public class ControladorMateria : ControladorBase
    {
        private IRepositorioDisciplina repositorioDisciplina;
        private IRepositorioMateria repositorioMateria;
        private ListagemMateriaControl listagemMaterias;
        private RepositorioMateriaEmBancoDeDados repositorioMateria1;
        private RepositorioDisciplinaEmBancoDeDados repositorioDisciplina1;

        public ControladorMateria(IRepositorioMateria repositorioMateria, IRepositorioDisciplina repositorioDisciplina)
        {
            this.repositorioMateria = repositorioMateria;
            this.repositorioDisciplina = repositorioDisciplina;
        }

        public override void Editar()
        {
            Materia materiaSelecionada = listagemMaterias.ObtemMateriaSelecionada();

            if (materiaSelecionada == null)
            {
                MessageBox.Show("Selecione uma matéria primeiro",
                "Edição de Matérias", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TelaCadastroMateriaForm tela = new TelaCadastroMateriaForm(repositorioMateria);

            tela.Materia = materiaSelecionada;

           tela.GravarRegistro = repositorioMateria.Editar;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
               
                CarregarMaterias();
            }
        }

        private void CarregarMaterias()
        {
            //var materias = repositorioMateria.ObterRegistros();

            var materias = repositorioMateria.SelecionarTodos();

            listagemMaterias.AtualizarRegistros(materias);
        }

        public override void Excluir()
        {
            Materia materiaSelecionada = listagemMaterias.ObtemMateriaSelecionada();

            if (materiaSelecionada == null)
            {
                MessageBox.Show("Selecione uma matéria primeiro",
                "Exclusão de Matérias", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult resultado = MessageBox.Show("Deseja realmente excluir a matéria?",
                "Exclusão de Materias", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                repositorioMateria.Excluir(materiaSelecionada);
                CarregarMaterias();
            }
        }

        public override void Inserir()
        {
            TelaCadastroMateriaForm tela = new TelaCadastroMateriaForm(repositorioMateria);
           
            tela.Materia = new Materia();

            tela.GravarRegistro = repositorioMateria.Inserir;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                if(tela.Materia.Disciplina != null)
                AtualizarDisciplina(tela.Materia);

                CarregarMaterias();
            }
        }

        private void AtualizarDisciplina(Materia materia)
        {
            materia.Disciplina.Materias.Add(materia);
        }

        public override UserControl ObtemListagem()
        {

            if (listagemMaterias == null)
                listagemMaterias = new ListagemMateriaControl();

            CarregarMaterias();

            return listagemMaterias;
        }

      
    }
}
