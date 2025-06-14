using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Program
{
    private static List<Curso> listaDeCursos = new List<Curso>();
    private static Dictionary<int, Curso> cursos = new Dictionary<int, Curso>();
    private static List<Candidato> candidatos = new List<Candidato>();


    public static void Main()
    {
        bool executando = true;

        do
        {

            Console.WriteLine("1. Carregar Arquivo de Entrada (entrada.txt)");
            Console.WriteLine("2. Processar Dados dos Candidatos");
            Console.WriteLine("3. Exibir Resultados por Curso");
            Console.WriteLine("4. Gerar Arquivo de Saída (saida.txt)");
            Console.WriteLine("5. Sair");

            Console.Write("Digite a sua opção: ");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.WriteLine("\n[Opção 1 selecionada: Carregar arquivo de Entrada]");
                    break;

                case "2":
                    Console.WriteLine("\n[Opção 2 selecionada: Processar Dados]");
                    break;

                case "3":
                    Console.WriteLine("\n[Opção 3 selecionada: Exibir Resultados]");
                    break;

                case "4":
                    Console.WriteLine("\n[Opção 4 selecionada: Gerar Arquivo de Saída]");
                    break;

                case "5":
                    Console.WriteLine("\nSaindo do programa...");
                    executando = false;
                    break;

                default:
                    Console.WriteLine("\nOpção inválida! Por favor, tente novamente!");
                    break;
            }

            if (executando)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        } while (executando);
    }


    public class Curso
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public int Vagas { get; set; }
        public List<Candidato> Selecionados { get; set; }
        public FilaFlexivel FilaDeEspera { get; set; }

        public Curso(int codigo, string nome, int vagas)
        {
            Codigo = codigo;
            Nome = nome;
            Vagas = vagas;
            Selecionados = new List<Candidato>();
            FilaDeEspera = new FilaFlexivel();
        }
    }

    public class FilaFlexivel
    {
        private class Celula
        {
            public Candidato Elemento { get; set; }
            public Celula Proximo { get; set; }

            public Celula(Candidato elemento)
            {
                Elemento = elemento;
                Proximo = null;
            }
        }

        private Celula primeiro;
        private Celula ultimo;

        public FilaFlexivel()
        {
            primeiro = new Celula(null);
            ultimo = primeiro;
        }

        public void Enfileirar(Candidato candidato)
        {
            ultimo.Proximo = new Celula(candidato);
            ultimo = ultimo.Proximo;
        }

        public List<Candidato> ParaLista()
        {
            var lista = new List<Candidato>();
            for (Celula i = primeiro.Proximo; i != null; i = i.Proximo)
            {
                lista.Add(i.Elemento);
            }
            return lista;
        }
    }

    public static class Ordenacao
    {
        public static void MergeSort(List<Candidato> lista)
        {
            if (lista.Count <= 1)
                return;

            int meio = lista.Count / 2;
            List<Candidato> esquerda = new List<Candidato>();
            List<Candidato> direita = new List<Candidato>();

            for (int i = 0; i < meio; i++)
                esquerda.Add(lista[i]);
            for (int i = meio; i < lista.Count; i++)
                direita.Add(lista[i]);

            MergeSort(esquerda);
            MergeSort(direita);
            Merge(lista, esquerda, direita);
        }

        private static void Merge(List<Candidato> original, List<Candidato> esquerda, List<Candidato> direita)
        {
            int iEsq = 0, iDir = 0, iOrig = 0;

            while (iEsq < esquerda.Count && iDir < direita.Count)
            {
                if (esquerda[iEsq].CompareTo(direita[iDir]) <= 0)
                {
                    original[iOrig++] = esquerda[iEsq++];
                }
                else
                {
                    original[iOrig++] = direita[iDir++];
                }
            }

            while (iEsq < esquerda.Count)
            {
                original[iOrig++] = esquerda[iEsq++];
            }

            while (iDir < direita.Count)
            {
                original[iOrig++] = direita[iDir++];
            }
        }
    }
}

public class Candidato
{

}