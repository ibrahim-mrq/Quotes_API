using AutoMapper;
using Quotes.Models;
using Quotes.DTO.Requests;
using Quotes.DTO.Responses;

namespace Quotes.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<AddQuoteRequest, Quote>();
            CreateMap<Quote, QuoteResponse>();


            CreateMap<AddCategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();


            CreateMap<AddAuthorRequest, Author>();
            CreateMap<Author, AuthorResponse>();


            CreateMap<AddFavoriteRequest, Favorite>();
            CreateMap<Favorite, FavoriteResponse>();


            CreateMap<Quote, FavoriteQuoteResponse>();
        }
    }
}
