﻿using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using TestWeb.Services;
using TestWeb.Settings;

namespace TestWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
	    public IContainer ApplicationContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
	        // Create the container builder.
	        var builder = new ContainerBuilder();

	        // Register dependencies, populate the services from
	        // the collection, and build the container. If you want
	        // to dispose of the container at the end of the app,
	        // be sure to keep a reference to it as a property or field.
	        builder.RegisterType<TestServiceProxy>().SingleInstance();
	        builder.RegisterType<SettingsHolder>().SingleInstance();

			builder.Populate(services);
	        this.ApplicationContainer = builder.Build();

	        // Create the IServiceProvider based on the container.
	        return new AutofacServiceProvider(this.ApplicationContainer);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
			IHostingEnvironment env, 
			ILoggerFactory loggerFactory,
		    IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

	        // If you want to dispose of resources that have been resolved in the
	        // application container, register for the "ApplicationStopped" event.
	        appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
		}
    }
}
