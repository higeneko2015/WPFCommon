using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WPFCommon
{
    public class ServiceManager
    {
        private IServiceProvider Service { get; set; }

        private ServiceCollection ServiceList { get; }

        public ServiceManager()
        {
            this.ServiceList = new ServiceCollection();
        }

        public void AddSingleton<T1, T2>()
        {
            this.ServiceList.AddSingleton(typeof(T1), typeof(T2));
        }

        public void AddSingleton<T1>()
        {
            this.ServiceList.AddSingleton(typeof(T1));
        }

        public void AddTransient<T1, T2>()
        {
            this.ServiceList.AddTransient(typeof(T1), typeof(T2));
        }

        public void AddTransient<T1>()
        {
            this.ServiceList.AddTransient(typeof(T1));
        }

        public void Build()
        {
            this.Service = this.ServiceList.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return this.Service.GetRequiredService<T>();
        }

        public void ReplaceService<T1, T2>(ServiceLifetime type)
        {
            var descriptor = new ServiceDescriptor(typeof(T1), typeof(T2), type);
            this.ServiceList.Replace(descriptor);
        }

        public IServiceProvider GetProvider()
        {
            return Service;
        }
    }
}