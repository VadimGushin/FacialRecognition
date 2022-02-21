using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Model
{
    public interface ITokens
    {
        Task<Token> WithTokenValue(string tokenValue);
        Task<bool> IsValid(string tokenValue);
        void InvalidateToken(Token existentToken);
        Task CreateTokenFor(Invite existentInvite);
    }
}
