using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MessageSimulator.Core.Infrustructure
{
    public class InfrustructureFactory: IInfrustructureFactory
    {
        public T CreateInstanceOf<T>() where T : class
        {
            IEnumerable<Type> dataGatewayTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IInfrustructureType)));

            Type dataAcessType = dataGatewayTypes.FirstOrDefault(x => x.GetInterfaces().Contains(typeof(T)));

            if (dataAcessType != null)
                return Activator.CreateInstance(dataAcessType) as T;

            throw new Exception($"'{typeof(T).Name}' is not registered with '{this.GetType().Name}'");
        }
    }
}
