using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;


namespace ObjectNotation
{
    public static partial class FormatterExtensions
    {
        public static ArraySegment<Byte> GetStoreBytes(this JsonFormatter f)
        {
            return f.GetStore().Bytes;
        }

        public static void Key(this JsonFormatter f, string x)
        {
            f.Key(Utf8String.From(x));
        }

        public static void Value(this JsonFormatter f, IEnumerable<byte> raw, int count)
        {
            f.Value(new ArraySegment<byte>(raw.Take(count).ToArray()));
        }

        public static void Value(this JsonFormatter f, Byte[] bytes)
        {
            f.Value(new ArraySegment<Byte>(bytes));
        }

        static MethodInfo GetMethod<T>(Expression<Func<T>> expression)
        {
            var method = typeof(FormatterExtensions).GetMethod("Serialize");
            return method.MakeGenericMethod(typeof(T));
        }

        //
        // https://stackoverflow.com/questions/238765/given-a-type-expressiontype-memberaccess-how-do-i-get-the-field-value
        //
        // public static void KeyValue<T>(this JsonFormatter f, Expression<Func<T>> expression)
        // {
        //     // lambda body
        //     var lambdaBody = (MemberExpression)expression.Body;

        //     if (lambdaBody.Expression.NodeType == ExpressionType.Constant)
        //     {
        //         // 
        //         // KeyValue(() => Field);
        //         // 
        //         var constant = (ConstantExpression)lambdaBody.Expression;
        //         var field = (FieldInfo)lambdaBody.Member;
        //         var value = field.GetValue(constant.Value);
        //         if (value != null)
        //         {
        //             f.Key(lambdaBody.Member.Name);
        //             f.Serialize(value);
        //         }
        //     }
        //     else
        //     {
        //         // 
        //         // KeyValue(() => p.Field);
        //         // 
        //         var capture = (MemberExpression)lambdaBody.Expression;

        //         var captureVariable = (ConstantExpression)capture.Expression;
        //         var captureObj = captureVariable.Value;
        //         var captureField = (FieldInfo)capture.Member;
        //         var captureValue = captureField.GetValue(captureObj);

        //         var field = (FieldInfo)lambdaBody.Member;

        //         var value = field.GetValue(captureValue);
        //         if (!(value is null))
        //         {
        //             f.Key(field.Name);
        //             f.Serialize((T)value);
        //         }
        //     }
        // }
    }
}
