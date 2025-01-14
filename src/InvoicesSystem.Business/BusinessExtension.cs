using InvoicesSystem.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using InvoicesSystem.Data.Repository.Interface;
using InvoiceSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoicesSystem
{
    public static class BusinessExtension
	{
		public static IServiceCollection RegisterRepositories(this IServiceCollection services) {
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            return services;
		}

        public static IServiceCollection AddBusinessDbContext(this IServiceCollection services, string connectionString)
		{
            services.AddDbContext<InvoiceDbContext>(options =>
						options.UseSqlServer(connectionString,
						 b => b.MigrationsAssembly("InvoicesSystem.Data")));
			return services;
		}
	}
}