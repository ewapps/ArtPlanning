using ArtPlanning.Models.Database;
using System.Linq;

namespace ArtPlanning.Models.Repositories
{
    public class FolderRepository
    {
        public int GetUnclosedFoldersCount()
        {
            using (ARTSHIPPERSDatabase database = new ARTSHIPPERSDatabase())
            {                
                var innerQuery = (from m in database.mission
                                  join t in database.task on m.id equals t.mission_id
                                  where !t.scheduled_start_date.HasValue
                                  select m.folder_id);

                var unclosedfoldersCount = (from f in database.folder
                                            where innerQuery.Contains(f.id) && (!f.closed.HasValue || f.closed.Value == false) select f).Count();

                return unclosedfoldersCount;
            }
        }
    }
}