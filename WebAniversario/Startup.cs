using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WebAniversario.Data;
using WebAniversario.Repository;

namespace WebAniversario
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<WebAniversarioContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WebAniversarioContext")));

            //Para o serviço de injeção de dependência do ASP.NET Core funcionar,
            //é necessário registrar a nova classe, isto é, DataService, no contêiner de injeção de dependências.
            // Foi realizado registro chamando um método que adiciona uma instância,
            //a qual queremos que exista somente enquanto os objetos que a utilizarem estiverem ativos. 
            //Este método é AddTransient, que significa adicionar uma instância temporária, nesse caso para criar o banco de dados caso esse nao exista.
            // Normalmente, trabalhamos com interfaces para a injeção de independência, criei uma interface a partir da classe DataService
            services.AddTransient<IDataService, DataService>();

            services.AddTransient<IPessoaRepository, PessoaRepository>();
        }

        // IserviceProvider interface do asp.net para injetar a dependencia - para gerar uma tabela caso ela ainda nao exista
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //Para não poluirmos a classe Startup, foi criado uma classe DataService e dentro dela o metodo InicializaDB
            serviceProvider.GetService<IDataService>().InicializaDB();
        }
    }
}

