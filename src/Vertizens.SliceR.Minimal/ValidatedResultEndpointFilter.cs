using Microsoft.AspNetCore.Http;
using Vertizens.SliceR.Validated;
using System.Reflection;

namespace Vertizens.SliceR.Minimal;
internal class ValidatedResultEndpointFilter : IEndpointFilter
{
    private static readonly MethodInfo _toResultMethod = typeof(ValidatedResultEndpointFilter).GetMethod(nameof(ToHttpResult), BindingFlags.NonPublic | BindingFlags.Static)!;
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result != null)
        {
            if (result is ValidatedResult validateResult)
            {
                var resultType = result.GetType();
                if (resultType.IsGenericType)
                {
                    var toResultTypedMethod = _toResultMethod.GetGenericMethodDefinition().MakeGenericMethod(resultType.GetGenericArguments());
                    result = toResultTypedMethod.Invoke(null, [result]);
                }
                else
                {
                    result = validateResult.ToHttpResult();
                }
            }
        }

        return result;
    }

    private static IResult ToHttpResult<TResult>(ValidatedResult<TResult> validatedResult)
    {
        return validatedResult.ToHttpResult();
    }
}
