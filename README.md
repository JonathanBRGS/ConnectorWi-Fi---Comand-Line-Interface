# ConnectorWi-Fi - Comand Line Interface

Este projeto é um exemplo de um ataque de força bruta simplificado para descobrir a senha de uma rede Wi-Fi.
O programa tenta diferentes combinações de senhas com base em um conjunto de caracteres definidos e verifica se o computador conseguiu conectar-se à
internet através do Wi-Fi.

Atenção: Este software foi criado para fins educacionais e não deve ser utilizado para acessar redes sem permissão. O uso inadequado deste código pode
violar a lei e resultar em consequências legais.

# Funcionalidades:

- Gera combinações de senhas com comprimento variável (de 8 a 12 caracteres).
- Usa caracteres alfanuméricos (a-z e 0-9) para as tentativas de senha.
- Conecta-se à rede Wi-Fi fornecida pelo usuário.
- Verifica a conexão à internet usando o comando de Ping.
- Interrompe as tentativas de senha assim que a conexão à internet for bem-sucedida.

# Pré-requisitos:

- Windows: Este código utiliza o comando netsh, que é específico para sistemas operacionais Windows.
- .NET SDK: O código está escrito em C# e requer o .NET SDK instalado para ser executado.
- Permissões de Administrador: É necessário executar o programa como administrador para poder modificar perfis de rede Wi-Fi e executar os comandos do
netsh.

# Estrutura do código:

# Funções principais:

- GenerateCombinations: Gera todas as combinações possíveis de caracteres alfanuméricos com o comprimento especificado.
- CreateWiFiProfile: Cria um arquivo de perfil de Wi-Fi com o SSID e a senha fornecida.
- ConnectToWiFi: Executa o comando para conectar-se à rede Wi-Fi usando o perfil criado.
- IsConnectedToInternet: Verifica se o computador está conectado à internet fazendo um ping para o Google.
- ExecuteCommand: Executa comandos no prompt de comando (cmd.exe) e retorna a saída.

# Considerações legais:

Este software foi criado para uso educacional e demonstrativo. A invasão de redes Wi-Fi sem permissão é ilegal. Use o software de maneira ética e apenas
em redes para as quais você tenha autorização.

# Contribuições:

Sinta-se à vontade para fazer um fork do projeto, abrir issues para bugs ou solicitar novas funcionalidades, e enviar pull requests para melhorias no
código.

# Licença:

Este projeto é distribuído sob a licença MIT.