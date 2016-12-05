using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Showcase.GrainInterfaces;

namespace Showcase.Grains
{
    public class ShowcaseSet : Grain<ShowcaseSetInfo>, IShowcaseSet
    {
        public Task<IEnumerable<PointPropertyInfo>> GetPointProperties()
        {
            return Task.FromResult(State.PointProperties.AsEnumerable());
        }

        public Task<PointPropertyInfo> RegisterPointProperty(string propertyName, Type propertyType)
        {
            var pi = State.PointProperties.FirstOrDefault(p => p.PropertyName == propertyName);
            if (pi != null)
            {
                if(pi.PropertyType != propertyType)
                    throw new ApplicationException($"Already registered property {propertyName} with type {pi.PropertyType}");
                return Task.FromResult(pi);
            }
            State.PointProperties.Add(pi = new PointPropertyInfo(propertyName, propertyType));
            return Task.FromResult(pi);
        }
    }
}