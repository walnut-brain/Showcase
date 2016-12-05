using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;
using static LanguageExt.Prelude;
using static WalnutBrain.Showcase.Matcher;

namespace WalnutBrain.Showcase
{
    internal static class Tools
    {
        private static bool IsPropertySelector(MemberExpression me)
        {
            return (me.Expression is ConstantExpression || me.Expression is ParameterExpression || me.Expression == null) &&
                   me.Member is PropertyInfo;
        }

        public static Option<PropertyInfo> TryGetPropertyInfo(Expression ex)
        {
            return
                match(ex).As<Option<PropertyInfo>>()
                    .Is<LambdaExpression>(le => TryGetPropertyInfo(le.Body))
                    .Is<MemberExpression>(
                        me => IsPropertySelector(me) ? Some((PropertyInfo)me.Member) : None)
                    .Else(None);
        }
    }
}