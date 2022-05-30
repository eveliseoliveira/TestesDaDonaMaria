using System.Collections.Generic;

namespace TestesDaDonaMaria.Dominio
{
    public class Materia : EntidadeBase<Materia>
    {
       
        private List<Materia> materias = new List<Materia>();


        public string Nome { get; set; }
        public List<Materia> Materias { get { return materias; } }

        public override void Atualizar(Materia registro)
        {
            
        }

        public override string ToString()
        {
            string materiasToString = "";
            
            if (Materias != null)
            {
                foreach (var materia in Materias)
                    materiasToString += materia.Titulo + ", ";
            }
            
            
            return $"Número: {Numero}, Nome: {Nome}";
        }
    }
}