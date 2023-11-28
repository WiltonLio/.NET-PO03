using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<(int, string, int, double)> estoque = new List<(int, string, int, double)>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Cadastrar Produto");
            Console.WriteLine("2. Consultar Produto por Código");
            Console.WriteLine("3. Atualizar Estoque");
            Console.WriteLine("4. Gerar Relatórios");
            Console.WriteLine("5. Sair");

            Console.Write("Escolha uma opção: ");
            string escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    CadastrarProduto();
                    break;
                case "2":
                    ConsultarProdutoPorCodigo();
                    break;
                case "3":
                    AtualizarEstoque();
                    break;
                case "4":
                    GerarRelatorios();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static void CadastrarProduto()
    {
        try
        {
            Console.Write("Código do Produto: ");
            int codigo = int.Parse(Console.ReadLine());

            Console.Write("Nome do Produto: ");
            string nome = Console.ReadLine();

            Console.Write("Quantidade em Estoque: ");
            int quantidade = int.Parse(Console.ReadLine());

            Console.Write("Preço Unitário: ");
            double preco = double.Parse(Console.ReadLine());

            estoque.Add((codigo, nome, quantidade, preco));

            Console.WriteLine("Produto cadastrado com sucesso!");
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro: Entrada inválida. Certifique-se de inserir valores numéricos corretos.");
        }
    }

    static void ConsultarProdutoPorCodigo()
    {
        try
        {
            Console.Write("Digite o Código do Produto: ");
            int codigo = int.Parse(Console.ReadLine());

            var produto = estoque.FirstOrDefault(p => p.Item1 == codigo);

            if (produto.Equals(default((int, string, int, double))))
            {
                throw new ProdutoNaoEncontradoException("Produto não encontrado.");
            }

            Console.WriteLine($"Nome: {produto.Item2}, Quantidade: {produto.Item3}, Preço: {produto.Item4:C}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro: Entrada inválida. Certifique-se de inserir um valor numérico correto.");
        }
        catch (ProdutoNaoEncontradoException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    static void AtualizarEstoque()
    {
        try
        {
            Console.Write("Digite o Código do Produto: ");
            int codigo = int.Parse(Console.ReadLine());

            var produto = estoque.FirstOrDefault(p => p.Item1 == codigo);

            if (produto.Equals(default((int, string, int, double))))
            {
                throw new ProdutoNaoEncontradoException("Produto não encontrado.");
            }

            Console.Write("Digite a Quantidade (negativa para saída, positiva para entrada): ");
            int quantidade = int.Parse(Console.ReadLine());

            if (produto.Item3 + quantidade < 0)
            {
                throw new EstoqueInsuficienteException("Quantidade em estoque insuficiente para a saída.");
            }

            estoque.Remove(produto);
            estoque.Add((produto.Item1, produto.Item2, produto.Item3 + quantidade, produto.Item4));

            Console.WriteLine("Estoque atualizado com sucesso!");
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro: Entrada inválida. Certifique-se de inserir um valor numérico correto.");
        }
        catch (ProdutoNaoEncontradoException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        catch (EstoqueInsuficienteException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    static void GerarRelatorios()
    {
        Console.Write("Informe o Limite de Quantidade para o Relatório 1: ");
        int limiteQuantidade = int.Parse(Console.ReadLine());

        var relatorio1 = estoque.Where(p => p.Item3 < limiteQuantidade);
        ImprimirRelatorio("Relatório 1: Produtos com Quantidade Abaixo do Limite", relatorio1);

        Console.Write("Informe o Valor Mínimo para o Relatório 2: ");
        double valorMinimo = double.Parse(Console.ReadLine());

        Console.Write("Informe o Valor Máximo para o Relatório 2: ");
        double valorMaximo = double.Parse(Console.ReadLine());

        var relatorio2 = estoque.Where(p => p.Item4 >= valorMinimo && p.Item4 <= valorMaximo);
        ImprimirRelatorio("Relatório 2: Produtos com Valor Entre Mínimo e Máximo", relatorio2);

        var relatorio3 = estoque.Select(p => new
        {
            Nome = p.Item2,
            ValorTotal = p.Item3 * p.Item4
        });

        ImprimirRelatorio("Relatório 3: Valor Total do Estoque e Valor Total por Produto", relatorio3);
    }

    static void ImprimirRelatorio<T>(string titulo, IEnumerable<T> relatorio)
    {
        Console.WriteLine(titulo);
        foreach (var item in relatorio)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }
}

class ProdutoNaoEncontradoException : Exception
{
    public ProdutoNaoEncontradoException(string message) : base(message) { }
}

class EstoqueInsuficienteException : Exception
{
    public EstoqueInsuficienteException(string message) : base(message) { }
}
