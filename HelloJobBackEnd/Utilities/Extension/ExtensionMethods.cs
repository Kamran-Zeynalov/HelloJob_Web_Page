using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Comparer;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace HelloJobBackEnd.Utilities.Extension
{
    public static class ExtensionMethods
    {

        public static async Task<string> CreateImage(this IFormFile file, string imagepath, string folder)
        {
            var destinationpath = Path.Combine(imagepath, folder);
            Random r = new();
            int random = r.Next(0, 1000);
            var filename = string.Concat(random, file.FileName);
            var path = Path.Combine(destinationpath, filename);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filename;
        }

        public static bool DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
        public static bool IsValidLength(this IFormFile file, double size)
        {
            return (double)file.Length / 1024 / 1024 <= size;
        }

        public static bool IsValidFile(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }


        public static List<Cv> RelatedByBusinessArea(IQueryable<Cv> queryable, Cv cv, int id)
        {
            List<Cv> relatedCvs = new();
            if (cv.BusinessAreaId != 0)
            {
                List<Cv> relatedByBusinessArea = queryable
                  .Include(e => e.Education)
                   .Include(e => e.Experience)
                   .Include(c => c.City)
                   .Include(c => c.BusinessArea)
                   .Include(o => o.OperatingMode)
                   .Include(x => x.User).
                   Include(x => x.WishListItems).ThenInclude(x => x.WishList)
                    .AsEnumerable()
                    .Where(p =>
                        p.BusinessAreaId == cv.BusinessAreaId && p.TimeIsOver == false &&
                        p.Id != id &&
                        !relatedCvs.Contains(p, new CvComparer())
                    )
                    .Take(6).ToList();

                relatedCvs.AddRange(relatedByBusinessArea);
            }

            return relatedCvs;
        }


        public static List<Vacans> RelatedByBusinessArea(IQueryable<Vacans> queryable, Vacans vacans, int id)
        {
            List<Vacans> relatedvacanss = new();
            if (vacans.BusinessAreaId != 0)
            {
                List<Vacans> relatedByBusinessArea = queryable.
                 Include(v => v.BusinessArea).
              Include(e => e.Education).
            Include(e => e.Experience).
            Include(c => c.City).
            Include(c => c.Company).
            Include(c => c.BusinessArea).ThenInclude(b => b.BusinessTitle).
            Include(i => i.infoEmployeers).
             Include(i => i.InfoWorks).
            Include(o => o.OperatingMode)
                    .AsEnumerable()
                    .Where(p =>
                        p.BusinessAreaId == vacans.BusinessAreaId && p.TimeIsOver == false &&
                        p.Id != id &&
                        !relatedvacanss.Contains(p, new VacansComparer())
                    )
                    .Take(6).ToList();

                relatedvacanss.AddRange(relatedByBusinessArea);
            }

            return relatedvacanss;
        }

    }
}
