namespace Portfolio.Api.Domain.Entities;

public class Portfolio
{
    public Guid Id {get; private set;}
    public string Name {get; private set;}
    public string BaseCurrency {get; private set;}
    public DateTime CreatedAt {get; private set;}

    private Portfolio(){ }
    public Portfolio(string name, string baseCurrency){
        Id = Guid.NewGuid();
        Name = name;
        BaseCurrency = baseCurrency;
        CreatedAt = DateTime.UtcNow;
    }
}
