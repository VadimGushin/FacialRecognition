using System;
using System.Linq.Expressions;

namespace Langate.FacialRecognition.Mobile.Heplers
{
    public class Common
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyDelegate)
        {
            var expression = (MemberExpression)propertyDelegate.Body;
            return expression.Member.Name;
        }
    }
}
