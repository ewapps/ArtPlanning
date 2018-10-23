using ArtPlanning.Models;
using ArtPlanning.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtPlanning.Models.Repositories
{
    public class TaskRepository
    {
        public IEnumerable<TaskViewModel> GetTasks(int folderId)
        {
            using (ARTSHIPPERSDatabase db = new ARTSHIPPERSDatabase())
            {
                IEnumerable<TaskViewModel> tasks = (from t in db.task
                                                    join tt in db.task_type on t.task_type_id equals tt.id
                                                    join a in db.address on t.address_id equals a.id
                                                    join m in db.mission on t.mission_id equals m.id
                                                    join f in db.folder  on m.folder_id equals f.id
                                                    where f.id == folderId && !t.scheduled_start_date.HasValue
                                                    select new TaskViewModel
                                                    {
                                                       ID = t.id,
                                                       FolderID = f.id,
                                                       MissionID = m.id,
                                                       ScheduledStartDate = t.scheduled_start_date,
                                                       ScheduledEndDate = t.scheduled_end_date,
                                                       Fixed = t.@fixed.Value,
                                                       Title = t.title,
                                                       StreetLine1 = a.street_line1,
                                                       Number = a.number,
                                                       Box = a.box,
                                                       Zip = a.zip,
                                                       City = a.city,
                                                       Type = tt.code
                                                    }).ToList();

                return tasks;
            }
        }

        public int GetUnsignedTasksCount()
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {
                return (from t in database.task
                        join m in database.mission on t.mission_id equals m.id
                        join f in database.folder on m.folder_id equals f.id
                        where !t.scheduled_start_date.HasValue && (!f.closed.HasValue || f.closed.Value == false)
                        select t).Count();
            }
        }
    }
}