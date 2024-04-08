using FluentValidation;
using Invoicing.AutoMappers;
using Invoicing.DTOs;
using Invoicing.Middleware;
using Invoicing.Models;
using Invoicing.Repository;
using Invoicing.Services;
using Invoicing.Validators;
using Microsoft.EntityFrameworkCore;

namespace Invoicing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // Services Injection
            builder.Services.AddKeyedScoped<IClientService<ClientDTO, ClientInsertDTO, ClientUpdateDTO>, ClientService>("ClientService");
            builder.Services.AddKeyedScoped<IInvoiceService<InvoiceDTO, InvoiceInsertDTO>, InvoiceService>("InvoiceService");
            builder.Services.AddKeyedScoped<ICommonService<ProductDTO, ProductInsertDTO, ProductUpdateDTO>, ProductService>("ProductService");
            builder.Services.AddKeyedScoped<IItemService<ItemDTO, ItemInsertDTO>, ItemService>("ItemService");

            // Repository Injection
            builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
            builder.Services.AddScoped<IClientRepository<Client>, ClientRepository>();
            builder.Services.AddScoped<IInvoiceRepository<Invoice>, InvoiceRepository>();
            builder.Services.AddScoped<IItemRepository<Item>, ItemRepository>();

            // Inyectando Contexto de Bases de datos Entity Framework
            builder.Services.AddDbContext<InvoiceContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceConnection"));
            });

            // Validators
            builder.Services.AddScoped<IValidator<ClientInsertDTO>, ClientInsertValidator>();
            builder.Services.AddScoped<IValidator<ClientUpdateDTO>, ClientUpdateValidator>();
            builder.Services.AddScoped<IValidator<InvoiceInsertDTO>, InvoiceInsertValidator>();
            builder.Services.AddScoped<IValidator<ProductInsertDTO>, ProductInsertValidator>();
            builder.Services.AddScoped<IValidator<ProductUpdateDTO>, ProductUpdateValidator>();
            builder.Services.AddScoped<IValidator<ItemInsertDTO>, ItemInsertValidator>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Middleware
            app.UseMiddleware<ApiKeyMiddleware>();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            //Create a new scope to retrieve scoped services
            using (var scope = app.Services.CreateScope())
            {
                // Get the DbContext instance
                var dbContext = scope.ServiceProvider.GetRequiredService<InvoiceContext>();

                // Apply migrations
                dbContext.Database.Migrate();
            }

            app.Run();
        }
    }
}
