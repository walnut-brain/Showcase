using System;

namespace Showcase.GrainInterfaces
{
    public class PointPropertyInfo
    {
        public PointPropertyInfo(string propertyName, Type propertyType)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        public string PropertyName { get; }
        public Type PropertyType { get; }
    }
}