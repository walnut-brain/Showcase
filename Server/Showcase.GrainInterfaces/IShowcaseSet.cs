using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Showcase.GrainInterfaces
{
    public interface IShowcaseSet : IGrainWithStringKey
    {
        Task<IEnumerable<PointPropertyInfo>> GetPointProperties();
        Task<PointPropertyInfo> RegisterPointProperty(string propertyName, Type propertyType);
    }
}