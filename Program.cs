using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

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
                    CarregarEntrada("entrada.txt");
                    break;

                case "2":
                    Console.WriteLine("\n[Opção 2 selecionada: Processar Dados]");
                    break;

                case "3":
                    Console.WriteLine("\n[Opção 3 selecionada: Exibir Resultados]");
                    break;

                case "4":
                    Console.WriteLine("\n[Opção 4 selecionada: Gerar Arquivo de Saída]");
                    GerarSaida("saida.txt");
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



    // Lê o arquivo de entrada e popula as listas de cursos e candidatos
    static void CarregarEntrada(string caminhoArquivo)
    {
        // Verifica se o arquivo existe antes de tentar abrir
        if (!File.Exists(caminhoArquivo))
        {
            Console.WriteLine("Arquivo não encontrado!");
            return;
        }

        // Limpa listas anteriores para evitar dados duplicados em múltiplas cargas
        listaDeCursos.Clear();
        listaDeCandidatos.Clear();

        // Lê todas as linhas do arquivo para processamento
        var linhas = File.ReadAllLines(caminhoArquivo);

        // A primeira linha contém a quantidade de cursos e candidatos, separados por ';'
        var primeiraLinha = linhas[0].Split(';');

        int qtdCursos = int.Parse(primeiraLinha[0]);
        int qtdCandidatos = int.Parse(primeiraLinha[1]);

        // A partir da linha 1 até qtdCursos, são as informações dos cursos
        for (int i = 1; i <= qtdCursos; i++)
        {
            // Cada linha tem: codigo;nome;vagas
            var partes = linhas[i].Split(';');
            listaDeCursos.Add(new Curso
            {
                Codigo = int.Parse(partes[0]),
                Nome = partes[1],
                Vagas = int.Parse(partes[2])
            });
        }

        // A partir da linha (qtdCursos+1) até (qtdCursos+qtdCandidatos), estão os candidatos
        for (int i = qtdCursos + 1; i < qtdCursos + 1 + qtdCandidatos; i++)
        {
            var partes = linhas[i].Split(';');
            listaDeCandidatos.Add(new Candidato
            {
                Nome = partes[0],
                NotaRed = double.Parse(partes[1]),
                NotaMat = double.Parse(partes[2]),
                NotaLing = double.Parse(partes[3]),
                CodCursoOp1 = int.Parse(partes[4]),
                CodCursoOp2 = int.Parse(partes[5])
            });
        }

        Console.WriteLine($"Arquivo carregado com sucesso: {qtdCursos} cursos e {qtdCandidatos} candidatos.");
    }

  
    // Gera o arquivo de saída 
    static void GerarSaida(string caminho)
    {
        if (listaDeCursos.Count == 0)
        {
            Console.WriteLine("Nenhum curso carregado.");
            return;
        }

        // Usa StreamWriter para escrever linhas no arquivo especificado
        using (var writer = new StreamWriter(caminho))
        {
            // Para cada curso, escreve as informações de saída formatadas
            foreach (var curso in listaDeCursos)
            {
                // Linha com nome do curso e nota de corte (com duas casas decimais e vírgula)
                writer.WriteLine($"{curso.Nome} {FormatarNota(curso.NotaCorte)}");
                // Cabeçalho da lista de selecionados
                writer.WriteLine("Selecionados");

                // Escreve cada candidato selecionado com nome e nota média formatada / Aqui as listas já estão ordenadas ao processar
                foreach (var c in curso.Selecionados)
                {
                    writer.WriteLine($"{c.Nome} {FormatarNota(c.NotaMedia)}");
                }

                // Cabeçalho da fila de espera
                writer.WriteLine("Fila de Espera");

                // Escreve cada candidato na fila de espera
                foreach (var c in curso.FilaEspera)
                {
                    writer.WriteLine($"{c.Nome} {FormatarNota(c.NotaMedia)}");
                }
                // Linha em branco para separar os cursos no arquivo
                writer.WriteLine();
            }
        }
        Console.WriteLine($"Arquivo '{caminho}' gerado com sucesso.");
    }

    // Formata nota no padrão com 2 casas decimais e vírgula decimal
    static string FormatarNota(double nota)
    {
        return nota.ToString("F2").Replace('.', ',');
    }
}


public class Candidato
{
    public string Nome { get; set; }
    public double NotaRed { get; set; }
    public double NotaMat { get; set; }
    public double NotaLing { get; set; }
    public int CodCursoOp1 { get; set; }
    public int CodCursoOp2 { get; set; }

    public double NotaMedia => (NotaRed + NotaMat + NotaLing) / 3.0;
}

// Classe que representa um curso
public class Curso
{
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public int Vagas { get; set; }

    public List<Candidato> Selecionados { get; set; } = new List<Candidato>();
    public List<Candidato> FilaEspera { get; set; } = new List<Candidato>();

    public double NotaCorte { get; set; }
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
