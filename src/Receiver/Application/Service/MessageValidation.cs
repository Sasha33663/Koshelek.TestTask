using Application.Exeptions;

namespace Application.Service;

public class MessageValidation
{
    public async Task NumberValidateAsync(int count)
    {
        if (count > 1)
        {
            throw new NumberException();
        }
    }
    public async Task TextValidateAsync(string text)
    {
        if (text.Length>=128)
        {
            throw new TextException();
        }
    }
}