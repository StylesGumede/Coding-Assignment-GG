using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MessageSimulator.Core.Infrustructure;
using MessageSimulator.Core.Infrustructure.Configuration;
using MessageSimulator.Core.Infrustructure.Data;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageSimulator.Core.Data.Factories
{
    /// <summary>
    /// Creates instances that implement the <see cref="IDataAccess"/> interface.
    /// </summary>
    public class DataFactory : IDataFactory
    {
        private readonly IInfrustructureFactory _infrustructureFactory;

        public DataFactory(IInfrustructureFactory infrustructureFactory)
        {
            this._infrustructureFactory = infrustructureFactory;
        }

        public T CreateInstanceOf<T>() where T : class
        {
            IEnumerable<Type> dataGatewayTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IDataAccess)));

            Type dataAcessType = dataGatewayTypes.FirstOrDefault(x => x.GetInterfaces().Contains(typeof(T)));

            if (dataAcessType != null)
                return Activator.CreateInstance(dataAcessType,
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>()) as T;

            throw new Exception($"'{typeof(T).Name}' is not registered with '{nameof(DataFactory)}'");
        }
    }
}