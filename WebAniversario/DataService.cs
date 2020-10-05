using Microsoft.EntityFrameworkCore;
using WebAniversario.Data;

namespace WebAniversario
{
    class DataService : IDataService
    {
        private readonly WebAniversarioContext contexto;

        public DataService(WebAniversarioContext contexto)
        {
            this.contexto = contexto;
        }
        // cofiguracao necessaria para geracao de tabela caso ela nao exista ainda
        // EnsureCreatedeste método não utiliza migrações. Sendo assim,
        //como ele cria o banco de dados? Verificando se o banco ainda não existe, em caso positivo, 
        //ele procura o nosso modelo e mapeamento, para poder fazer um esquema, e então gerar o banco
        //de dados do SQL Server.
        //Contudo, uma vez que utilizamos o método EnsureCreated(), não podemos mais aplicar nenhuma migração no sistema
        //Portanto, o recomendável é utilizar, em seu lugar, o método Migrate(),
        //que faz a mesma coisa, só que utilizando as migrações.
        public void InicializaDB()
        {
            contexto.Database.Migrate();
        }
    }
}

