using System.Net.Sockets;
using System.Net;
using System.Text;

namespace pap.Conection;

public class WifiConection
{
    private TcpClient? tcpCliente;
    private IPAddress? enderecoIP;
    private StreamWriter? stwEnviar;
    private StreamReader? strReceber;
    private string receber = string.Empty;

    // Função para conectar ao servidor (Arduino)
    public void Conected(string EnderecoIP, Int16 Porta)
    {
        try
        {
            enderecoIP = IPAddress.Parse(EnderecoIP);
            tcpCliente = new TcpClient();
            tcpCliente.Connect(enderecoIP, Porta);

            stwEnviar = new StreamWriter(tcpCliente.GetStream());
            strReceber = new StreamReader(tcpCliente.GetStream());

            stwEnviar.Write('s'); // Envia uma mensagem inicial de conexão
            stwEnviar.Flush();

            Console.WriteLine("Conectado com sucesso ao Arduino");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na conexão: {ex.Message}");
        }
    }

    // Função para desconectar
    public void Desconected()
    {
        try
        {
            if (stwEnviar != null) stwEnviar.Close();
            if (strReceber != null) strReceber.Close();
            if (tcpCliente != null) tcpCliente.Close();
        }
        catch (Exception ex)
        {
           
        }
    }

    // Função para enviar dados
    public void Send(string sendString)
    {
        try
        {
            if (tcpCliente != null && tcpCliente.Connected)
            {
                stwEnviar!.Write(sendString);
                stwEnviar.Flush();
            }
            else
            {
                
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    // Função para ler dados recebidos
    public string? Read()
    {
        try
        {
            if (tcpCliente != null && tcpCliente.Connected)
            {
                // Verifica se há dados disponíveis para leitura antes de chamar Read()
                if (tcpCliente.GetStream().DataAvailable)
                {
                    // Usando ReadLine() ou ReadToEnd(), dependendo do que o Arduino enviar
                    StringBuilder recebidos = new StringBuilder();
                    while (tcpCliente.GetStream().DataAvailable)
                    {
                        char c = (char)strReceber!.Read(); // Lê um caractere de cada vez
                        recebidos.Append(c); // Adiciona o caractere ao StringBuilder
                    }
                    return recebidos.ToString(); // Retorna os dados recebidos
                }
                else
                {
                    return null; // Sem dados disponíveis
                }
            }
            else
            {
                Console.WriteLine("Erro: Não conectado ao Arduino.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler dados: {ex.Message}");
            return null;
        }
    }

    // Verificação do estado de conexão
    public bool IsConected()
    {
        return tcpCliente != null && tcpCliente.Connected;
    }
}