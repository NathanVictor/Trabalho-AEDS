using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public class Program
{

    private static List<Curso> listaDeCursos = new List<Curso>();
    private static List<Candidato> listaDeCandidatos = new List<Candidato>();
    private static Dictionary<int, Curso> cursosMap = new Dictionary<int, Curso>();

    public static void Main()
    {
        bool executando = true;
        do
        {

            Console.WriteLine("\n1. Carregar Arquivo de Entrada (entrada.txt)");
            Console.WriteLine("2. Processar Dados dos Candidatos");
            Console.WriteLine("3. Exibir Resultados por Curso");
            Console.WriteLine("4. Gerar Arquivo de Saída (saida.txt)");
            Console.WriteLine("5. Sair");

            Console.Write("Digite a sua opção: ");
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.WriteLine("\n[Opção 1: Carregar arquivo de Entrada]");
                    CarregarEntrada("entrada.txt");
                    break;
                case "2":
                    Console.WriteLine("\n[Opção 2: Processar Dados]");
                    ProcessarDados();
                    break;
                case "3":
                    Console.WriteLine("\n[Opção 3: Exibir Resultados]");
                    ExibirResultados();
                    break;
                case "4":
                    Console.WriteLine("\n[Opção 4: Gerar Arquivo de Saída]");
                    GerarSaida("saida.txt");
                    break;
                case "5":
                    Console.WriteLine("\nSaindo do programa...");
                    executando = false;
                    break;
                default:
                    Console.WriteLine("\nOpção inválida! Tente novamente.");
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


    static void CarregarEntrada(string caminhoArquivo)
    {
        if (!File.Exists(caminhoArquivo))
        {
            Console.WriteLine($"Erro: Arquivo '{caminhoArquivo}' não encontrado!");
            return;
        }

        listaDeCursos.Clear();
        listaDeCandidatos.Clear();
        cursosMap.Clear();

        var linhas = File.ReadAllLines(caminhoArquivo);
        var primeiraLinha = linhas[0].Split(';');
        int qtdCursos = int.Parse(primeiraLinha[0]);
        int qtdCandidatos = int.Parse(primeiraLinha[1]);

        for (int i = 1; i <= qtdCursos; i++)
        {
            var partes = linhas[i].Split(';');
            var novoCurso = new Curso(int.Parse(partes[0]), partes[1], int.Parse(partes[2]));
            listaDeCursos.Add(novoCurso);
            cursosMap[novoCurso.Codigo] = novoCurso;
        }

        for (int i = qtdCursos + 1; i < qtdCursos + 1 + qtdCandidatos; i++)
        {
            var partes = linhas[i].Split(';');
            listaDeCandidatos.Add(new Candidato
            {
                Nome = partes[0],
                NotaRed = double.Parse(partes[1], CultureInfo.InvariantCulture),
                NotaMat = double.Parse(partes[2], CultureInfo.InvariantCulture),
                NotaLing = double.Parse(partes[3], CultureInfo.InvariantCulture),
                CodCursoOp1 = int.Parse(partes[4]),
                CodCursoOp2 = int.Parse(partes[5])
            });
        }
        Console.WriteLine($"Arquivo carregado: {qtdCursos} cursos e {qtdCandidatos} candidatos.");
    }


    static void ProcessarDados()
    {
        if (listaDeCandidatos.Count == 0)
        {
            Console.WriteLine("Dados não carregados. Use a Opção 1 primeiro.");
            return;
        }


        foreach (var curso in listaDeCursos) curso.Resetar();


        var candidatosOrdenados = new List<Candidato>(listaDeCandidatos);
        Ordenacao.MergeSort(candidatosOrdenados);


        var statusSelecao = new Dictionary<Candidato, int>();


        foreach (var candidato in candidatosOrdenados)
        {
            if (cursosMap.TryGetValue(candidato.CodCursoOp1, out var curso) && curso.Selecionados.Count < curso.Vagas)
            {
                curso.Selecionados.Add(candidato);
                statusSelecao[candidato] = 1;
            }
        }


        foreach (var candidato in candidatosOrdenados)
        {
            if (!statusSelecao.ContainsKey(candidato)) 
            {
                if (cursosMap.TryGetValue(candidato.CodCursoOp2, out var curso) && curso.Selecionados.Count < curso.Vagas)
                {
                    curso.Selecionados.Add(candidato);
                    statusSelecao[candidato] = 2;
                }
            }
        }

        
        foreach (var candidato in candidatosOrdenados)
        {
            int status = statusSelecao.GetValueOrDefault(candidato, 0);
            if (status == 2) 
            {
                if (cursosMap.TryGetValue(candidato.CodCursoOp1, out var cursoOp1)) cursoOp1.FilaEspera.Enfileirar(candidato);
            }
            else if (status == 0) 
            {
                if (cursosMap.TryGetValue(candidato.CodCursoOp1, out var cursoOp1)) cursoOp1.FilaEspera.Enfileirar(candidato);
                if (cursosMap.TryGetValue(candidato.CodCursoOp2, out var cursoOp2)) cursoOp2.FilaEspera.Enfileirar(candidato);
            }
        }

        foreach (var curso in listaDeCursos) curso.CalcularNotaCorte();
        
        Console.WriteLine("Dados processados com sucesso.");
    }
    

    static void ExibirResultados()
    {
        if (!listaDeCursos.Any() || listaDeCursos.All(c => c.Selecionados.Count == 0 && c.FilaEspera.EstaVazia()))
        {
            Console.WriteLine("Nenhum dado processado. Use as Opções 1 e 2 primeiro.");
            return;
        }

        foreach (var curso in listaDeCursos)
        {
            Console.WriteLine($"\n--------------------------------------------------");
            Console.WriteLine($"{curso.Nome} - Nota de Corte: {FormatarNota(curso.NotaCorte)}");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("\n>> APROVADOS");
            var selecionadosOrdenados = curso.Selecionados;
            selecionadosOrdenados.Sort(); 
            if (selecionadosOrdenados.Any())
            {
                foreach (var c in selecionadosOrdenados) Console.WriteLine($"- {c.Nome} (Média: {FormatarNota(c.NotaMedia)})");
            }
            else Console.WriteLine("Nenhum candidato aprovado.");

            Console.WriteLine("\n>> FILA DE ESPERA");
            var esperaOrdenada = curso.FilaEspera.ParaLista();
            esperaOrdenada.Sort(); 
            if (esperaOrdenada.Any())
            {
                foreach (var c in esperaOrdenada) Console.WriteLine($"- {c.Nome} (Média: {FormatarNota(c.NotaMedia)})");
            }
            else Console.WriteLine("Fila de espera vazia.");
        }
    }


    static void GerarSaida(string caminho)
    {
        if (!listaDeCursos.Any() || listaDeCursos.All(c => c.Selecionados.Count == 0 && c.FilaEspera.EstaVazia()))
        {
            Console.WriteLine("Nenhum dado processado para gerar o arquivo.");
            return;
        }

        using (var writer = new StreamWriter(caminho, false, Encoding.UTF8))
        {
            foreach (var curso in listaDeCursos)
            {
                writer.WriteLine($"{curso.Nome} {FormatarNota(curso.NotaCorte)}");
                
                writer.WriteLine("Selecionados");
                var selecionadosOrdenados = curso.Selecionados;
                selecionadosOrdenados.Sort();
                foreach (var c in selecionadosOrdenados) writer.WriteLine($"{c.Nome} {FormatarNota(c.NotaMedia)}");

                writer.WriteLine("Fila de Espera");
                var esperaOrdenada = curso.FilaEspera.ParaLista();
                esperaOrdenada.Sort();
                foreach (var c in esperaOrdenada) writer.WriteLine($"{c.Nome} {FormatarNota(c.NotaMedia)}");
                
                writer.WriteLine();
            }
        }
        Console.WriteLine($"Arquivo '{caminho}' gerado com sucesso.");
    }

    static string FormatarNota(double nota) => nota.ToString("F2", new CultureInfo("pt-BR"));
}

public class Candidato : IComparable<Candidato>
{
    public string Nome { get; set; }
    public double NotaRed { get; set; }
    public double NotaMat { get; set; }
    public double NotaLing { get; set; }
    public int CodCursoOp1 { get; set; }
    public int CodCursoOp2 { get; set; }

    public double NotaMedia => (NotaRed + NotaMat + NotaLing) / 3.0;


    public int CompareTo(Candidato other)
    {
        if (other == null) return 1;
        int result = other.NotaMedia.CompareTo(this.NotaMedia);
        if (result == 0)
        {
            result = other.NotaRed.CompareTo(this.NotaRed);
        }
        return result;
    }
}

public class Curso
{
    public int Codigo { get; }
    public string Nome { get; }
    public int Vagas { get; }
    public double NotaCorte { get; set; }
    public List<Candidato> Selecionados { get; set; }
    public FilaFlexivel FilaEspera { get; set; }

    public Curso(int codigo, string nome, int vagas)
    {
        Codigo = codigo;
        Nome = nome;
        Vagas = vagas;
        Resetar();
    }
    
    public void Resetar()
    {
        Selecionados = new List<Candidato>();
        FilaEspera = new FilaFlexivel();
        NotaCorte = 0;
    }

    public void CalcularNotaCorte()
    {
        if (Selecionados.Count > 0)
        {
            Selecionados.Sort();
            NotaCorte = Selecionados.Last().NotaMedia;
        }
    }
}


public class FilaFlexivel
{
    private class Celula
    {
        public Candidato Elemento { get; }
        public Celula Proximo { get; set; }
        public Celula(Candidato elemento) { Elemento = elemento; Proximo = null; }
    }

    private Celula primeiro, ultimo;

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
    
    public bool EstaVazia() => primeiro == ultimo;

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
        if (lista.Count <= 1) return;
        int meio = lista.Count / 2;
        var esquerda = new List<Candidato>(lista.GetRange(0, meio));
        var direita = new List<Candidato>(lista.GetRange(meio, lista.Count - meio));
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
                original[iOrig++] = esquerda[iEsq++];
            else
                original[iOrig++] = direita[iDir++];
        }
        while (iEsq < esquerda.Count) original[iOrig++] = esquerda[iEsq++];
        while (iDir < direita.Count) original[iOrig++] = direita[iDir++];
    }
}