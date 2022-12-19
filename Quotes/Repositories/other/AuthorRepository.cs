using AutoMapper;
using Quotes.DTO.Requests;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Models;
using Quotes.Repositories.Interfaces;

namespace Quotes.Repositories.other
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _map;

        public AuthorRepository(DBContext dBContext, IMapper map)
        {
            this._dbContext = dBContext;
            this._map = map;
        }


        public OperationType Add(AddAuthorRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            if (!Constants.InputLength(request, 4).Status)
            {
                return Constants.InputLength(request, 4);
            }

            var localItem = _dbContext.Authors.Where(
                x => x.Name == request.Name &&
                x.BirthYear == request.BirthYear &&
                x.BirthCountry == request.BirthCountry &&
                x.DeathYear == request.DeathYear
                ).FirstOrDefault();

            if (localItem != null && localItem.IsDelete == true)
            {
                localItem.IsDelete = false;
                _dbContext.Authors.Update(localItem);
                _dbContext.SaveChanges();
                return Constants.SuccessResponse("Add Author successfully", new { Author = _map.Map<AuthorResponse>(localItem) });
            }
            else if (localItem != null && localItem.IsDelete == false)
            {
                return Constants.SuccessResponse("Author Already Exist!", new { Author = _map.Map<AuthorResponse>(localItem) });
            }
            var currentItem = _map.Map<Author>(request);
            _dbContext.Authors.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Add Author successfully", new { Author = _map.Map<AuthorResponse>(currentItem) });
        }

        public OperationType Update(int AuthorId, AddAuthorRequest request)
        {
            var localItem = _dbContext.Authors.Where(x => x.Id.Equals(AuthorId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Author Id not exists!", null);
            }
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            if (!Constants.InputLength(request, 4).Status)
            {
                return Constants.InputLength(request, 4);
            }

            localItem.UpdatedAt = DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt");
            var currentItem = _map.Map(request, localItem);
            _dbContext.Authors.Update(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Update Author successfully", new { Author = _map.Map<AuthorResponse>(currentItem) });
        }

        public OperationType Delete(int AuthorId)
        {
            var localItem = _dbContext.Authors.Where(x => x.Id.Equals(AuthorId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Author Id Not Found!", null);
            }
            localItem.IsDelete = true;
            _dbContext.Authors.Update(localItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Author Deleted successfully", new { Author = _map.Map<AuthorResponse>(localItem) });
        }

        public OperationType GetAll()
        {
            var localList = _dbContext.Authors.Where(x => x.IsDelete == false).ToList();
            var filter = _map.Map<List<Author>, List<AuthorResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Authors = filter });
        }

        public OperationType GetById(int AuthorId)
        {
            var localList = _dbContext.Authors.Where(x => x.Id.Equals(AuthorId) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return Constants.NotFoundResponse("Author Id not exists!", null);
            }
            return Constants.SuccessResponse("successfull", new { Author = _map.Map<AuthorResponse>(localList) });
        }

        public OperationType Clear()
        {
            var list = _dbContext.Authors.ToList();
            _dbContext.Authors.RemoveRange(list);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("All Authors have been successfully deleted!", null);
        }


    }
}
