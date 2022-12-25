using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Quotes.DTO.Requests.Other;
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
            var localFavorite = _dbContext.Favorites.Where(x => x.UserId == request.UserId && x.QuoteId == request.QuoteId).FirstOrDefault();
            var favoriteResponse = new FavoriteResponse();
            var currentItem = new Favorite();
            var message = "";
            if (localFavorite != null)
            {
                if (localFavorite.IsDelete)
                {
                    localFavorite.IsDelete = false;
                    message = "Add Favorite successfully";
                }
                else
                {
                    localFavorite.IsDelete = true;
                    message = "Remove Favorite successfully";
                }
                _dbContext.Favorites.Update(localFavorite);
                favoriteResponse = _map.Map<FavoriteResponse>(localFavorite);
                favoriteResponse.Quote = _map.Map<FavoriteQuoteResponse>(localQuote);
                favoriteResponse.IsFavorite = !localFavorite.IsDelete;
                favoriteResponse.UpdatedAt = DateTime.Now.ToString(Constants.TYPE_DATE_TIME_FORMATER);
            }
            else
            {
                currentItem = _map.Map<Favorite>(request);
                favoriteResponse = _map.Map<FavoriteResponse>(currentItem);
                favoriteResponse.Quote = _map.Map<FavoriteQuoteResponse>(localQuote);
                favoriteResponse.IsFavorite = true;
                _dbContext.Favorites.Add(currentItem);
                message = "Add Favorite successfully";
            }
            _dbContext.SaveChanges();
            return Constants.SuccessResponse(message, new { Favorite = favoriteResponse });
        }

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
            var localListFavorites = _dbContext.Favorites.Where(x => x.UserId.Equals(UserId) && x.IsDelete == false).ToList();
            var locaalFavoritesResponse = _map.Map<List<Favorite>, List<FavoriteResponse>>(localListFavorites);
            var localListQuote = _dbContext.Quotes.Where(x => x.IsDelete == false).ToList();
            if (!localListQuote.IsNullOrEmpty())
            {
                var localList = locaalFavoritesResponse.Zip(localListQuote, (f, q) => new { favorit = f, quote = q });
                foreach (var item in localList)
                {
                    if (item.quote.Id == item.favorit.QuoteId)
                    {
                        item.favorit.Quote = _map.Map<FavoriteQuoteResponse>(item.quote);
                        item.favorit.IsFavorite = true;
                    }
                }
            }
            return Constants.SuccessResponse("successfull", new { favorites = locaalFavoritesResponse });
        }

        public OperationType GetByFavoriteId(int FavoriteId)
        {
            if (!Constants.InputValidation(FavoriteId).Status)
            {
                return Constants.InputValidation(FavoriteId);
            }
            var localFavorite = _dbContext.Favorites.Where(x => x.Id.Equals(FavoriteId) && x.IsDelete == false).FirstOrDefault();
            if (localFavorite == null)
            {
                return Constants.NotFoundResponse("Favorite Id Not Found!", null);
            }
            var locaalFavoriteResponse = _map.Map<FavoriteResponse>(localFavorite);
            var localListQuote = _dbContext.Quotes.Where(x => x.IsDelete == false).ToList();
            foreach (var item in localListQuote)
            {
                if (item.Id == locaalFavoriteResponse.QuoteId)
                {
                    locaalFavoriteResponse.Quote = _map.Map<FavoriteQuoteResponse>(item);
                    locaalFavoriteResponse.IsFavorite = true;
                }
            }
            return Constants.SuccessResponse("successfull", new { favorites = locaalFavoriteResponse });
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
