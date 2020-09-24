# bancodigital



Para executar o projeto no Docker:

Requisitos:
	- Docker Desktop
	
1. Acessar a pasta que contém o arquivo *.sln do projeto e executar o comando:
	
	docker-compose up -d
		
2. Após finalizado o comando anterior, abra o browser e acesse a seguinte URL:
		
	https://localhost:10001/ui/playground
	
Para executar pelo Visual Studio:

Requisitos:
	- Visual Studio 2019
	- Servidor MongoDB 

	
	1- Abra a solution
	2- No projeto BancoDigital.API, alterar a entrada "BancoDigitalConnection" no arquivo de configuração "appsettings.json" para apontar para o servidor de MongoDB que estiver disponível na rede.