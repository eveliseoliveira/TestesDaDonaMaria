﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentValidation.Results;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestesDaDonaMaria.Dominio;

namespace TestesDaDonaMaria.Apresentacao.ModuloDisciplina
{
    public partial class TelaCadastroDisciplinaForm : Form
    {
        private Materia disciplina;
       
        public Func<Materia, ValidationResult> GravarRegistro { get; set; }

        public TelaCadastroDisciplinaForm()
        {
            InitializeComponent();

        }

        public Materia Disciplina
        {
            get
            {
                return disciplina;
            }
            set
            {
                disciplina = value;
                txtNumero.Text = disciplina.Numero.ToString();
                txtNome.Text = disciplina.Nome;
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            disciplina.Nome = txtNome.Text;

            var resultadoValidacao = GravarRegistro(disciplina);

            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                MessageBox.Show(erro,
               "Cadastro de Disciplinas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                 return;

                DialogResult = DialogResult.None;
            }

        }
    }
}
