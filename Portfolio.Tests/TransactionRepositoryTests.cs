using Xunit;
using Portfolio.Api.Domain.Services;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Enums;
using Portfolio.Api.Domain.Exceptions;
using Portfolio.Api.Domain.Repositories;

namespace Portfolio.Tests;
public class TransactionRepositoryTests     
{
[Fact]
public void Add_Throws_When_Duplicate_Sequence_For_Same_Portfolio_And_Date()
{
    var repo = new TransactionRepository();
    var portfolioId = Guid.NewGuid();
    var tradeDate = new DateTime(2024, 01, 01);

    var t1 = Transaction.Create(
        portfolioId, "AAPL", TransactionType.Buy,
        10, 100, tradeDate, sequence: 1
    );

    var t2 = Transaction.Create(
        portfolioId, "AAPL", TransactionType.Buy,
        5, 110, tradeDate, sequence: 1
    );

    repo.Add(t1);

    Assert.Throws<DomainException>(() => repo.Add(t2));
}
}