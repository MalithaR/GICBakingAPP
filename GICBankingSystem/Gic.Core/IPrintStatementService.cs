using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;

namespace GICBankingSystem.Gic.Core
{
    public interface IPrintStatementService
    {       
        Task GeneratePrintStatement(PrinStatementDTO prinStatementDTO);
    }
}
