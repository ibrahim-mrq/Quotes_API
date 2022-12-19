using AutoMapper;
using Azure.Core;
using Quotes.DTO.Requests;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Models;
using Quotes.Repositories.Interfaces;

namespace Quotes.Repositories.other
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _map;

        public FavoriteRepository(DBContext dBContext, IMapper map)
        {
            this._dbContext = dBContext;
            this._map = map;
        }

        public OperationType Add(AddFavoriteRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localUser = _dbContext.Users.Where(x => x.Id == request.UserId && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("User Id Not Found!", null);
            }
            var localQuote = _dbContext.Quotes.Where(x => x.Id == request.QuoteId && x.IsDelete == false).FirstOrDefault();
            if (localQuote == null)
            {
                return Constants.NotFoundResponse("Quote Id Not Found!", null);
            }

            var localItem = _dbContext.Favorites.Where(
                x => x.UserId == request.UserId &&
                x.QuoteId == request.QuoteId &&
                x.IsDelete == false).FirstOrDefault();

            if (localItem != null)
            {
                return Constants.NotFoundResponse("Favorite Id Already Exist!", null);
            }

            var currentItem = _map.Map<Favorite>(request);
            var favoriteResponse = _map.Map<FavoriteResponse>(currentItem);
            favoriteResponse.Quote = _map.Map<FavoriteQuoteResponse>(localQuote);
            favoriteResponse.IsFavorite = true;
            _dbContext.Favorites.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Add Favorite successfully", new { Favorite = favoriteResponse });
        }

        /*     public OperationType Add(AddFavoriteRequest request)
             {
                 if (!Constants.InputValidation(request).Status)
                 {
                     return Constants.InputValidation(request);
                 }
                 var localUser = _dbContext.Users.Where(x => x.Id == request.UserId && x.IsDelete == false).FirstOrDefault();
                 if (localUser == null)
                 {
                     return Constants.NotFoundResponse("User Id Not Found!", null);
                 }
                 var localQuote = _dbContext.Quotes.Where(x => x.Id == request.QuoteId && x.IsDelete == false).FirstOrDefault();
                 if (localQuote == null)
                 {
                     return Constants.NotFoundResponse("Quote Id Not Found!", null);
                 }

                 var localItem = _dbContext.Favorites.Where(x => x.UserId == request.UserId && x.QuoteId == request.QuoteId).FirstOrDefault();
                 if (localItem != null)
                 {
                     var message = "";
                     if (localItem.IsDelete == true)
                     {
                         localItem.IsDelete = false;
                         message = "Add Favorite successfully";
                     }
                     else
                     {
                         localItem.IsDelete = true;
                         message = "remove Favorite successfully";
                     }
                     var response = _map.Map<FavoriteResponse>(localItem);
                     response.IsFavorite = !localItem.IsDelete;
                     response.Quote = _map.Map<FavoriteQuoteResponse>(localQuote);
                     _dbContext.Favorites.Update(localItem);
                     _dbContext.SaveChanges();
                     return Constants.SuccessResponse(message, new { Favorite = response });
                 }

                 var currentItem = _map.Map<Favorite>(request);
                 var favoriteResponse = _map.Map<FavoriteResponse>(currentItem);
                 favoriteResponse.Quote = _map.Map<FavoriteQuoteResponse>(localQuote);
                 favoriteResponse.IsFavorite = true;
                 _dbContext.Favorites.Add(currentItem);
                 _dbContext.SaveChanges();
                 return Constants.SuccessResponse("Add Favorite successfully", new { Favorite = favoriteResponse });
             }*/

        public OperationType Delete(DeleteFavoriteRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localUser = _dbContext.Users.Where(x => x.Id == request.UserId && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("User Id Not Found!", null);
            }
            var localFavorite = _dbContext.Favorites.Where(x => x.Id == request.FavoriteId && x.IsDelete == false).FirstOrDefault();
            if (localFavorite == null)
            {
                return Constants.NotFoundResponse("Favorite Id Not Found!", null);
            }
            localFavorite.IsDelete = true;
            _dbContext.Favorites.Update(localFavorite);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Favorite Deleted successfully", new { Favorite = _map.Map<FavoriteResponse>(localFavorite) });
        }


        public OperationType GetByUserId(int UserId)
        {
            if (!Constants.InputValidation(UserId).Status)
            {
                return Constants.InputValidation(UserId);
            }
            var localUser = _dbContext.Users.Where(x => x.Id == UserId && x.IsDelete == false).FirstOrDefault();
            if (localUser == null)
            {
                return Constants.NotFoundResponse("User Id Not Found!", null);
            }
            var localList = _dbContext.Favorites.Where(x => x.UserId.Equals(UserId) && x.IsDelete == false).ToList();
            var filter = _map.Map<List<Favorite>, List<FavoriteResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Quotes = filter });
        }

        public OperationType GetByFavoriteId(int FavoriteId)
        {
            if (!Constants.InputValidation(FavoriteId).Status)
            {
                return Constants.InputValidation(FavoriteId);
            }
            var localList = _dbContext.Favorites.Where(x => x.Id.Equals(FavoriteId) && x.IsDelete == false).FirstOrDefault();
            return Constants.SuccessResponse("successfull", new { Favorites = _map.Map<FavoriteResponse>(localList) });
        }

        public OperationType Clear()
        {
            var list = _dbContext.Favorites.ToList();
            _dbContext.Favorites.RemoveRange(list);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("All Favorites have been successfully deleted!", null);
        }


    }
}
