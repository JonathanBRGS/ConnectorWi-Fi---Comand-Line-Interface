# ConnectorWi-Fi - Comand Line Interface

Este projeto � um exemplo de um ataque de for�a bruta simplificado para descobrir a senha de uma rede Wi-Fi.
O programa tenta diferentes combina��es de senhas com base em um conjunto de caracteres definidos e verifica se o computador conseguiu conectar-se �
internet atrav�s do Wi-Fi.

Aten��o: Este software foi criado para fins educacionais e n�o deve ser utilizado para acessar redes sem permiss�o. O uso inadequado deste c�digo pode
violar a lei e resultar em consequ�ncias legais.

# Funcionalidades:

- Gera combina��es de senhas com comprimento vari�vel (de 8 a 12 caracteres).
- Usa caracteres alfanum�ricos (a-z e 0-9) para as tentativas de senha.
- Conecta-se � rede Wi-Fi fornecida pelo usu�rio.
- Verifica a conex�o � internet usando o comando de Ping.
- Interrompe as tentativas de senha assim que a conex�o � internet for bem-sucedida.

# Pr�-requisitos:

- Windows: Este c�digo utiliza o comando netsh, que � espec�fico para sistemas operacionais Windows.
- .NET SDK: O c�digo est� escrito em C# e requer o .NET SDK instalado para ser executado.
- Permiss�es de Administrador: � necess�rio executar o programa como administrador para poder modificar perfis de rede Wi-Fi e executar os comandos do
netsh.

# Estrutura do c�digo:

# Fun��es principais:

- GenerateCombinations: Gera todas as combina��es poss�veis de caracteres alfanum�ricos com o comprimento especificado.
- CreateWiFiProfile: Cria um arquivo de perfil de Wi-Fi com o SSID e a senha fornecida.
- ConnectToWiFi: Executa o comando para conectar-se � rede Wi-Fi usando o perfil criado.
- IsConnectedToInternet: Verifica se o computador est� conectado � internet fazendo um ping para o Google.
- ExecuteCommand: Executa comandos no prompt de comando (cmd.exe) e retorna a sa�da.

# Considera��es legais:

Este software foi criado para uso educacional e demonstrativo. A invas�o de redes Wi-Fi sem permiss�o � ilegal. Use o software de maneira �tica e apenas
em redes para as quais voc� tenha autoriza��o.

# Contribui��es:

Sinta-se � vontade para fazer um fork do projeto, abrir issues para bugs ou solicitar novas funcionalidades, e enviar pull requests para melhorias no
c�digo.

# Licen�a:

Este projeto � distribu�do sob a licen�a MIT.