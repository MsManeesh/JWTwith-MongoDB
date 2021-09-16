using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Services;
namespace ReminderService.Controllers
{
    /*
    * As in this assignment, annotate
    * the class with [ApiController] annotation and define the controller level route as per REST Api standard. 
    * and Authorize the Reminder Controller with Authorize atrribute
    */
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReminderController : ControllerBase
    {
        /*
        * ReminderService should  be injected through constructor injection. 
        * Please note that we should not create Reminderservice object using the new keyword
        */
        IReminderService _reminderService;
        string userId = string.Empty;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        /* Implement HttpVerbs and its Functionalities asynchronously*/

        /*
        * Define a handler method which will get us the reminders by a userId.
        * 
        * This handler method should return any one of the status messages basis on
        * different situations: 
        * 1. 200(OK) - If the reminder found successfully.
        * 
        * This handler method should map to the URL using HTTP GET method
        * and also handle the custom exception for the same
        */

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString();
                if (userId != null)
                {
                    List<ReminderSchedule> reminderSchedules = await _reminderService.GetReminders(userId);
                    return Ok(reminderSchedules);
                }
                else
                    return Unauthorized();
                
            }
            catch (NoReminderFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /*
        * Define a handler method which will create a reminder by reading the
        * Serialized reminder object from request body and save the reminder in
        * reminder table in database. 
        * This handler method should return any one of the status messages
        * basis on different situations: 
        * 1. 201(CREATED - In case of successful creation of the reminder 
        * 2. 409(CONFLICT) - In case of duplicate reminder ID
        * This handler method should use HTTP POST
        * method".
        */
        [HttpPost]
        public async Task<IActionResult> Post(Reminder reminder)
        {
            try
            {
                userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString();
                if (userId != null)
                {
                    bool flag = await _reminderService.CreateReminder(userId, reminder.Email, reminder.NewsReminders.FirstOrDefault());
                    return Created("", flag);
                }
                else
                    return Unauthorized();
                
            }
            catch (ReminderAlreadyExistsException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /*
        * Define a handler method which will delete a reminder from a database.
        * This handler method should return any one of the status messages basis on
        * different situations: 
        * 1. 200(OK) - If the reminder deleted successfully from database. 
        * 2. 404(NOT FOUND) - If the reminder with specified userId with newsId is  not found. 
        * This handler method should map to HTTP Delete.
        */
        [HttpDelete]
        //[Route("{newsId:int}")]
        public async Task<IActionResult> Delete(int newsId)
        {
            try
            {
                userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString();
                if (userId != null)
                {
                    bool flag = await _reminderService.DeleteReminder(userId, newsId);
                    return Ok(flag);
                }
                else
                    return Unauthorized();
                
            }
            catch (NoReminderFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /*
         * Define a handler method (Put) which will update a reminder by userId,newsId and with Reminder Details
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news updated successfully to the database using userId with newsId
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should be used to update the existing reminder details.
         */
        [HttpPut]
        public async Task<IActionResult> Put(ReminderSchedule reminderSchedule)
        {
            try
            {
                userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString();
                if (userId != null)
                {
                    bool flag = await _reminderService.UpdateReminder(userId, reminderSchedule);
                    return Ok(flag);
                }
                else
                    return Unauthorized();
                
            }
            catch (NoReminderFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }
}
