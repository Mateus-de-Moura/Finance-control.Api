 # Baixar imagem  rabbitMq  e rodar no docker

<!-- Apos baixar o  docker desktop e efetuar login, execute o comando abaixo no terminal para baixar a imagem do rabbitMq -->

docker pull rabbitmq:management

<!-- Apos baixar a imagem, rode o comando abaixo  para subir o container -->

docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management


<!-- Acessar a interface de administração -->

http://localhost:15672

teste
