using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NewsService.Services
{
    public class NewsService:INewsService
    {
        /*
       * NewsRepository should  be injected through constructor injection. 
       * Please note that we should not create NewsRepository object using the new keyword
       */
        readonly INewsRepository repository;
        public NewsService(INewsRepository newsRepository)
        {
            repository = newsRepository;
        }

        

        /* Implement all the methods of respective interface asynchronously*/

        /* Implement CreateNews method to add the new news details*/
        public async Task<int> CreateNews(string userId, News news)
        {
            bool flag = await repository.IsNewsExist(userId, news.Title);
            if (flag == false)
            {
                return await repository.CreateNews(userId, news);
            }
            else
                throw new NewsAlreadyExistsException($"{userId} have already added this news");
        }
        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            News news = await repository.GetNewsById(userId, newsId);
            if (news != null)
            {
                return await repository.AddOrUpdateReminder(userId, newsId, reminder);
            }
            else
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
        }
        /* Implement DeleteNews method to remove the existing news*/
        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            bool flag = await repository.DeleteNews(userId, newsId);
            if (flag)
            {
                return flag;
            }
            else
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
        }
        /* Implement DeleteReminder method to delte the Reminder using userId*/
        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            bool flag = await repository.IsReminderExists(userId, newsId);
            if (flag)
            {
                return await repository.DeleteReminder(userId, newsId);
            }
            else
                throw new NoReminderFoundException("No reminder found for this news");
        }
        /* Implement FindAllNewsByUserId to get the News Details by userId*/
        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            List<News> newsList = await repository.FindAllNewsByUserId(userId);
            if (newsList != null)
                return newsList;
            else
                throw new NoNewsFoundException($"No news found for {userId}");
        }
    }
}
