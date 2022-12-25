using AutoMapper;
using Quotes.Models;
using Quotes.DTO.Responses;
using Quotes.DTO.Requests.Other;
using Quotes.DTO.Requests.User;

namespace Quotes.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<RegisterRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();

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
