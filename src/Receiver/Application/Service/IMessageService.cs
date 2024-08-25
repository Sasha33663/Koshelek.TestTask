using Domain;
using GrpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service;
public interface IMessageService
{
    Task<Domain.Message> CreateMessageAsync(int number, string text, CancellationToken token);
    Task <GetHistoryResponse> GetHistoryAsync();
}
