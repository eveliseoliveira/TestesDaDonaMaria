﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestesDaDonaMaria.Infra;
using TestesDaDonaMaria.Apresentacao.Compartilhado;
using TestesDaDonaMaria.Apresentacao.ModuloMateria;
using TestesDaDonaMaria.Apresentacao.ModuloQuestao;
using TestesDaDonaMaria.Apresentacao.ModuloTeste;
using TestesDaDonaMaria.Infra.Banco_de_Dados.ModuloDisciplina;
using TestesDaDonaMaria.Infra.Banco_de_Dados.ModuloMateria;
using TestesDaDonaMaria.Dominio.ModuloDisciplina;
using TestesDaDonaMaria.Dominio.ModuloMateria;

namespace TestesDaDonaMaria.Apresentacao.ModuloDisciplina
{
    public partial class MenuPrincipalForm : Form
    {

        private ControladorBase controlador;
        private Dictionary<string, ControladorBase> controladores;
        private DataContext contextoDados;


        public MenuPrincipalForm(DataContext contextoDados)
        {
            InitializeComponent();

            this.contextoDados = contextoDados;

            InicializarControladores();
        }

        private void InicializarControladores()
        {
            IRepositorioDisciplina repositorioDisciplina = new RepositorioDisciplinaEmBancoDeDados();
            IRepositorioMateria repositorioMateria = new RepositorioMateriaEmBancoDeDados();
            var repositorioQuestao = new RepositorioQuestao(contextoDados);
            var repositorioTeste = new RepositorioTeste(contextoDados);


            controladores = new Dictionary<string, ControladorBase>();

            controladores.Add("Disciplinas", new ControladorDisciplina(repositorioDisciplina));
            controladores.Add("Materias", new ControladorMateria(repositorioMateria, repositorioDisciplina));
            controladores.Add("Questoes", new ControladorQuestao(repositorioQuestao, repositorioDisciplina, repositorioMateria));
            controladores.Add("Testes", new ControladorTeste(repositorioTeste, repositorioQuestao, repositorioDisciplina, repositorioMateria));

        }

        private void testesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarToolbox(new ConfiguracaoToolBoxTestes());

            var opcaoSelecionada = (ToolStripMenuItem)sender;

            SelecionarControlador(opcaoSelecionada);

            CarregarListagem();
        }

        private void DisciplinasMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarToolbox(new ConfiguracaoToolBoxDisciplina());
                         
            var opcaoSelecionada = (ToolStripMenuItem)sender;

            SelecionarControlador(opcaoSelecionada);

            CarregarListagem();
        }

        private void materiasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ConfigurarToolbox(new ConfiguracaoToolboxMateria());

            var opcaoSelecionada = (ToolStripMenuItem)sender;

            SelecionarControlador(opcaoSelecionada);

            CarregarListagem();
        }

        private void questoesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarToolbox(new ConfiguracaoToolboxQuestao());

            var opcaoSelecionada = (ToolStripMenuItem)sender;

            SelecionarControlador(opcaoSelecionada);

            CarregarListagem();
        }

        private void btInserir_Click(object sender, EventArgs e)
        {
            if (controlador == null)
            {
                return;
            }
            controlador.Inserir();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (controlador == null)
            {
                return;
            }
            controlador.Editar(/*disciplinaSelecionada*/);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (controlador == null)
            {
                return;
            }
            controlador.Excluir();
        }

        private void SelecionarControlador(ToolStripMenuItem opcaoSelecionada)
        {
            var tipo = opcaoSelecionada.Text;

            controlador = controladores[tipo];
        }

        private void CarregarListagem()
        {
            var listagemControl = controlador.ObtemListagem();

            panelRegistros.Controls.Clear();

            listagemControl.Dock = DockStyle.Fill;

            panelRegistros.Controls.Add(listagemControl);
        }

        private void ConfigurarToolbox(ConfiguracaoToolBoxBase configuracao)
        {
            ConfigurandoTooltips(configuracao);

            ConfigurandoBotoes(configuracao);
        }

        private void ConfigurandoBotoes(ConfiguracaoToolBoxBase configuracao)
        {
            btnInserir.Enabled = configuracao.InserirHabilitado;
            btnEditar.Enabled = configuracao.EditarHabilitado;
            btnExcluir.Enabled = configuracao.ExcluirHabilitado;
        }

        private void ConfigurandoTooltips(ConfiguracaoToolBoxBase configuracao)
        {
            btnInserir.ToolTipText = configuracao.TooltipInserir;
            btnEditar.ToolTipText = configuracao.TooltipEditar;
            btnExcluir.ToolTipText = configuracao.TooltipExcluir;
            
        }

        private void btnDuplicar_Click(object sender, EventArgs e)
        {
                if (controlador == null)
                {
                    return;
                }
            ((ControladorTeste)controlador).Duplicar();
            
 
        }

        private void btnGerarPDF_Click(object sender, EventArgs e)
        {
            if (controlador == null)
            {
                return;
            }
            ((ControladorTeste)controlador).GerarPdf();
        }
    }
}