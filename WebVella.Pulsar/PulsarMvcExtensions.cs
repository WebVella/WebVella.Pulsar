using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using WebVella.Pulsar.Services;

namespace WebVella.Pulsar
{
	public static class PulsarMvcExtensions
	{
		public static IServiceCollection AddPulsar(this IServiceCollection services)
		{
			services.AddScoped<JsService>();
			return services;
		}
	}
}
