using MongoDB.Driver;
using ReminderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReminderService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e ReminderRepository by inheriting IReminderRepository class 
    //which is used to implement all Data access operations
    public class ReminderRepository:IReminderRepository
    {
        //define a private variable to represent Reminder Database Context
        ReminderContext _reminderContext;
        public ReminderRepository(ReminderContext reminderContext)
        {
            _reminderContext = reminderContext;
        }

        public async Task CreateReminder(string userId, string email, ReminderSchedule schedule)
        {
            Reminder reminder = await _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            if (reminder == null)
            {
                Reminder newReminder = new Reminder() { UserId = userId, Email = email, NewsReminders = new List<ReminderSchedule>() { schedule } };
                await _reminderContext.Reminders.InsertOneAsync(newReminder);
            }
            else
            {
                reminder.NewsReminders.Add(schedule);
                await _reminderContext.Reminders.ReplaceOneAsync(x => x.UserId == userId, reminder);
            }
        }

        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            bool flag = false;
            Reminder reminder = await _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            if (reminder != null)
            {
                ReminderSchedule reminderSchedule = reminder.NewsReminders.FirstOrDefault(x => x.NewsId == newsId);
                if (reminderSchedule != null)
                {
                    
                    foreach(ReminderSchedule x in reminder.NewsReminders)
                    {
                        if (x.NewsId == newsId)
                        {
                            reminder.NewsReminders.Remove(x);
                            break;
                        }
                    }
                    await _reminderContext.Reminders.ReplaceOneAsync(x => x.UserId == userId, reminder);
                    flag = true;
                }
            }
            return flag;

        }

        public async Task<List<ReminderSchedule>> GetReminders(string userId)
        {
            Reminder reminder = await _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            if (reminder != null)
                return reminder.NewsReminders;
            else
                return null;


        }

        public async Task<bool> IsReminderExists(string userId, int newsId)
        {
            bool flag = false;
            Reminder reminder = await _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            if (reminder != null && reminder.NewsReminders!= null)
            {
                foreach (ReminderSchedule x in reminder.NewsReminders)
                {
                    if (x.NewsId == newsId)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        public async Task<bool> UpdateReminder(string userId, ReminderSchedule reminder)
        {
            bool flag = false;
            Reminder existReminder = await _reminderContext.Reminders.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            if (existReminder != null && existReminder.NewsReminders != null)
            {
                foreach (ReminderSchedule x in existReminder.NewsReminders)
                {
                    if (x.NewsId == reminder.NewsId)
                    {
                        x.Schedule = reminder.Schedule;
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                    await _reminderContext.Reminders.ReplaceOneAsync(x => x.UserId == userId, existReminder);
            }

            return flag;
        }
        //Implement the methods of interface Asynchronously.

        // Implement CreateReminder method which should be used to save a new reminder.  

        // Implement DeleteReminder method which should be used to delete an existing reminder.

        // Implement GetReminders method which should be used to get a reminder by userId.

        // Implement IsReminderExists method which should be used to check an existing reminder by newsId

        // Implement UpdateReminder method which should be used to update an existing reminder using  userId and 
        // reminder Schedule
    }
}
