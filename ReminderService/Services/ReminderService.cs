using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ReminderService.Services
{
    public class ReminderService:IReminderService
    {
        /*Inherit the respective interface and implement the methods in 
         the class i.e ReminderService by inheriting IReminderService
         */

        /* ReminderRepository should  be injected through constructor injection. 
         * Please note that we should not create ReminderRepository object using the new keyword
         */
        IReminderRepository _reminderRepository;
        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task<bool> CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            bool flag = await _reminderRepository.IsReminderExists(userId, schedule.NewsId);
            if (!flag)
            {
                await _reminderRepository.CreateReminder(userId, email, schedule);
                return true;
            }
            else
                throw new ReminderAlreadyExistsException($"This News already have a reminder");
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            bool flag = await _reminderRepository.DeleteReminder(userId, newsId);
            if (flag)
            {
                return flag;

            }
            else
                throw new NoReminderFoundException("No reminder found for this news");
        }

        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            List<ReminderSchedule> reminderSchedules = await _reminderRepository.GetReminders(userId);
            if (reminderSchedules != null)
                return reminderSchedules;
            else
                throw new NoReminderFoundException("No reminders found for this user");
        }

        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            bool flag = await _reminderRepository.UpdateReminder(userId, reminder);
            if (flag)
            {
                return flag;

            }
            else
                throw new NoReminderFoundException("No reminder found for this news");
        }
        /* Implement all the methods of respective interface asynchronously*/

        /* Implement GetReminders method which should be used to get all reminders by userId.*/

        /* Implement CreateReminder method which should be used to create a new reminder using userId, email
           and reminder Object*/

        /* Implement DeleteReminder method which should be used to delete a reminder by userId and newsId*/

        /* Implement a UpdateReminder method which should be used to update an existing reminder by using
         userId and reminder details*/
    }
}
