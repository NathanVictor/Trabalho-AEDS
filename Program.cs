using System;
using System.Collections.Generic;
using System.Reflection;

class Program
{
    // Variáveis para armazenar os dados carregados
    public static void Main()
    {

        bool executado = true;

        do
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("   Gerenciador de Processo Seletivo - UniStark   ");
            Console.WriteLine("=================================================");
            Console.WriteLine("1. Carregar Arquivo de Entrada (entrada.txt)");
            Console.WriteLine("2. Processar Dados dos Candidatos");
            Console.WriteLine("3. Exibir Resultados por Curso");
            Console.WriteLine("4. Gerar Arquivo de Saída (saida.txt)");
            Console.WriteLine("5. Sair");
            Console.WriteLine("=================================================");
            Console.Write("Digite a sua opção: ");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.WriteLine("\n [Opção 1 selecionada: Carregar arquivo de Entrada]");
                    // Inserir lógica para ler o arquivo 'entrada.txt'.
                    // Necessário ler os dados dos cursos e dos candidatos.
                    // Armazenar os cursos em um dicionário e os candidatos em uma Lista.
                    Console.WriteLine("Lógica de carregamento de arquivo a ser implementada.\n");
                    break;

                case "2":
                    Console.WriteLine("\n[Opção 2 selecionada: Processar Dados]");
                    // Inserir aqui a lógica de processamento.
                    // 1. Calcular a média de cada candidato.
                    // 2. Ordenar a lista de candidatos por nota (e redação como desempate).
                    // 3. Alocar os cancidatos nas vagas (1ª e 2ª opção) e filas de espera.
                    Console.WriteLine("Lógica de processamento dos dados a ser implementada.\n");
                    break; 

                case "3":
                    Console.WriteLine("\n [Opção 3 selecionada: Exibir Resultados]");
                    // Inserir a lógica para exibir os resultados no console.
                    // Iterar sobre cada curso e mostrar:
                    // Nome do curso e nota de corte.
                    // Lista de selecionados em ordem decrescente.
                    // fila de espera em ordem decrescente.
                    Console.WriteLine("lógica de exibição de resultados a ser implementada.\n");
                    break;

                case "4":
                      Console.WriteLine("\n[Opção 4 selecionada: Gerar Arquivo de Saída]");
                    // Inserir aqui a lógica para criar o 'saida.txt'.
                    // O formato do arquivo de saída deve seguir estritamente o especificado no documento. 
                    Console.WriteLine("Lógica de geração de arquivo a ser implementada.\n");
                    break;

                case "5":
                    Console.WriteLine("\nSaindo do programa...");
                    executado = false;
                    break;

                default:
                    Console.WriteLine("\n Opção inválida! Por favor, tente novamente!");
                    break;

            }
            if (executado)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        } while (executado);
    }
}
