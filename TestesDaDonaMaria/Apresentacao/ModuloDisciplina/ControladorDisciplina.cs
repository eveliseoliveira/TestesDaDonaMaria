﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestesDaDonaMaria.Apresentacao.Compartilhado;
using TestesDaDonaMaria.Apresentacao.ModuloDisciplina;
using TestesDaDonaMaria.Dominio;
using TestesDaDonaMaria.Infra;
using TestesDaDonaMaria.Infra.ModuloDisciplina;

namespace TestesDaDonaMaria.Apresentacao.ModuloDisciplina
{
    public class ControladorDisciplina : ControladorBase
    {

        private RepositorioDisciplina repositorioDisciplina;
        private ListagemDisciplinasControl listagemDisciplinas;

        public RepositorioDisciplinaEmBancoDeDados RepositorioDisciplina { get; }

        public ControladorDisciplina(RepositorioDisciplina repositorioDisciplina)
        {
            this.repositorioDisciplina = repositorioDisciplina;
        }

        public override void Editar()
        {
            Materia disciplinaSelecionada = listagemDisciplinas.ObtemDisciplinaSelecionada();

            if (disciplinaSelecionada == null)
            {
                MessageBox.Show("Selecione uma disciplina primeiro",
                "Edição de Disciplinas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
           
            TelaCadastroDisciplinaForm tela = new TelaCadastroDisciplinaForm();

            tela.Disciplina = disciplinaSelecionada;

            tela.GravarRegistro = repositorioDisciplina.Editar;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarDisciplinas();
            }
        }

        public override void Excluir()
        {
            Materia disciplinaSelecionada = listagemDisciplinas.ObtemDisciplinaSelecionada();

            if (disciplinaSelecionada == null)
            {
                MessageBox.Show("Selecione uma disciplina primeiro",
                "Exclusão de Disciplinas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string validacao = ValidarExclusao(disciplinaSelecionada);

            if(validacao != "")
            {
                MessageBox.Show(validacao,
                "Exclusao de Disciplinas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult resultado = MessageBox.Show("Deseja realmente excluir a disciplina?",
                "Exclusão de Disciplinas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                repositorioDisciplina.Excluir(disciplinaSelecionada);
                CarregarDisciplinas();
            }
        }

        private string ValidarExclusao(Materia disciplinaSelecionada)
        {
            string validacao = "";

            if (disciplinaSelecionada.Materias.Count != 0)
                validacao = "Não é possível excluir uma disciplina relacionada com " +
                    "uma matéria";

            return validacao;
        }

        public override void Inserir()
        {
            TelaCadastroDisciplinaForm tela = new TelaCadastroDisciplinaForm();
            
            tela.Disciplina = new Materia();

            tela.GravarRegistro = repositorioDisciplina.Inserir;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarDisciplinas();
            }
        }

       

        private void CarregarDisciplinas()
        {
            var disciplinas = repositorioDisciplina.ObterRegistros();

            listagemDisciplinas.AtualizarRegistros(disciplinas);
        }

        public override UserControl ObtemListagem()
        {
            if (listagemDisciplinas == null)
                listagemDisciplinas = new ListagemDisciplinasControl();

            CarregarDisciplinas();

            return listagemDisciplinas;
        }
    }
}
