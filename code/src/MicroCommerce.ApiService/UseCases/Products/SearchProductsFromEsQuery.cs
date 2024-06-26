using System.Text.Json;
using Elastic.Clients.Elasticsearch;
using MediatR;
using MicroCommerce.ApiService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MicroCommerce.ApiService.UseCases.Products;

public record SearchProductsFromEsQuery : IRequest<IEnumerable<SearchProductFromEsItemResponse>>
{
    public string SearchTerm { get; init; } = "";
    
    public static Func<IMediator, Task<IEnumerable<SearchProductFromEsItemResponse>>> EndpointHandler => (mediator) => mediator.Send(new SearchProductsFromEsQuery());
}

public class GetProductFromEsQueryHandler(ILogger<GetProductFromEsQueryHandler> logger, ApplicationDbContext context, ElasticsearchClient esClient) : IRequestHandler<SearchProductsFromEsQuery, IEnumerable<SearchProductFromEsItemResponse>>
{
    public async Task<IEnumerable<SearchProductFromEsItemResponse>> Handle(SearchProductsFromEsQuery request, CancellationToken cancellationToken)
    {
        var esRequest = new SearchRequest(ElasticSearchIndexKey.Product.Alias)
        {
            From = 0,
            Size = 1000,
        };
        
        logger.LogInformation("Query ES with request {Request}", JsonSerializer.Serialize(esRequest));
        var response = await esClient.SearchAsync<ProductDocument>(esRequest, cancellationToken);
        
        if (!response.IsValidResponse)
        {
            throw new Exception("Error: " + response.ElasticsearchServerError);
        }

        if (response.Documents.Count == 0)
        {
            return new List<SearchProductFromEsItemResponse>();
        }

        var ids = response.Documents.Select(s => s.Id).ToList();

        var products = await context.Products
            .Where(s => ids.Contains(s.Id))
            .ToListAsync(cancellationToken);

        return products.ConvertAll(s => new SearchProductFromEsItemResponse
        {
            Id = s.Id,
            Name = s.Name,
            ImageUrl = s.ImageUrl,
            Price = s.Price,
        });
    }
}

public record SearchProductFromEsItemResponse
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public string ImageUrl { get; init; } = "";
    public decimal Price { get; init; }
}