using System.Collections.Generic;
using Showcase.GrainInterfaces;

namespace Showcase.Grains
{
    public class ShowcaseSetInfo
    {
        public List<PointPropertyInfo> PointProperties { get; } = new List<PointPropertyInfo>();
    }
}