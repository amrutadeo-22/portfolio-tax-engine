using System.Linq;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Enums;
using Portfolio.Api.Domain.Exceptions;
namespace Portfolio.Api.Domain.Services;

public class FifoAccountingService
{
    public FifoResult Calculate(IEnumerable<Transaction> transactions){
        var result = new FifoResult();
        var PortfolioGroups = transactions.OrderBy(t => t.TradeDate).ThenBy(t => t.Sequence)
                            .GroupBy(t => t.PortfolioId);
        var portfolioIds = transactions
            .Select(t => t.PortfolioId)
            .Distinct()
            .ToList();

        if (portfolioIds.Count == 0)
            throw new DomainException("No transactions provided.");
        if (transactions.GroupBy(t => new { t.PortfolioId, t.AssetSymbol, t.TradeDate, t.Sequence })
            .Any(g => g.Count() > 1))
        {
            throw new DomainException("Duplicate sequence detected.");
        }

        foreach (var portfolioGroup in PortfolioGroups){
            if (!result.RemainingHoldingsPerPortfolio.ContainsKey(portfolioGroup.Key))
            {
                result.RemainingHoldingsPerPortfolio[portfolioGroup.Key] = new();
            }
            var assetGroups = portfolioGroup.GroupBy(t => t.AssetSymbol);
            
            foreach(var assetGroup in assetGroups){
                var taxLots = new Queue<TaxLot>();

                foreach(var transaction in assetGroup){
                    if(transaction.Type == TransactionType.Buy){
                        taxLots.Enqueue(new TaxLot(
                            transaction.AssetSymbol,
                            transaction.Quantity,
                            transaction.Price,
                            transaction.TradeDate
                        ));
                    }
                    else{
                        ProcessSell(transaction, taxLots, result);
                    }

                    
                }
                var remainingQty = taxLots.Sum(l => l.QuantityRemaining);
                if (remainingQty > 0)
                {
                    result.SetRemaining(
                        portfolioGroup.Key,
                        assetGroup.Key,
                        remainingQty
                    );
                }
            }
            
        }
        return result;

    }
    private static void ProcessSell(
        Transaction transaction,
        Queue<TaxLot> taxLots,
        FifoResult result
    ){
        var sellQuantity = transaction.Quantity;
        while(sellQuantity > 0){
            if(taxLots.Count == 0){
                throw new DomainException(
                    $"Oversell detected for asset {transaction.AssetSymbol} in portfolio {transaction.PortfolioId}"
                );
            }
            var currentLot = taxLots.Peek();
            var qtyToConsume = Math.Min(sellQuantity, currentLot.QuantityRemaining);

            var pnl = (transaction.Price - currentLot.CostPrice) * qtyToConsume;
            result.AddRealizedPnL(transaction.PortfolioId, transaction.AssetSymbol, pnl);
            
            currentLot.Consume(qtyToConsume);
            sellQuantity -= qtyToConsume;

            if (currentLot.QuantityRemaining == 0)
                taxLots.Dequeue();
        }
    }
}        
        

        
    