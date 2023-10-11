# Como fazer deploy em nuvem do projeto

Alterar a connection string no arquivo [appsettings.json](https://github.com/JulioFretes/ProductList/blob/master/ProductList/appsettings.json), para a connection string que será utilizada.

Dar deploy da aplicação em sua provedora de nuvem escolhida, no meu caso vou utilizar a azure de exemplo.

No caso da azure iremos utilizar [Web App](https://portal.azure.com/#create/Microsoft.WebSite).

Unico ponto importante no 1° passo da criação do Web App é selecionar a opção "Código" na seção Detalhes de Instância e após isso selecionar o ".NET 7".

Na parte de implementação selecionar implementação continua e após isso selecionar o repositorio que está o código.

Após isso pode ser finalizada a criação do Web App.

No repositorio do github selecionado será criado um arquivo ".yml", como [esse](https://github.com/JulioFretes/ProductList/blob/master/.github/workflows/master_productlistcp02.yml).

A unica alteração que precisará ser feita nele será adicionar duas etapas do build, que serão:

\- name: Install EF Core tools
        run: dotnet tool install --global dotnet-ef

\- name: Run EF Core migrations
        run: dotnet ef database update --project ${{ github.workspace }}\ProductList\ProductList.csproj --startup-project ${{ github.workspace }}\ProductList\ProductList.csproj --context ListContext

elas ficarão entre o build e o publish.

Essas duas etapas serão para criar a estrutura do banco de dados.

Após isso será apenas executar o workflow.
