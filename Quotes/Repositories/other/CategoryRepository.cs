using AutoMapper;
using Quotes.Models;
using Quotes.DTO.Requests;
using Quotes.DTO.Responses;
using Quotes.Helper;
using Quotes.Repositories.Interfaces;

namespace Resturants.Repositories.other
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _map;

        public CategoryRepository(DBContext dBContext, IMapper map)
        {
            this._dbContext = dBContext;
            this._map = map;
        }


        public OperationType Add(AddCategoryRequest request)
        {
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            var localCategory = _dbContext.Categories.Where(x => x.Name == request.Name).SingleOrDefault();
            if (localCategory != null && localCategory.IsDelete == true)
            {
                localCategory.IsDelete = false;
                _dbContext.Categories.Update(localCategory);
                _dbContext.SaveChanges();
                return Constants.SuccessResponse("Add Category successfully", new { Category = _map.Map<CategoryResponse>(localCategory) });
            }
            else if (localCategory != null && localCategory.IsDelete == false)
            {
                return Constants.BadRequestResponse("Category Name Already Exist!", null);
            }
            var currentItem = _map.Map<Category>(request);
            _dbContext.Categories.Add(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Add Category successfully", new { Category = _map.Map<CategoryResponse>(currentItem) });
        }

        public OperationType Update(int CategoryId, AddCategoryRequest request)
        {
            var localItem = _dbContext.Categories.Where(x => x.Id.Equals(CategoryId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Category Id not exists!", null);
            }
            if (!Constants.InputValidation(request).Status)
            {
                return Constants.InputValidation(request);
            }
            localItem.UpdatedAt = DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt");
            var currentItem = _map.Map(request, localItem);
            _dbContext.Categories.Update(currentItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Update Category successfully", new { Category = _map.Map<CategoryResponse>(currentItem) });
        }

        public OperationType Delete(int CategoryId)
        {
            var localItem = _dbContext.Categories.Where(x => x.Id.Equals(CategoryId) && x.IsDelete == false).SingleOrDefault();
            if (localItem == null)
            {
                return Constants.NotFoundResponse("Category Id Not Found!", null);
            }
            localItem.IsDelete = true;
            _dbContext.Categories.Update(localItem);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("Category Deleted successfully", new { Category = _map.Map<CategoryResponse>(localItem) });
        }

        public OperationType GetAll()
        {
            var localList = _dbContext.Categories.Where(x => x.IsDelete == false).ToList();
            var filter = _map.Map<List<Category>, List<CategoryResponse>>(localList);
            return Constants.SuccessResponse("successfull", new { Categories = filter });
        }

        public OperationType GetById(int CategoryId)
        {
            var localList = _dbContext.Categories.Where(x => x.Id.Equals(CategoryId) && x.IsDelete == false).FirstOrDefault();
            if (localList == null)
            {
                return Constants.NotFoundResponse("Category Id not exists!", null);
            }
            return Constants.SuccessResponse("successfull", new { Category = _map.Map<CategoryResponse>(localList) });
        }

        public OperationType Clear()
        {
            var list = _dbContext.Categories.ToList();
            _dbContext.Categories.RemoveRange(list);
            _dbContext.SaveChanges();
            return Constants.SuccessResponse("All Categories have been successfully deleted!", null);
        }


    }
}
