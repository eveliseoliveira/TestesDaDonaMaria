﻿using System.Windows.Forms;

namespace TestesDaDonaMaria.Apresentacao.Compartilhado

{
    public abstract class ControladorBase
    {

        public abstract void Inserir();
        public abstract void Editar();
        public abstract void Excluir();

        public virtual void AdicionarItens() { }

        public virtual void AtualizarItens() { }

        public abstract UserControl ObtemListagem();
    }
}